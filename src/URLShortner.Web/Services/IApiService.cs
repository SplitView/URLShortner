using URLShortner.Web.ViewModels;

namespace URLShortner.Web.Services
{
    public interface IApiService
    {
        Task<GetRedirectUrlViewModel> GetRedirectUrlAsync(string uniqueKey);
    }
}
