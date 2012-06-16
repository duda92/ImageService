using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ImageService;
using System.Threading;
using System.Timers;
using SystemTimer = System.Timers.Timer;
using ImageService.Contracts;


namespace ImageService.Common
{
    public class ImageServiceClientManager : IDisposable
    {
        private IFaultExceptionProcessor faultProcessor;
        private SystemTimer timer;
        private Action callBackAction;
        private Thread regularUpdateThread;

        #region Constructors

        public ImageServiceClientManager(IImageService imageService)
            : this(imageService, null, null) { }

        public ImageServiceClientManager(IImageService imageService, Action autoUpdateCallback)
            : this(imageService, null, autoUpdateCallback) { }

        public ImageServiceClientManager(IImageService imageService, IClientNotifier notifyer, Action autoUpdateCallback)
        {
            ImageServiceProxy = imageService;
            Notifier = notifyer;
            faultProcessor = new NotifierExceptionProcessor(notifyer);

            if (autoUpdateCallback != null) 
            {
                callBackAction = autoUpdateCallback;
                StartUpdateThread();
            }  
        }

        ~ImageServiceClientManager()
        {
            Dispose(false);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Special object for notifying client
        /// </summary>
        /// <value>
        /// Concrete notifier instance.
        /// </value>
        public IClientNotifier Notifier { get; private set; }

        /// <summary>
        /// Image service proxy.
        /// </summary>
        /// <value>
        /// The image service proxy.
        /// </value>
        public IImageService ImageServiceProxy { get; private set; }
        #endregion

        #region Public methods

        //-----------------------------------Upload----------------------------------------
        public void UploadImage(string fullFileName)
        {
            byte[] imageData = null;
            using (FileStream imageFile = File.Open(fullFileName, FileMode.Open))
            {
                imageData = new byte[imageFile.Length];
                imageFile.Read(imageData, 0, imageData.Length);
            }
            UploadImage(imageData, Path.GetFileName(fullFileName));
        }

        public void UploadImage(byte[] imageData, string imageName)
        {
            IEnumerable<ImageFileData> allImagesFileData = GetAllImagesInfo(false);
            if (allImagesFileData == null)
                return;
            if (allImagesFileData.SingleOrDefault(p => p.FileName == imageName) != null)
                faultProcessor.ProcessMessage("Image with the same name already exists");

            ImageFileData data = new ImageFileData() { FileName = imageName, LastDateModified = DateTime.Now, ImageData = imageData };
            try
            {
                ImageServiceProxy.UploadImage(data);
            }
            catch (Exception ex)
            {
                faultProcessor.ProcessException(ex);
                return;
            }
        }

        //-----------------------------------GetInfo----------------------------------------
        public IEnumerable<ImageFileData> GetAllImagesInfo(bool withLoad)
        {
            ImageFileData[] serviceImagesData = null;
            try
            {
                serviceImagesData = ImageServiceProxy.GetAllImagesList(withLoad).ToArray();
                
                if (callBackAction != null && timer.Enabled == false)
                    StartUpdate();
            }
            catch (Exception ex)
            {
                faultProcessor.ProcessException(ex);
                return null;
            }
            return serviceImagesData;
        }

        public IEnumerable<ImageFileData> GetAllImagesInTimeRange(DateTime leftRange, DateTime rightRange, bool withLoad)
        {
            IEnumerable<ImageFileData> serviceImagesData = GetAllImagesInfo(withLoad);
            serviceImagesData = (from image in serviceImagesData
                                where image.LastDateModified >= leftRange && image.LastDateModified <= rightRange
                                select image).ToArray();
            return serviceImagesData;
        }

        public IEnumerable<ImageFileData> GetAllImagesContainedString(string fileNamePart, bool withLoad)
        {
            IEnumerable<ImageFileData> serviceImagesData = GetAllImagesInfo(withLoad);
            serviceImagesData = (from image in serviceImagesData
                                 where image.FileName.Contains(fileNamePart)
                                 select image).ToArray();
            return serviceImagesData;
        }
        
        //-----------------------------------Download image----------------------------------------
        public byte[] DownloadImage(string fileName)
        {
            ImageFileData image = null;
            try
            {
                image = ImageServiceProxy.GetImageByName(fileName);
            }
            catch (Exception ex)
            {
                faultProcessor.ProcessException(ex);
                return null;
            }
            return image.ImageData;
        }

        public void UpdateImageServiceProxy(IImageService imageServiceProxy)
        {
            this.ImageServiceProxy = imageServiceProxy;    
        }

        #endregion

        public Nullable<bool> Status()
        {
            bool status = false;
            try
            {
                status = ImageServiceProxy.Status();
            }
            catch (Exception ex)
            {
                StopUpdate();
                faultProcessor.ProcessException(ex);
                return null;
            }
            return status;
        }

        #region Private methods

        protected void Dispose(bool disposing)
        {
            if(disposing)
                ((IDisposable)ImageServiceProxy).Dispose();
        }

        private void CheckStatus(Object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!ImageServiceProxy.Status())
                    callBackAction.Invoke();
            }
            catch
            {
                StopUpdate();
            }
        }

        private void StartUpdateThread()
        {
            if (callBackAction != null)
            {
                regularUpdateThread = new Thread(StartUpdate) { IsBackground = true };
                regularUpdateThread.Start();
            }          
        }
        
        private void StartUpdateThread(Action autoUpdateCallback)
        {
            callBackAction = autoUpdateCallback;
            if (callBackAction != null)
            {
                regularUpdateThread = new Thread(StartUpdate) { IsBackground = true };
                regularUpdateThread.Start();
            }          
        }
        
        private void StartUpdate()
        {
            timer = new SystemTimer(3000);    
            timer.Elapsed += new ElapsedEventHandler(CheckStatus);
            timer.Start();
            CheckStatus(null, null);
        }

        private void StopUpdate()
        {
            if (timer != null)
                timer.Stop();
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
