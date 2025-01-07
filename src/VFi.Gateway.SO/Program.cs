
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Provider.Polly;
using MMLib.Ocelot.Provider.AppConfiguration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using VFi.Gateway.SO.Handlers;
using VFi.NetDevPack.Context;
using System.Security.Claims;
using VFi.NetDevPack.DynamicJwtBearer.DynamicAuthority;
using Ocelot.Values;
using VFi.NetDevPack.DynamicJwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;
using Polly;
using static IdentityModel.OidcConstants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Logging;

var ocelotJson = new JObject();
var builder = WebApplication.CreateBuilder(args);
IdentityModelEventSource.ShowPII = true;
//if (!builder.Environment.IsDevelopment())
//{
    foreach (var jsonFilename in Directory.EnumerateFiles("Configuration", "ocelot.*.Production.json", SearchOption.AllDirectories))
    {
        using (StreamReader fi = File.OpenText(jsonFilename))
        {
            var json = JObject.Parse(fi.ReadToEnd());
            ocelotJson.Merge(json, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
        }
    }
//}
//else
//{
//    foreach (var jsonFilename in Directory.EnumerateFiles("Configuration", "ocelot.*.Development.json", SearchOption.AllDirectories))
//    {
//        using (StreamReader fi = File.OpenText(jsonFilename))
//        {
//            var json = JObject.Parse(fi.ReadToEnd());
//            ocelotJson.Merge(json, new JsonMergeSettings
//            {
//                MergeArrayHandling = MergeArrayHandling.Union
//            });
//        }
//    }
//}



File.WriteAllText("ocelot.json", ocelotJson.ToString());
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json",
                    optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                    optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddAspNetUserConfiguration();
builder.Services
                .AddSwaggerForOcelot(builder.Configuration)
                .AddOcelot(builder.Configuration)
                .AddAppConfiguration().AddPolly()
                .AddConsul()
                .AddConfigStoredInConsul()
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.AuthInternalHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.CrawlerHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.TokenHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.AuthExSecurityHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.PIMTokenHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.DAMTokenHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.SpiderTokenHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.MasterTokenHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.SO.Handlers.SOTokenHandler>(false);
builder.Services.AddHealthChecks();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Gateway", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });

});

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
    options.AddPolicy("CorsPolicy",
    builder => builder
          //.AllowAnyOrigin()
          .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins(allowedOrigins)
        .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.AddMemoryCache();


builder.Services.AddAuthentication()
                           .AddDynamicJwtBearer("OidcSecurity", options =>
{
    var aud = builder.Configuration["OidcSecurity:Aud"];
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ValidateAudience = !string.IsNullOrEmpty(aud);
    if (!string.IsNullOrEmpty(aud))
    {
        options.Audience = aud;
        options.TokenValidationParameters.ValidAudiences = new string[] { aud };
    }
    options.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 403;
            c.Response.ContentType = "text/plain";
            return c.Response.WriteAsync(c.Exception.ToString());
        }
    };
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Validate();
})
                           .AddDynamicAuthorityJwtBearerResolver<ResolveAuthorityService>()
                           .AddJwtBearer("JwtExSecurity", options =>
                            {
                                options.TokenValidationParameters = new
                                                         TokenValidationParameters()
                                {
                                    IssuerSigningKey = new SymmetricSecurityKey
                                    (Encoding.UTF8.GetBytes(builder.Configuration["JwtExSecurity:Key"])),
                                    ValidAudience = builder.Configuration["JwtExSecurity:Audience"],
                                    ValidIssuer = builder.Configuration["JwtExSecurity:Issuer"],
                                    ValidateIssuerSigningKey = true,
                                    ValidateLifetime = false,
                                    ValidateAudience = !string.IsNullOrEmpty(builder.Configuration["JwtExSecurity:Audience"]),
                                    ClockSkew = TimeSpan.Zero
                                };

                            })
                                                       .AddJwtBearer("JwtExSecurity01", options =>
                            {
                                options.TokenValidationParameters = new
                                                         TokenValidationParameters()
                                {
                                    IssuerSigningKey = new SymmetricSecurityKey
                                    (Encoding.UTF8.GetBytes(builder.Configuration["JwtExSecurity01:Key"])),
                                    ValidAudience = builder.Configuration["JwtExSecurity01:Audience"],
                                    ValidIssuer = builder.Configuration["JwtExSecurity01:Issuer"],
                                    ValidateIssuerSigningKey = true,
                                    ValidateLifetime = false,
                                    ValidateAudience = !string.IsNullOrEmpty(builder.Configuration["JwtExSecurity01:Audience"]),
                                    ClockSkew = TimeSpan.Zero
                                };

                            })
                                                       .AddJwtBearer("JwtExSecurity02", options =>
                            {
                                options.TokenValidationParameters = new
                                                         TokenValidationParameters()
                                {
                                    IssuerSigningKey = new SymmetricSecurityKey
                                    (Encoding.UTF8.GetBytes(builder.Configuration["JwtExSecurity02:Key"])),
                                    ValidAudience = builder.Configuration["JwtExSecurity02:Audience"],
                                    ValidIssuer = builder.Configuration["JwtExSecurity02:Issuer"],
                                    ValidateIssuerSigningKey = true,
                                    ValidateLifetime = false,
                                    ValidateAudience = !string.IsNullOrEmpty(builder.Configuration["JwtExSecurity02:Audience"]),
                                    ClockSkew = TimeSpan.Zero
                                };

                            });

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");

