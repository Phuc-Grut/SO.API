using VFi.NetDevPack.Context;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace VFi.Gateway.SO.Handlers
{
    public class CrawlerHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IDictionary<string, string> defaultHeaders = new Dictionary<string, string>()
        {
            {"User-Agent", "Mozilla/5.0 {Windows NT 10.0; Win64; x64} AppleWebKit/537.36 {KHTML, like Gecko} Chrome/77.0.3865.90 Safari/537.36"},
            {"Accept-Charset", "ISO-8859-1"},
            {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"},
            {"accept-encoding", "gzip, deflate, br"},
        };

        public CrawlerHandler(IConfiguration config, ITokenService tokenService, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _config = config;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var downstreamRoute = _accessor.HttpContext.Items?.DownstreamRouteHolder()?.Route?.DownstreamRoute?.FirstOrDefault();
            if (downstreamRoute != null)
            {
                if (downstreamRoute.Key == "BarKey")
                {
                    //Do something else
                }
            }
           
            base.InnerHandler = new HttpClientHandler()
            {

            };
            request.Headers.AcceptEncoding.Clear();
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("br")); 
            var respon = await base.SendAsync(request, cancellationToken);
           
            var xResponseContent =  respon.Content.ReadAsStringAsync().Result;

            //Stream receiveStream = await  respon.Content.ReadAsStreamAsync();
            //StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
            //var t  = readStream.ReadToEnd();

            JObject postsByUserName = new JObject();
            postsByUserName.Add(new JProperty("key", "ok"));
            var postsByUsernameString = JsonConvert.SerializeObject(postsByUserName);
            var stringContent = new StringContent(postsByUsernameString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };
            respon.Content = stringContent;
            return respon;
        }

    } 
}
