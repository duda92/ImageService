namespace WindowsFormsImageServiceClient
{
    partial class ImageServiceClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                channelFactory.Close();
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.UpdateListButton = new System.Windows.Forms.Button();
            this.UploadButton = new System.Windows.Forms.Button();
            this.ImagesInfoGrid = new System.Windows.Forms.DataGridView();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resetConnectButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ImagesInfoGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateListButton
            // 
            this.UpdateListButton.Location = new System.Drawing.Point(27, 12);
            this.UpdateListButton.Name = "UpdateListButton";
            this.UpdateListButton.Size = new System.Drawing.Size(75, 23);
            this.UpdateListButton.TabIndex = 2;
            this.UpdateListButton.Text = "Update";
            this.UpdateListButton.UseVisualStyleBackColor = true;
            this.UpdateListButton.Click += new System.EventHandler(this.UpdateListButton_Click);
            // 
            // UploadButton
            // 
            this.UploadButton.Location = new System.Drawing.Point(27, 41);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(75, 23);
            this.UploadButton.TabIndex = 3;
            this.UploadButton.Text = "Upload";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // ImagesInfoGrid
            // 
            this.ImagesInfoGrid.AllowUserToAddRows = false;
            this.ImagesInfoGrid.AllowUserToDeleteRows = false;
            this.ImagesInfoGrid.AllowUserToResizeColumns = false;
            this.ImagesInfoGrid.AllowUserToResizeRows = false;
            this.ImagesInfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagesInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ImagesInfoGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ImagesInfoGrid.EnableHeadersVisualStyles = false;
            this.ImagesInfoGrid.Location = new System.Drawing.Point(12, 12);
            this.ImagesInfoGrid.MultiSelect = false;
            this.ImagesInfoGrid.Name = "ImagesInfoGrid";
            this.ImagesInfoGrid.RowHeadersVisible = false;
            this.ImagesInfoGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ImagesInfoGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ImagesInfoGrid.Size = new System.Drawing.Size(509, 134);
            this.ImagesInfoGrid.TabIndex = 1;
            this.ImagesInfoGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ImagesInfoGrid_CellMouseDoubleClick);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(12, 152);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(642, 472);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.resetConnectButton);
            this.panel1.Controls.Add(this.UpdateListButton);
            this.panel1.Controls.Add(this.UploadButton);
            this.panel1.Location = new System.Drawing.Point(527, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(127, 134);
            this.panel1.TabIndex = 4;
            // 
            // resetConnectButton
            // 
            this.resetConnectButton.Location = new System.Drawing.Point(27, 71);
            this.resetConnectButton.Name = "resetConnectButton";
            this.resetConnectButton.Size = new System.Drawing.Size(75, 41);
            this.resetConnectButton.TabIndex = 4;
            this.resetConnectButton.Text = "Reset Connect";
            this.resetConnectButton.UseVisualStyleBackColor = true;
            this.resetConnectButton.Click += new System.EventHandler(this.resetConnectButton_Click);
            // 
            // ImageServiceClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 636);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.ImagesInfoGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ImageServiceClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImageServiceClient";
            this.Load += new System.EventHandler(this.ImageServiceClientForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ImagesInfoGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UpdateListButton;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.DataGridView ImagesInfoGrid;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button resetConnectButton;
    }
}

