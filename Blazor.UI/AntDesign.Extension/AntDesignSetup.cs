using Microsoft.Extensions.DependencyInjection;

namespace Blazor.UI.AntDesion.Extensions;

public static class AntDesignSetup
{
    public static void AddAntDesignSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddAntDesign();
    }
}