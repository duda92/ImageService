using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageService.Common;
using System.Web;

namespace WebNotifying
{
    internal static class WebNotifyingExtention
    {
        public static string GetLastWarning(this IClientNotifier notifier)
        {
            return ((WebNotifier)notifier).LastWarning;
        }

        public static string GetLastMessage(this IClientNotifier notifier)
        {
            return ((WebNotifier)notifier).LastMessage;
        }

        public static string GetLastError(this IClientNotifier notifier)
        {
            return ((WebNotifier)notifier).LastError;
        }
    }

    public class WebNotifier : IClientNotifier
    {
        public string LastWarning { get; set; }
        public string LastMessage { get; set; }
        public string LastError { get; set; }

        
        #region IClientNotifier Members

        public void Warning(string message)
        {
            LastWarning = message;
        }

        public void Message(string message)
        {
            LastMessage = message;
        }

        public void Error(string message)
        {
            LastError = message;
        }

        #endregion
    }
}
