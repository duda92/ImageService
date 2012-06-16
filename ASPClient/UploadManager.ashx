<%@ WebHandler Language="C#" Class="AjaxFileUpload.Web.UploadManager" %>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using ImageService.Common;
using System.Web.SessionState;

namespace AjaxFileUpload.Web
{
    /// <summary>
    /// Summary description for ImageManager
    /// </summary>
    public class UploadManager : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string fileName = context.Request.QueryString.Get(0);

            byte[] fileBytes = new byte[context.Request.GetBufferlessInputStream().Length];
            using (Stream fileInputStream = context.Request.GetBufferlessInputStream())
                fileInputStream.Read(fileBytes, 0, fileBytes.Length);

            ImageServiceClientManager manager = (ImageServiceClientManager)HttpContext.Current.Session["Manager"];
            manager.UploadImage(fileBytes, fileName);
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }
        
        #endregion
    }
}