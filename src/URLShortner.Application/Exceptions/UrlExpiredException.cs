using System;

namespace URLShortner.Application.Exceptions
{
    public class UrlExpiredException : Exception
    {
        public UrlExpiredException(string key) : base($"Url with key {key} expired.")
        {

        }
    }
}
