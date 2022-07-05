using Blazor.Common.Extensions;
using Blazor.UI;
using Blazor.UI.AntDesion.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Main>("app"); 
builder.Services.AddAntDesignSetup();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5022") });
builder.Services.AddScoped<TaskDetailServices>();

//builder.Services.AddBlazorBaseServerSetup();
var app = builder.Build(); 
await app.RunAsync();