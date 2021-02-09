using System;

namespace Purple.Sdk.MimeTyper.Exceptions
{
    public class MimeTyperFetchException : MimeTyperException
    {
        public MimeTyperFetchException(string details) : base(details, "fetch_mime_types_exception")
        {
        }

        public MimeTyperFetchException(string details, Exception e) : base(details, "fetch_mime_types_exception", e)
        {
        }
    }
}