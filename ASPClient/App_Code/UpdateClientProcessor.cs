using System;
using System.Collections.Generic;
using ImageService.Common;
using System.Timers;
using System.Web.Script.Serialization;
using WebNotifying;
using SystemTimer = System.Timers.Timer;

public class UpdateClientProcessor
{
   private SystemTimer timer;
   private Object _lock = new Object();
   private List<AsyncRequestState> _clientStateList = new List<AsyncRequestState>();
   private JavaScriptSerializer serializer = new JavaScriptSerializer();

   public UpdateClientProcessor()
   {
       timer = new SystemTimer(3000);
       timer.Elapsed += CheckStatusForRegistered;
       timer.Start();
   }

    public void UpdateClient(AsyncRequestState state, String guid)
    {
        lock (_lock)
        {
            AsyncRequestState clientState = _clientStateList.Find(s => s.ClientGuid == guid);
            if (clientState != null)
            {
                clientState.CurrentContext = state.CurrentContext;
                clientState.ExtraData = state.ExtraData;
                clientState.AsyncCallback = state.AsyncCallback;
            }
        }
    }
    
    public void RegicterClient(AsyncRequestState state)
    {
        lock (_lock)
        {
            state.ClientGuid = Guid.NewGuid().ToString("N");
            _clientStateList.Add(state);
        }
    }

    public void UnregisterClient(AsyncRequestState state)
    {
        lock (_lock)
        {
            _clientStateList.Remove(state);
        }
    }

    private void CheckStatusForRegistered(Object sender, ElapsedEventArgs e)
    {
        lock (_lock)
        {
            foreach (AsyncRequestState clientState in _clientStateList)
            {
                if (clientState.CurrentContext.Session != null)
                {
                    ImageServiceClientManager manager = (ImageServiceClientManager)clientState.CurrentContext.Session["Manager"];
                    bool? status = manager.Status();
                    if (status.HasValue && status.Value == false)
                    {
                        clientState.CurrentContext.Response.Write(serializer.Serialize(new { isError = false, needToUpdate = true }));
                        clientState.CompleteRequest();
                    }
                    else if (!status.HasValue)
                    {
                        string errorMessage_ = manager.Notifier.GetLastError();
                        clientState.CurrentContext.Response.Write(serializer.Serialize(new { isError = true, errorMessage = errorMessage_ }));
                        clientState.CompleteRequest();
                        _clientStateList.Clear();
                        break;
                    }
                }
            }
        }
    }
}
