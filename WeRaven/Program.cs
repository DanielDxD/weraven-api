using WeRaven.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using WeRaven.Data;
using WeRaven.Services;

EnvTool.Configure();

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(EnvTool.JwtKey);
builder.Services.AddSignalR();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: EnvTool.CorsName, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "http://192.168.2.6:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddMemoryCache();
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Optimal;
});
builder
    .Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; })
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "We Raven API - V1",
        Version = "v1",
        Description = "The private WeRaven Api",
        Contact = new OpenApiContact
        {
            Name = "E-mail",
            Email = "backend-team@weraven.net"
        }
    });
});

builder.Services.AddTransient<EmailService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddSingleton<MongoService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(EnvTool.DbConnection);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "Redis App";
    options.Configuration = "localhost:6379";
});
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.UseResponseCompression();

app.Run();
