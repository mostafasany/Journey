using System;
using System.IO;
using System.Xml.Serialization;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Unity;

namespace Abstractions.Services
{
    public class SerializerService : BaseService, ISerializerService
    {
        public SerializerService(IUnityContainer container, IExceptionService exceptionService) : base(container)
        {
            ExceptionService = exceptionService;
        }

        public T DeserializeFromString<T>(string data)
        {
            try
            {
                using (var stringReader = new StringReader(data))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T) serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }

        public string SerializeToString<T>(T value)
        {
            try
            {
                using (var stringWriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stringWriter, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new CoreServiceException(ex);
            }
        }
    }
}