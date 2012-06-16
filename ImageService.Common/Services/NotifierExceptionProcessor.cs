using System;
using System.ServiceModel;
using ImageService;
using ImageService.Contracts;

namespace ImageService.Common
{
    public class NotifierExceptionProcessor : IFaultExceptionProcessor
    {
        public NotifierExceptionProcessor(IClientNotifier notifier)
        {
            Notifier = notifier;
        }

        public IClientNotifier Notifier { get; private set;  }

        public void ProcessException(Exception ex)
        {
            if (Notifier != null)
            {
                if (ex is FaultException<ServerFault>)
                    Notifier.Error((ex as FaultException<ServerFault>).Detail.FriendlyDiscription);
                if (ex is EndpointNotFoundException)
                    Notifier.Error("Endpoint not found!");
                if (ex is CommunicationObjectFaultedException || ex is System.ServiceModel.CommunicationException)
                    Notifier.Error("Service host is unavailable now");
            }
            else
                throw ex;
        }


        public void ProcessMessage(string message)
        {
            if (Notifier != null)
            {
                Notifier.Message(message);
            }
            else
                Console.WriteLine("Message form manager {0}: ",message);
        }
    }
}
