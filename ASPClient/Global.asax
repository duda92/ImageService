<%@ Application Language="C#" %>
<%@ Import Namespace="ImageService.Common" %>
<%@ Import Namespace="ImageService.Contracts" %>
<%@ Import Namespace="System.ServiceModel" %>
<%@ Import Namespace="WebNotifying" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        System.Diagnostics.Debug.WriteLine("Application_Start");
        System.Web.Caching.Cache cache = System.Web.HttpContext.Current.Cache;
        cache.Insert("UpdateClientProcessor", new UpdateClientProcessor(), null,
            Cache.NoAbsoluteExpiration,
            new TimeSpan(0, 0, 0),
            CacheItemPriority.Default, new CacheItemRemovedCallback(this.RemovedCallback));  
    }
    
    void Application_End(object sender, EventArgs e) 
    {
    
    }
        
    void Application_Error(object sender, EventArgs e) 
    {
    
    }

    void Session_Start(object sender, EventArgs e) 
    {
        ImageServiceClientManager Manager;
        IEnumerable<ImageFileData> ImagesFileData;
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

        Manager = new ImageServiceClientManager(channel, Notyfier, null);
        ImagesFileData = Manager.GetAllImagesInfo(true);       
        Session["Manager"] = Manager;
        Session["ImagesFileData"] = ImagesFileData;
    }

    void Session_End(object sender, EventArgs e) 
    {
        ImageServiceClientManager manager = (ImageServiceClientManager)Session["Manager"];
        if (manager != null)
            manager.Dispose();
    }

    public void RemovedCallback(String key, object value,
        CacheItemRemovedReason removedReason)
    {
        System.Web.HttpContext.Current.Cache["UpdateClientProcessor"] = new UpdateClientProcessor();
    }
</script>
