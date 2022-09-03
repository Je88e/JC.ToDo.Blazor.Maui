using Blazor.UI;
using Blazor.UI.UIService;
using Client.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting; 

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Main>("app");
builder.Services.AddAntDesignSetup();

builder.Services.AddBlazorHttpClient();
builder.Services.AddScoped<TaskDetailServices>();


#region jwt
builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();
builder.Services.AddAuthorizationCore(option =>
{
	option.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
}); 
#endregion


#region identity
//builder.Services.AddOidcAuthentication(options =>
//{
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//}); 
#endregion

var app = builder.Build();
await app.RunAsync();