namespace Shortner.Application.CustomUrl
{
    public class GetRedirectUrlViewModel
    {
        public GetRedirectUrlViewModel(string originalUrl)
        {
            OriginalUrl = originalUrl;
        }

        public string OriginalUrl { get; }
    }
}
