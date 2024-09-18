using Microsoft.Extensions.Options;
using URLShortner.Web.Config;
using URLShortner.Web.ViewModels;

namespace URLShortner.Web.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(IOptionsMonitor<GatewayConfig> optionsMonitor,
        HttpClient httpClient)
    {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(optionsMonitor.CurrentValue.Url);
        }

    public async Task<GetRedirectUrlViewModel> GetRedirectUrlAsync(string uniqueKey)
    {
           var response =  await _httpClient.GetAsync($"shortner/{uniqueKey}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<GetRedirectUrlViewModel>();

        }
}