using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Common.Extensions;

public static class HttpClientSetup
{
    public static void AddBlazorHttpClient(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"http://localhost:5022") });
    }
}