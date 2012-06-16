namespace ImageService.Common
{
    public interface IClientNotifier
    {
        void Warning(string message);
        void Message(string message);
        void Error  (string message);
    }
}
