using Blazor.UI.AntDesion.Extensions;
using Blazor.UI.Data;
/* 项目“Blazor.Maui (net6.0-ios)”的未合并的更改
在此之前:
using Blazor.UI.AntDesion.Extensions;
在此之后:
using Microsoft.AspNetCore.Components.WebView.Maui;
*/

/* 项目“Blazor.Maui (net6.0-maccatalyst)”的未合并的更改
在此之前:
using Blazor.UI.AntDesion.Extensions;
在此之后:
using Microsoft.AspNetCore.Components.WebView.Maui;
*/

/* 项目“Blazor.Maui (net6.0-windows10.0.19041.0)”的未合并的更改
在此之前:
using Blazor.UI.AntDesion.Extensions;
在此之后:
using Microsoft.AspNetCore.Components.WebView.Maui;
*/


namespace Blazor.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddAntDesignSetup();
        return builder.Build();
    }
}
