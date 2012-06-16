using System;
using System.Windows;
using ImageService.Contracts;
using System.ServiceModel;
using ImageService.Common;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fields
        private ChannelFactory<IImageService> channelFactory;
        private ImageServiceClientManager manager;
        private DataGridModel model;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            model = (DataGridModel)imageFilesGrid.DataContext;
            IImageService channel = null;
            try
            {
                channelFactory = new ChannelFactory<IImageService>("streamingBinding");
                channel = channelFactory.CreateChannel();
            }
            catch (Exception)
            {
                MessageBox.Show("Can't create client channel!");
                return;
            }
            IClientNotifier notyfier = new WPFNotifier();
            manager = new ImageServiceClientManager(channel, notyfier, UpdateImagesInfoGrid);

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedFileName = string.Empty;
            if (imageFilesGrid.SelectedItem != null)
                selectedFileName = ((ImageFileData)imageFilesGrid.SelectedItem).FileName;

            model.ImagesFileData = new ObservableCollection<ImageFileData>(manager.GetAllImagesInfo(false));

            if (selectedFileName != string.Empty)
            {
                ImageFileData rowObject = model.ImagesFileData.SingleOrDefault(p => p.FileName.Equals(selectedFileName));
                if (rowObject != null)
                {
                    int index = model.ImagesFileData.IndexOf(rowObject);
                    imageFilesGrid.ScrollIntoView(imageFilesGrid.Items[index]);
                    DataGridRow row = (DataGridRow)imageFilesGrid.ItemContainerGenerator.ContainerFromIndex(index);
                    row.IsSelected = true;
                }
            }
        }

        private void Upload()
        {
            string uploadedFileName = string.Empty;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"C:\Users\bdudnik\Pictures";
            dlg.Filter = "Image Files |*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
            dlg.RestoreDirectory = true;

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                manager.UploadImage(dlg.FileName);
                uploadedFileName = Path.GetFileName(dlg.FileName);
                UpdateImagesInfoGrid();
            }

            ImageFileData rowObject = model.ImagesFileData.SingleOrDefault(p => p.FileName.Equals(uploadedFileName));
            if (rowObject != null)
            {
                int index = model.ImagesFileData.IndexOf(rowObject);
                imageFilesGrid.ScrollIntoView(imageFilesGrid.Items[index]);
                DataGridRow row = (DataGridRow)imageFilesGrid.ItemContainerGenerator.ContainerFromIndex(index);
                row.IsSelected = true;
            }
        }

        private void ResetConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.UpdateImageServiceProxy(channelFactory.CreateChannel());
            }
            catch (Exception)
            {
                MessageBox.Show("Can't create client channel!");
            }
        }

        private void UpdateImagesInfoGrid()
        {
            model.ImagesFileData = new ObservableCollection<ImageFileData>(manager.GetAllImagesInfo(false));
            imageFilesGrid.Dispatcher.Invoke(new Action(delegate { imageFilesGrid.Items.Refresh(); }));
        }

        private void imageFilesGrid_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string imageName = string.Empty;
            byte[] imageData = ((ImageFileData)((DataGrid)sender).SelectedItem).ImageData;
            if (imageData == null)
                imageData = manager.DownloadImage(((ImageFileData)((DataGrid)sender).SelectedItem).FileName);
            if (imageData == null)
                return;
            BitmapImage bitmap = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            ImageViewer1.Source = bitmap;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            channelFactory.Close();
        }

        private bool CanExecute()
        {
            return true;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            string uploadedFileName = string.Empty;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"C:\Users\bdudnik\Pictures";
            dlg.Filter = "Image Files |*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
            dlg.RestoreDirectory = true;

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                manager.UploadImage(dlg.FileName);
                uploadedFileName = Path.GetFileName(dlg.FileName);
                UpdateImagesInfoGrid();
            }

            ImageFileData rowObject = model.ImagesFileData.SingleOrDefault(p => p.FileName.Equals(uploadedFileName));
            if (rowObject != null)
            {
                int index = model.ImagesFileData.IndexOf(rowObject);
                imageFilesGrid.ScrollIntoView(imageFilesGrid.Items[index]);
                DataGridRow row = (DataGridRow)imageFilesGrid.ItemContainerGenerator.ContainerFromIndex(index);
                row.IsSelected = true;
            }
        }


    }
}
