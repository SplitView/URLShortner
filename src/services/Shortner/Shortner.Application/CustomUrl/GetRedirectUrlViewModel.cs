namespace Shortner.Application.CustomUrl;

public class GetRedirectUrlViewModel(string originalUrl)
{
    public string OriginalUrl { get; } = originalUrl;
}