namespace Abstractions.Services.Contracts
{
    public interface IDeepLinkingService
    {
        void ParseDeepLinkingAndExecute(string deepLink);
    }
}