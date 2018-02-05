namespace Abstractions.Services.Contracts
{
    public interface IResourceLoaderService
    {
        string GetString(string resource, string defaultValue = "");
    }
}