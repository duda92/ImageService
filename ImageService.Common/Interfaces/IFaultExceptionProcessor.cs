using System;

namespace ImageService.Common
{
    public interface IFaultExceptionProcessor
    {
        void ProcessException(Exception ex);

        void ProcessMessage(string message);
    }
}
