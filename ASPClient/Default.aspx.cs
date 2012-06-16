using System;
using System.Collections.Generic;
using ImageService.Contracts;
using ImageService.Common;
using System.ServiceModel;
using WebNotifying;

public partial class _Default : System.Web.UI.Page
{
    public ImageServiceClientManager Manager { get; set; }
    public IEnumerable<ImageFileData> ImagesFileData { get; set; }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ImagesFileData = (IEnumerable<ImageFileData>)Session["ImagesFileData"];
        Manager = (ImageServiceClientManager)Session["Manager"];

        ImagesFileData = Manager.GetAllImagesInfo(true);
        if (ImagesFileData != null)
        {
            GridView1.DataSource = ImagesFileData;
            GridView1.DataBind();
        }
    }

    protected void Update_Click(object sender, EventArgs e)
    {
        ImagesFileData = Manager.GetAllImagesInfo(false);
        if (ImagesFileData != null)
            GridView1.DataBind();
    }

    protected void ResetConnection_Click(object sender, EventArgs e)
    {
        ChannelFactory<IImageService> ChannelFactory;      
        IClientNotifier Notyfier = new WebNotifier();
        IImageService channel = null;
        try
        {
            ChannelFactory = new ChannelFactory<IImageService>("streamingBinding");
            channel = ChannelFactory.CreateChannel();
        }
        catch (Exception)
        {
            Notyfier.Error("Can't create client channel!");
            return;
        }
        ImageServiceClientManager Manager = new ImageServiceClientManager(channel, Notyfier, null);
        IEnumerable<ImageFileData> ImagesFileData = Manager.GetAllImagesInfo(true);
        Session["Manager"] = Manager;
        Session["ImagesFileData"] = ImagesFileData;
    }
}

