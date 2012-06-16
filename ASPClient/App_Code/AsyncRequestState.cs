using System;
using System.Threading;
using System.Web;

public class AsyncRequestState : IAsyncResult
{
    public HttpContext CurrentContext;
    public AsyncCallback AsyncCallback;
    public object ExtraData;
    public string ClientGuid;
    private Boolean _isCompleted;

    public AsyncRequestState(HttpContext context, AsyncCallback callback, object data)
    {
        CurrentContext = context;
        AsyncCallback = callback;
        ExtraData = data;
        _isCompleted = false;
    }

    public void CompleteRequest()
    {
        _isCompleted = true;
        if (AsyncCallback != null)
        {
            AsyncCallback(this);
        }
    }

    #region IAsyncResult Members
    
    public Boolean CompletedSynchronously
    {
        get { return false; }
    }

    public bool IsCompleted
    {
        get { return _isCompleted; }
    }

    public object AsyncState
    {
        get { return ExtraData; }
    }

    public WaitHandle AsyncWaitHandle
    {
        get { return new ManualResetEvent(false); }
    }
    #endregion

}
