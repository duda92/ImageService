using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.IO;
using ImageService.Common;
using ImageService.Contracts;
using System.ServiceModel;

namespace WindowsFormsImageServiceClient
{
    public partial class ImageServiceClientForm : Form
    {
        public ImageServiceClientForm()
        {
            InitializeComponent();
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
            IClientNotifier notyfier = new WindowsFormsNotifier();
            manager = new ImageServiceClientManager(channel, notyfier, UpdateImagesInfoGrid);
        }

        private void UpdateListButton_Click(object sender, EventArgs e)
        {
            UpdateImagesInfoGrid();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            string uploadedRowFileName = string.Empty;
            
            using (OpenFileDialog openImageFileDialog = new OpenFileDialog())
            {
                openImageFileDialog.InitialDirectory = @"C:\Users\bdudnik\Pictures";
                openImageFileDialog.Filter = "Image Files |*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
                openImageFileDialog.FilterIndex = 2;
                openImageFileDialog.RestoreDirectory = true;
                if (openImageFileDialog.ShowDialog() == DialogResult.OK)
                {
                    uploadedRowFileName = Path.GetFileName(openImageFileDialog.FileName);
                    manager.UploadImage(openImageFileDialog.FileName);
                    UpdateImagesInfoGrid();
                }
            }
            var dataSource = (BindingList<ImageFileData>)ImagesInfoGrid.DataSource;
            ImageFileData rowObject = dataSource.SingleOrDefault(p => p.FileName.Equals(uploadedRowFileName));
            if (rowObject != null)
                ImagesInfoGrid.Rows[dataSource.IndexOf(rowObject)].Selected = true;
        }

        private void ImageServiceClientForm_Load(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn fileNameColumn = new DataGridViewTextBoxColumn();
            fileNameColumn.DataPropertyName = "FileName";
            fileNameColumn.HeaderText = "File name";

            DataGridViewTextBoxColumn LastDateModifiedColumn = new DataGridViewTextBoxColumn();
            LastDateModifiedColumn.DataPropertyName = "LastDateModified";
            LastDateModifiedColumn.HeaderText = "Date of last modifying";

            DataGridViewTextBoxColumn ImageDataColumn = new DataGridViewTextBoxColumn();
            ImageDataColumn.DataPropertyName = "ImageData";

            ImagesInfoGrid.Columns.Add(fileNameColumn);
            ImagesInfoGrid.Columns.Add(LastDateModifiedColumn);
            ImagesInfoGrid.Columns.Add(ImageDataColumn);

            ImagesInfoGrid.Columns[0].Width = (int)(ImagesInfoGrid.Width * 0.5);
            ImagesInfoGrid.Columns[1].Width = (int)(ImagesInfoGrid.Width * 0.5);
            ImagesInfoGrid.Columns[2].Visible = false;
        }

        private void UpdateImagesInfoGrid()
        {
            string selectedRowFileName = string.Empty;
            if (ImagesInfoGrid.SelectedRows.Count != 0)
                selectedRowFileName = ((ImageFileData)ImagesInfoGrid.SelectedRows[0].DataBoundItem).FileName;
            IEnumerable<ImageFileData> imagesInfo = manager.GetAllImagesInfo(true);
            if (imagesInfo == null)
                return;
            ImagesInfoGrid.Rows.Clear();
            var bindingList = new BindingList<ImageFileData>(new List<ImageFileData>(imagesInfo));
            ImagesInfoGrid.Invoke(new MethodInvoker(delegate() { ImagesInfoGrid.DataSource = bindingList; }));

            ImageFileData rowObject = bindingList.SingleOrDefault(p => p.FileName.Equals(selectedRowFileName));    
            if (rowObject != null)
                ImagesInfoGrid.Rows[bindingList.IndexOf(rowObject)].Selected = true;
        }

        private void ImagesInfoGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string fileName = ((ImageFileData)ImagesInfoGrid.Rows[e.RowIndex].DataBoundItem).FileName;
            
            byte[] imageData = null;
            if (((ImageFileData)ImagesInfoGrid.Rows[e.RowIndex].DataBoundItem).ImageData == null)
                imageData = manager.DownloadImage(fileName);
            else
                imageData = ((ImageFileData)ImagesInfoGrid.Rows[e.RowIndex].DataBoundItem).ImageData;
            
            if (imageData == null)
                return;

            Image image = ByteArrayToImage(imageData);
            pictureBox.Image = image;
        }

        private Image ByteArrayToImage(byte[] imageData)
        {
            Image returnImage = null;
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                returnImage = Image.FromStream(ms);
            }
            return returnImage;
        }
        
        private void resetConnectButton_Click(object sender, EventArgs e)
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

        private ImageService.Common.ImageServiceClientManager manager;
        private ChannelFactory<IImageService> channelFactory;
        
        
    }
}
