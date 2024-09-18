namespace Shortner.Application.Exceptions;

public class UrlExpiredException(string key) : Exception($"Url with key {key} expired.");