using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Newtonsoft.Json;
using Unity;

namespace Abstractions.Services
{
    public class HttpService : BaseService, IHttpService
    {
        private readonly HttpClient client;

        public HttpService(IUnityContainer container, IExceptionService exceptionService) : base(container)
        {
            client = new HttpClient();
            ExceptionService = exceptionService;
            //client.Timeout = TimeSpan.FromSeconds(60);
        }

        public string AccessToken { get; set; }
        public event UnAuthroirzedChangedEventHandler UnAuthorizedChanged;

        public async Task<HttpResult<T>> HttpDeleteAsync<T>(string url, Dictionary<string, string> headers = null)
            where T : class
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Delete
                };

                if (headers != null)
                    foreach (var header in headers)
                        request.Headers.Add(header.Key, header.Value);

                var result = await client.SendAsync(request);
                var responseJson = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var itemType = typeof(T);
                    if (itemType == typeof(string))
                    {
                        var responseObject = (T) Convert.ChangeType(responseJson, typeof(T));
                        return new HttpResult<T>(responseObject, null, result);
                    }
                    else
                    {
                        var responseObject = JsonConvert.DeserializeObject<T>(responseJson);
                        return new HttpResult<T>(responseObject, null, result);
                    }
                }
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    UnAuthorizedChanged?.Invoke(headers,
                        new UnAuthroirzedChangedEventArgs {URL = url, Headers = headers});
                    throw new UnAuthorizedException(ExceptionType.General.ToString(), headers, url);
                }
                var errorResponseObject = JsonConvert.DeserializeObject<ErrorPayLoad>(responseJson);
                if (errorResponseObject == null)
                {
                    var error = new List<string> {responseJson};
                }
                return new HttpResult<T>(null, errorResponseObject, result);
            }
            catch (UnAuthorizedException ex)
            {
                throw new RequestFailedException(ExceptionType.UnAuthorized.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new RequestFailedException(ExceptionType.General.ToString(), ex);
            }
        }

        public async Task<HttpResult<T>> HttpGetAsync<T>(string url, Dictionary<string, string> headers) where T : class
        {
            HttpResponseMessage result = null;
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };

                if (!string.IsNullOrEmpty(AccessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue(AccessToken);
                if (headers != null)
                    foreach (var header in headers)
                        request.Headers.Add(header.Key, header.Value);


                result = await client.SendAsync(request);
                var responseJson = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (result.IsSuccessStatusCode)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Include
                    };
                    var responseObject = JsonConvert.DeserializeObject<T>(responseJson, settings);
                    return new HttpResult<T>(responseObject, null, result);
                }
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    UnAuthorizedChanged?.Invoke(headers,
                        new UnAuthroirzedChangedEventArgs {URL = url, Headers = headers});
                    throw new UnAuthorizedException("UnAuthorized", headers, url);
                }
                var errorRsponseObject = JsonConvert.DeserializeObject<ErrorPayLoad>(responseJson);
                return new HttpResult<T>(null, errorRsponseObject, result);
            }
            catch (UnAuthorizedException ex)
            {
                throw new RequestFailedException(ExceptionType.UnAuthorized.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new RequestFailedException(ExceptionType.General.ToString(), ex);
            }
        }

        public async Task<HttpResult<T>> HttpPostAsync<T>(string url, object content,
            Dictionary<string, string> headers = null) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(content);
                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = jsonContent
                };
                if (!string.IsNullOrEmpty(AccessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue(AccessToken);
                if (headers != null)
                    foreach (var header in headers)
                        request.Headers.Add(header.Key, header.Value);

                var result = await client.SendAsync(request);
                var responseJson = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var itemType = typeof(T);
                    if (itemType == typeof(string))
                    {
                        var responseObject = (T) Convert.ChangeType(responseJson, typeof(T));
                        return new HttpResult<T>(responseObject, null, result);
                    }
                    else
                    {
                        var responseObject = JsonConvert.DeserializeObject<T>(responseJson);
                        return new HttpResult<T>(responseObject, null, result);
                    }
                }
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    UnAuthorizedChanged?.Invoke(headers,
                        new UnAuthroirzedChangedEventArgs {URL = url, Headers = headers});
                    throw new UnAuthorizedException(ExceptionType.General.ToString(), headers, url);
                }
                var errorResponseObject = JsonConvert.DeserializeObject<ErrorPayLoad>(responseJson);
                return new HttpResult<T>(null, errorResponseObject, result);
            }
            catch (UnAuthorizedException ex)
            {
                throw new RequestFailedException(ExceptionType.UnAuthorized.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new RequestFailedException(ExceptionType.General.ToString(), ex);
            }
        }

        public async Task<HttpResult<T>> HttpPostAsync<T>(string url, HttpContent content,
            Dictionary<string, string> headers = null) where T : class
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = content
                };
                if (!string.IsNullOrEmpty(AccessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue(AccessToken);

                if (headers != null)
                    foreach (var header in headers)
                        request.Headers.Add(header.Key, header.Value);

                var result = await client.SendAsync(request);
                var responseJson = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var itemType = typeof(T);
                    if (itemType == typeof(string))
                    {
                        var responseObject = (T) Convert.ChangeType(responseJson, typeof(T));
                        return new HttpResult<T>(responseObject, null, result);
                    }
                    else
                    {
                        var responseObject = JsonConvert.DeserializeObject<T>(responseJson);
                        return new HttpResult<T>(responseObject, null, result);
                    }
                }
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    UnAuthorizedChanged?.Invoke(headers,
                        new UnAuthroirzedChangedEventArgs {URL = url, Headers = headers});
                    throw new UnAuthorizedException(ExceptionType.UnAuthorized.ToString(), headers, url);
                }
                var errorResponseObject = JsonConvert.DeserializeObject<ErrorPayLoad>(responseJson);
                return new HttpResult<T>(null, errorResponseObject, result);
            }
            catch (UnAuthorizedException ex)
            {
                throw new RequestFailedException(ExceptionType.UnAuthorized.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new RequestFailedException(ExceptionType.General.ToString(), ex);
            }
        }

        public async Task<HttpResult<T>> HttpPutAsync<T>(string url, object content,
            Dictionary<string, string> headers = null) where T : class
        {
            try
            {
                var json = JsonConvert.SerializeObject(content);
                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Put,
                    Content = jsonContent
                };
                if (!string.IsNullOrEmpty(AccessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue(AccessToken);
                if (headers != null)
                    foreach (var header in headers)
                        request.Headers.Add(header.Key, header.Value);

                var result = await client.SendAsync(request);
                var responseJson = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var itemType = typeof(T);
                    if (itemType == typeof(string))
                    {
                        var responseObject = (T) Convert.ChangeType(responseJson, typeof(T));
                        return new HttpResult<T>(responseObject, null, result);
                    }
                    else
                    {
                        var responseObject = JsonConvert.DeserializeObject<T>(responseJson);
                        return new HttpResult<T>(responseObject, null, result);
                    }
                }
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    UnAuthorizedChanged?.Invoke(headers,
                        new UnAuthroirzedChangedEventArgs {URL = url, Headers = headers});
                    throw new UnAuthorizedException(ExceptionType.General.ToString(), headers, url);
                }
                var errorResponseObject = JsonConvert.DeserializeObject<ErrorPayLoad>(responseJson);

                return new HttpResult<T>(null, errorResponseObject, result);
            }
            catch (UnAuthorizedException ex)
            {
                throw new RequestFailedException(ExceptionType.UnAuthorized.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new RequestFailedException(ExceptionType.General.ToString(), ex);
            }
        }

        private enum ExceptionType
        {
            UnAuthorized,
            General
        }
    }
}