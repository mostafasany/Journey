namespace Abstractions.Services
{
    public interface IDeepLinkService
    {
        void ParseDeepLinkingAndExecute(string deepLink);
    }
}