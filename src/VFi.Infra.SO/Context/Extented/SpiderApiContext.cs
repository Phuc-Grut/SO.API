using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Exceptions;

namespace VFi.Infra.SO.Context;

public class SpiderApiContext
{
    private readonly IConfiguration appConfig;

    public SpiderApiContext(IConfiguration configuration, IFlurlClientFactory flurlClientFac)
    {
        appConfig = configuration;
        Client = flurlClientFac.Get(BaseUrl);
        Client.WithOAuthBearerToken(Token);
    }
    private EndpointApiConfig Endpoint
    {
        get
        {
            return appConfig.GetSection("EndPointApi:Partner").Get<EndpointApiConfig>();
        }
    }
    public IFlurlClient Client { get; }
    public string BaseUrl
    {
        get
        {
            return Endpoint.BaseUrl;
        }

    }

    public string Token
    {
        get
        {
            return Endpoint.AccessToken;
        }
    }
}
