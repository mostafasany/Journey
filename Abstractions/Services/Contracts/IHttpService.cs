using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IHttpService
    {
        string AccessToken { get; set; }

        Task<HttpResult<T>> HttpGetAsync<T>(string url, Dictionary<string, string> headers = null) where T : class;

        Task<HttpResult<T>> HttpPostAsync<T>(string url, HttpContent content, Dictionary<string, string> headers = null)
            where T : class;

        Task<HttpResult<T>> HttpPostAsync<T>(string url, object content, Dictionary<string, string> headers = null)
            where T : class;

        Task<HttpResult<T>> HttpPutAsync<T>(string url, object content, Dictionary<string, string> headers = null)
            where T : class;

        Task<HttpResult<T>> HttpDeleteAsync<T>(string url, Dictionary<string, string> headers = null) where T : class;

        event UnAuthroirzedChangedEventHandler UnAuthorizedChanged;
    }

    public class HttpResult<T>
    {
        public HttpResult(T result, ErrorPayLoad errorResponse, HttpResponseMessage httpResponseMessage)
        {
            Result = result;
            HttpResponseMessage = httpResponseMessage;
            ErrorResponse = errorResponse;
        }

        public T Result { get; }
        public HttpResponseMessage HttpResponseMessage { get; }
        public ErrorPayLoad ErrorResponse { get; }
    }

    public class ErrorPayLoad
    {
        public List<Error> Errors { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Error
    {
        public string UserMessage { get; set; }
        public string InternalMessage { get; set; }
        public string MoreInfo { get; set; }
        public ErrorType Code { get; set; }
        public string ErrorType { get; set; }
    }

    public enum ErrorType
    {
        Validation,
        Authentication,
        Buisness
    }

    public class ErrorResponse
    {
        public List<string> ErrorMessages { get; set; }
        public string ErrorMessagesString { get; set; }
    }


    public delegate void UnAuthroirzedChangedEventHandler(object sender, UnAuthroirzedChangedEventArgs e);

    public class UnAuthroirzedChangedEventArgs : EventArgs
    {
        public string URL { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }


    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string message, object serviceParamters, string serviceURL = null,
            Exception innerException = null) : base(message, innerException)
        {
        }
    }
}