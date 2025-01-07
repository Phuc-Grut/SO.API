using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using VFi.NetDevPack.Configuration;

namespace VFi.Infra.SO.Context;

public class BidApiContext
{
    private readonly IFlurlClient _flurlClient;
    private readonly IConfiguration _appConfig;

    public BidApiContext(IConfiguration configuration, IFlurlClientFactory flurlClientFac)
    {
        _appConfig = configuration;
        _flurlClient = flurlClientFac.Get(BaseUrl);
        _flurlClient.WithOAuthBearerToken(Token);
    }
    private EndpointApiConfig Endpoint
    {
        get
        {
            return _appConfig.GetSection("EndPointApi:Bid").Get<EndpointApiConfig>();
        }
    }
    public IFlurlClient Client
    {
        get
        {
            _flurlClient.WithOAuthBearerToken(Token);
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
