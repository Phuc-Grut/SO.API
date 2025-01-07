using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using VFi.NetDevPack.Configuration;

namespace VFi.Infra.SO.Context;

public class POApiContext
{
    private readonly IFlurlClient _flurlClient;
    private readonly IConfiguration appConfig;

    public POApiContext(IConfiguration configuration, IFlurlClientFactory flurlClientFac)
    {
        appConfig = configuration;
        _flurlClient = flurlClientFac.Get(this.BaseUrl);
        _flurlClient.WithOAuthBearerToken(this.Token);
    }
    private EndpointApiConfig Endpoint
    {
        get
        {
            return appConfig.GetSection("EndPointApi:PO").Get<EndpointApiConfig>();
        }
    }
    public IFlurlClient Client
    {
        get
        {
            _flurlClient.WithOAuthBearerToken(this.Token);
            return _flurlClient;
        }
    }
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
            if (!string.IsNullOrEmpty(Endpoint.AccessToken))
            {
                return Endpoint.AccessToken;
            }

            return _token;
        }
        set
        {
            _token = value;
        }
    }

    private string _token;
}
