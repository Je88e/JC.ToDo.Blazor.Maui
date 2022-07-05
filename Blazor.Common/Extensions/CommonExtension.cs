using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Common.Extensions;

public static class CommonExtension
{
    public static void AddBlazorBaseServerSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"http://localhost:5500") });
    }
}