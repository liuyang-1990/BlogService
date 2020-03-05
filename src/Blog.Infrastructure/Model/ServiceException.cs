using System;
using System.Net;

namespace Blog.Infrastructure.Model
{
    public class ServiceException : Exception
    {
        public ServiceException(string msg) : base(msg)
        {

        }

        public ServiceException(string msg, string code) : base(msg)
        {
            this.ResponseCode = code;
        }

        public ServiceException(string msg, string code, HttpStatusCode statusCode) : base(msg)
        {
            this.ResponseCode = code;
            this.HttpStatusCode = statusCode;
        }
        public string ResponseCode { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

    }
}
