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
    //δ��Ȩʱ
    //x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})// Ҳ����ֱ��д�ַ�����AddAuthentication("Bearer")
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,//�����������±�
        ValidateIssuer = true,
        ValidIssuer = audienceConfig["Issuer"],//������
        ValidateAudience = true,
        ValidAudience = audienceConfig["Audience"],//������
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,//����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
        RequireExpirationTime = true,
    };
});

//��ɫ����
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());//������ɫ
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
    options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));//��Ĺ�ϵ
    options.AddPolicy("SystemAndAdmin", policy => policy.RequireRole("Admin").RequireRole("System"));//�ҵĹ�ϵ
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
