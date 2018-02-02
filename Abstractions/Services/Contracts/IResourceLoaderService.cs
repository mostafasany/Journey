namespace Services.Core
{
    public interface IResourceLoaderService
    {
        string GetString(string resource, string defaultValue = "");
    }
}