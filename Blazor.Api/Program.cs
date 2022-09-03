using Blazor.Common.Helper;
using Blazor.Common.LogHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Server.Extensions.Config;
using Server.Extensions.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string ApiName = "Blazor.Core.Api";

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureLogging((hostingContext, builder) =>
{
    builder.AddFilter("System", LogLevel.Error);
    builder.AddFilter("Microsoft", LogLevel.Error);
    builder.SetMinimumLevel(LogLevel.Error);
    builder.AddLog4Net(Path.Combine(Directory.GetCurrentDirectory(), "Config/log4net.config"));
})
.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.Sources.Clear();
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
});

#region JWT
builder.Services.AddSingleton(new SecretConfig(builder.Configuration));
builder.Services.AddSingleton<ISecretConfig>(new SecretConfig(builder.Configuration));
builder.Services.AddSingleton(new JwtTokenDefaultConfig(builder.Configuration));
builder.Services.AddAuthentication_JWTSetup();
builder.Services.AddAuthorizationSetup(); 
#endregion

//Add Cors
builder.Services.AddCorsSetup(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});

builder.Services.AddEntityFramework(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"{ApiName}");
        c.RoutePrefix = "";
    });
}

app.UseRouting();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//app.UseCors("CorsPolicy");
//app.UseCors(builder.Configuration.GetSection("Cors")["PolicyName"]);

//app.UseAuthentication();

//app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
