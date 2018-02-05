namespace Abstractions.Services.Contracts
{
    public interface IShareService
    {
        void Share(string text, string title, string url);
    }
}