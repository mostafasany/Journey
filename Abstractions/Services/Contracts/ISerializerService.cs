namespace Abstractions.Services.Contracts
{
    public interface ISerializerService
    {
        T DeserializeFromString<T>(string data);
        string SerializeToString<T>(T value);
    }
}