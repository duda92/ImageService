<%@ WebHandler Language="C#" Class="UpdateAsyncHandler" %>

using System;
using System.Web;
using System.Threading;
using ImageService.Common;
using System.Web.Caching;

public class UpdateAsyncHandler : IHttpAsyncHandler, System.Web.SessionState.IReadOnlySessionState
{
    private UpdateClientProcessor updateClientProcessor { get; set; }   
    #region IHttpAsyncHandler Members
    
    public IAsyncResult BeginProcessRequest(HttpContext ctx, AsyncCallback cb, Object obj)
    {
        Cache cache  = HttpContext.Current.Cache;
        updateClientProcessor = ((UpdateClientProcessor)cache["UpdateClientProcessor"]);
        AsyncRequestState currentAsyncState = new AsyncRequestState(ctx, cb, obj);
        ThreadPool.QueueUserWorkItem(new WaitCallback(RequestWorker), currentAsyncState);
        return currentAsyncState;
    }
    
    public void EndProcessRequest(IAsyncResult ar) { }

    #endregion

    #region IHttpHandler Members

    public bool IsReusable
    {
        get { return true; }
    }
    
    public void ProcessRequest(HttpContext context) { }
    
    #endregion
    
    // Main function of thread
    private void RequestWorker(Object obj)
    {
        AsyncRequestState state = obj as AsyncRequestState;

        string command = state.CurrentContext.Request.QueryString["cmd"];
        string guid = state.CurrentContext.Request.QueryString["guid"];

        switch (command)
        {
            case "register":
                updateClientProcessor.RegicterClient(state);
                state.CurrentContext.Response.Write(state.ClientGuid.ToString());
                state.CompleteRequest();
                break;
            case "unregister":
                updateClientProcessor.UnregisterClient(state);
                state.CompleteRequest();
                break;
            default:
                // when client reconects
                if (guid != null)
                    updateClientProcessor.UpdateClient(state, guid);
                break;
        }
    }
}