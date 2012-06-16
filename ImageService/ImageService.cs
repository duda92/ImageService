using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.IO;
using System.Collections;
using System.Configuration;

using ImageService;
using ImageService.Contracts;

namespace ImageServicing
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession)]
    public class ImageService : IImageService
    {

        private Dictionary<string, int> sessionTable = new Dictionary<string,int>();

        public bool Status()
        {
            Log("Status");
            int lastUpdateImagesCount = 0;
            if (sessionTable.Keys.Contains(OperationContext.Current.SessionId))
            {
                lastUpdateImagesCount = sessionTable.FirstOrDefault(p => p.Key == OperationContext.Current.SessionId).Value;
                if (GetImagesCount() == lastUpdateImagesCount)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
 
        public IEnumerable<ImageFileData>  GetAllImagesList(bool withFilesData)
        {
            Log("GetAllImagesList");
            List<ImageFileData> images_collection = new List<ImageFileData>();
            try
            {
                foreach (FileInfo fileInfo in GetAllImageFiles())
                {
                    ImageFileData imageFileData = new ImageFileData() { FileName = fileInfo.Name, LastDateModified = fileInfo.LastWriteTime };
                    byte[] imageBytes = null;
                    if (withFilesData)
                        imageBytes = File.ReadAllBytes(fileInfo.FullName);
                    imageFileData.ImageData = imageBytes;
                    images_collection.Add(imageFileData);
                }
            }
            catch (Exception e)
            {
                ServerFault fault = new ServerFault();
                fault.FriendlyDiscription = "Some problems just has been occured on server!";
                fault.Discription = e.Message;
                throw new FaultException<ServerFault>(fault, new FaultReason(fault.Discription));
            }

            AddUpdateToSessionTable(images_collection.Count);
            return images_collection;
        }

        private void AddUpdateToSessionTable(int updateCount)
        {
            if (sessionTable.Keys.Contains(OperationContext.Current.SessionId))
                sessionTable[OperationContext.Current.SessionId] = updateCount;
            else
                sessionTable.Add(OperationContext.Current.SessionId, updateCount);
        }

        public ImageFileData GetImageByName(string request_file_name)
        {
            Log("GetImageByName");
            try
            {
                if (string.IsNullOrEmpty(request_file_name))
                    throw new ArgumentException("Invalid requested file name!", request_file_name);   

                IEnumerable<FileInfo> allImageFilesList = GetAllImageFiles();
                FileInfo requestedImageFile = null;
                requestedImageFile = allImageFilesList.SingleOrDefault(f => f.Name == request_file_name);              
                if (requestedImageFile == null)
                    throw new ArgumentException("File that you has requested doesn't exist!", request_file_name);

                ImageFileData imageFileData = new ImageFileData() { FileName = requestedImageFile.Name, LastDateModified = requestedImageFile.LastWriteTime };
                byte[] imageBytes = File.ReadAllBytes(requestedImageFile.FullName);
                imageFileData.ImageData = imageBytes;
                return imageFileData;
            }
            catch (Exception e)
            {  
                ServerFault fault = new ServerFault();
                fault.FriendlyDiscription = "Some problems just has been occured on service host!";
                fault.Discription = e.Message;
                throw new FaultException<ServerFault>(fault, new FaultReason(fault.Discription));
            }
        }

        public void UploadImage(ImageFileData uploading_image)
        {
            Log("UploadImage");
            try
            {
                if (string.IsNullOrEmpty(uploading_image.FileName))
                    throw new ArgumentException("Invalid upldoading file name", uploading_image.FileName);

                if (uploading_image.ImageData == null || uploading_image.ImageData.Length == 0)
                    throw new ArgumentException("Uploaded file-data is empty!");

                string newImageFileName = uploading_image.FileName;
                string uploadFolder = ConfigurationManager.AppSettings["UploadFolder"];
                string newImageFilePath = Path.Combine(uploadFolder, newImageFileName);

                try
                {
                    using (Stream targetStream = new FileStream(newImageFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        targetStream.Write(uploading_image.ImageData, 0, uploading_image.ImageData.Length);
                    }
                }
                catch (IOException)
                { 
                    throw new IOException("Error writing new file or overwriting existed file");
                }
            }
            catch (Exception e)
            {
                ServerFault fault = new ServerFault();
                fault.FriendlyDiscription = "Some problems just has been occured on service host!";
                fault.Discription = e.Message;
                throw new FaultException<ServerFault>(fault, new FaultReason(fault.Discription));
            }
        }

        private IEnumerable<FileInfo> GetAllImageFiles()
        {
            string   uploadFolder    = ConfigurationManager.AppSettings["UploadFolder"];
            string[] imageExtentions = ConfigurationManager.AppSettings["ImageSearchPattern"].Split(' ');

            IEnumerable<FileInfo> imageFilesList = null;
            try
            {
                if(!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);
                DirectoryInfo dir = new DirectoryInfo(uploadFolder);
                imageFilesList = (from file in dir.GetFiles("*.*", SearchOption.AllDirectories)
                                 where imageExtentions.Contains(file.Extension)
                                 select file).ToArray();
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Service directory with images: " + uploadFolder + " not found!");
            }

            return imageFilesList;
        }

        private int GetImagesCount()
        { 
            int imagesCount = 0;
            try
            {
                imagesCount = (new List<FileInfo>(GetAllImageFiles())).Count;
            }
            catch (Exception e)
            {
                ServerFault fault = new ServerFault();
                fault.FriendlyDiscription = "Some problems just has been occured on server!";
                fault.Discription = e.Message;
                throw new FaultException<ServerFault>(fault, new FaultReason(fault.Discription));
            }
            return imagesCount;
        }

        private void Log(string methodName)
        {
            Console.WriteLine("{0} is calling by {1}", methodName, OperationContext.Current.SessionId);
            Console.WriteLine();
        }
    }
}
