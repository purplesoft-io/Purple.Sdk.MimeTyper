using System;

namespace Purple.Sdk.MimeTyper.Exceptions
{
    public class MimeTyperException : Exception
    {
        public string Code { get; }
        public string Details { get; }
        public MimeTyperException(string details, string code) : base(details)
        {
            Code = code;
            Details = details;
        }
        public MimeTyperException(string details, string code, Exception e) : base(details, e)
        {
            Code = code;
            Details = details;
        }
    }
}