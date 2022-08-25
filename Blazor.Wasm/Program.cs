using Blazor.Common.ClientExtensions;
using Blazor.UI; 
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Main>("app"); 
builder.Services.AddAntDesignSetup();

builder.Services.AddBlazorHttpClient();
builder.Services.AddScoped<TaskDetailServices>();
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

var app = builder.Build(); 
await app.RunAsync();