app.UseWebSockets();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapHealthChecks("/health");
app.UseSwaggerForOcelotUI(opt =>
{
    opt.RoutePrefix = "docs";
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse =
             ctx =>
{
    string path = ctx.File.PhysicalPath;
    if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".svg"))
    {
        TimeSpan maxAge = new TimeSpan(7, 0, 0, 0);
        ctx.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
    }
    if (path.EndsWith(".json") || path.EndsWith(".txt"))
    {
        TimeSpan maxAge = new TimeSpan(0, 0, 10, 0);
        ctx.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
        var _corsPolicyProvider = app.Services.GetRequiredService<ICorsPolicyProvider>();
        var corsService = app.Services.GetRequiredService<ICorsService>();
        var policy = _corsPolicyProvider.GetPolicyAsync(ctx.Context, "CorsPolicy").ConfigureAwait(false).GetAwaiter().GetResult();
        CorsResult corsResult = corsService.EvaluatePolicy(ctx.Context, policy);
        corsService.ApplyResult(corsResult, ctx.Context.Response);
    }
}
});

app.UseOcelot().Wait();
app.Run();
public static class TokenLifetimeValidator
{
    public static bool Validate(
        DateTime? notBefore,
        DateTime? expires,
        SecurityToken tokenToValidate,
        TokenValidationParameters @param
    )
    {
        var temp = expires > DateTime.UtcNow;
        return (expires != null && expires > DateTime.UtcNow);
    }
}

internal class ResolveAuthorityService : IDynamicJwtBearerAuthorityResolver
{
    private readonly IConfiguration configuration;

    public ResolveAuthorityService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public TimeSpan ExpirationOfConfiguration => TimeSpan.FromHours(1);

    public Task<string> ResolveAuthority(HttpContext httpContext)
    {
        var audienceConfig = configuration.GetSection("OidcSecurity");
        string authorization = httpContext.Request.Headers[HeaderNames.Authorization];
        var token = "";
        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorization.Substring("Bearer ".Length).Trim();
        }
        var jwt_token = new JwtSecurityToken(token);
        var realm = jwt_token.Claims.First(c => c.Type == "tenant").Value;
        var authority = $"{audienceConfig["Iss"]}/realms/{realm}";
        return Task.FromResult(authority);
    }
}