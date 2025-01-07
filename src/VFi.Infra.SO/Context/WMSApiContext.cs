using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Context;

public class WMSApiContext
{
    private readonly IFlurlClient _flurlClient;
    private readonly IConfiguration appConfig;

    public WMSApiContext(IConfiguration configuration, IFlurlClientFactory flurlClientFac)
    {
        appConfig = configuration;
        _flurlClient = flurlClientFac.Get(this.BaseUrl);
        _flurlClient.WithOAuthBearerToken(this.Token);
    }
    public EndpointApiConfig Endpoint
    {
        get
        {
            return appConfig.GetSection("EndPointApi:WMS").Get<EndpointApiConfig>();
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
