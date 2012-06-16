using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImageService.Common;
using ImageService.Contracts;
using System.ServiceModel;
using WebNotifying;

public partial class ImageProductor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ImageServiceClientManager manager = (ImageServiceClientManager)Session["Manager"];
        IEnumerable<ImageFileData> imagesFileData = (IEnumerable<ImageFileData>)Session["ImagesFileData"];
        if (manager == null || imagesFileData == null)
            return;
        string fileName = Request.QueryString["fileName"];
        byte[] imageData = null;

        ImageFileData imageFileData = imagesFileData.SingleOrDefault(p => p.FileName == fileName);
        if (imageFileData != null && imageFileData.ImageData != null)
            imageData = imageFileData.ImageData;
        else
            imageData = manager.DownloadImage(fileName);
        if (imageData != null)
        {
            Response.BinaryWrite(imageData);
        }
    }
}