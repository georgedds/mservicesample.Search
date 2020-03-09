using System;
using System.Net;
using mservicesample.Search.Api.Helpers;

namespace mservicesample.Search.Api.Middleware
{
    public class AppException : Exception
    {
        public AppException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public AppException( object errors = null)
        {
            Errors = errors;
        }

        public object Errors { get; set; }

        public HttpStatusCode Code { get; }

        public  int ErrorCode { get;}
    }

    public abstract class TransactionException : Exception
    {
        public TransactionException(string message)
            : base(message)
        {  }

        public abstract int ErrorCode { get; }
    }

    public class AppTestException : AppException
    {
        public AppTestException()
            : base($"This is a app test ex.")
        { }

        public  int ErrorCode => AppErrorCodes.TestError;
    }
}
