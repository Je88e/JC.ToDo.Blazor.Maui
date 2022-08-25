using Blazor.Common.ServerExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var loggerFactory
    = LoggerFactory.Create(builder =>
    { builder.AddLog4Net("Config/log4net.Config"); });

builder.Services.AddEntityFramework(builder.Configuration);

#region JWT
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var audienceConfig = builder.Configuration.GetSection("Audience");
var symmetricKeyAsBase64 = audienceConfig["Secret"];
var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
var signingKey = new SymmetricSecurityKey(keyByteArray);
var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //未授权时
    //x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})// 也可以直接写字符串，AddAuthentication("Bearer")
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,//参数配置在下边
        ValidateIssuer = true,
        ValidIssuer = audienceConfig["Issuer"],//发行人
        ValidateAudience = true,
        ValidAudience = audienceConfig["Audience"],//订阅人
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
        RequireExpirationTime = true,
    };
});

//角色策略
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//单独角色
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
    options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//或的关系
    options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//且的关系
});
 
# endregion

//Add Cors
builder.Services.AddCorsSetup(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

//app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseCors(builder.Configuration.GetSection("Cors")["PolicyName"]);

app.UseEndpoints(endpoints =>
{ 
    endpoints.MapControllers();
});

app.Run();
