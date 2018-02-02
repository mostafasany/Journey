using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Exceptions;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Services.Core;

namespace Services
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
                //var responseJson =
                //   "{\"Title\":\"Fast & Furious 6\",\"Year\":\"2013\",\"Rated\":\"PG-13\",\"Released\":\"24 May 2013\",\"Runtime\":\"130 min\",\"Genre\":\"Action, Crime, Thriller\",\"Director\":\"Justin Lin\",\"Writer\":\"Chris Morgan, Gary Scott Thompson (characters)\",\"Actors\":\"Vin Diesel, Paul Walker, Dwayne Johnson, Jordana Brewster\",\"Plot\":\"Since Dom (Diesel) and Brian's (Walker) Rio heist toppled a kingpin's empire and left their crew with $100 million, our heroes have scattered across the globe. But their inability to return home and living forever on the lam have left their lives incomplete. Meanwhile, Hobbs (Johnson) has been tracking an organization of lethally skilled mercenary drivers across 12 countries, whose mastermind (Evans) is aided by a ruthless second-in-command revealed to be the love Dom thought was dead, Letty (Rodriguez). The only way to stop the criminal outfit is to outmatch them at street level, so Hobbs asks Dom to assemble his elite team in London. Payment? Full pardons for all of them so they can return home and make their families whole again.\",\"Language\":\"English, Russian, Spanish, Indonesian, Danish, Cantonese\",\"Country\":\"USA\",\"Awards\":\"9 wins & 21 nominations.\",\"Poster\":\"https://images-na.ssl-images-amazon.com/images/M/MV5BMTM3NTg2NDQzOF5BMl5BanBnXkFtZTcwNjc2NzQzOQ@@._V1_SX300.jpg\",\"Ratings\":[{\"Source\":\"Internet Movie Database\",\"Value\":\"7.1/10\"},{\"Source\":\"Rotten Tomatoes\",\"Value\":\"69%\"},{\"Source\":\"Metacritic\",\"Value\":\"61/100\"}],\"Metascore\":\"61\",\"imdbRating\":\"7.1\",\"imdbVotes\":\"328,573\",\"imdbID\":\"tt1905041\",\"Type\":\"movie\",\"tomatoMeter\":\"N/A\",\"tomatoImage\":\"N/A\",\"tomatoRating\":\"N/A\",\"tomatoReviews\":\"N/A\",\"tomatoFresh\":\"N/A\",\"tomatoRotten\":\"N/A\",\"tomatoConsensus\":\"N/A\",\"tomatoUserMeter\":\"N/A\",\"tomatoUserRating\":\"N/A\",\"tomatoUserReviews\":\"N/A\",\"tomatoURL\":\"http://www.rottentomatoes.com/m/fast_and_furious_6/\",\"DVD\":\"10 Dec 2013\",\"BoxOffice\":\"$238,700,000\",\"Production\":\"Universal Pictures\",\"Website\":\"http://www.thefastandthefurious.com/\",\"Response\":\"True\"}";
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