using VFi.NetDevPack.Context;
using Microsoft.AspNetCore.Authentication;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace VFi.Gateway.SO.Handlers
{
    public class AuthInternalHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public AuthInternalHandler(IConfiguration config, ITokenService tokenService, IHttpContextAccessor accessor)
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
                //    var func = downstreamRoute.SecurityOptions.Function;
                //    var actions = downstreamRoute.SecurityOptions.ActionList;
                if (downstreamRoute.Key == "something")
                {
                    //Do something else
                }
            }
           
            var auth = request.Headers.Authorization; 
            var jwt = auth.Parameter;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var sub = token.Payload.Sub; 

            var tokeclaims = token.Payload.Claims;
            var validUser = new UserClaims();
            validUser.Username = this.GetClaimValue(tokeclaims, "preferred_username");
            validUser.FullName = this.GetClaimValue(tokeclaims, "name");
            validUser.Tenant = this.GetClaimValue(tokeclaims, "tenant");
            validUser.TenantName = this.GetClaimValue(tokeclaims, "tenant_name");
            validUser.Data_Zone = this.GetClaimValue(tokeclaims, "data_zone");
            validUser.Data = this.GetClaimValue(tokeclaims, "data");
            validUser.Email = this.GetClaimValue(tokeclaims, "email");
            validUser.Email_Verified = bool.Parse(this.GetClaimValue(tokeclaims, "email_verified"));
            validUser.Sub = sub;
            validUser.Product = _config["JwtInternal:Product"].ToString();
            validUser.Exp = token.Payload.Exp;
            var generatedToken = _tokenService.BuildToken(_config["JwtInternal:Key"].ToString(), _config["JwtInternal:Issuer"].ToString(),  validUser);
            Console.WriteLine(generatedToken);
            //  request.Headers.Add("Authorization", "Bearer " + generatedToken); 
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", generatedToken);
            request.Headers.Remove("AccessToken");
          return await base.SendAsync(request, cancellationToken);
        }
        private string GetClaimValue(IEnumerable<Claim> tokeclaims, string key)
        {
            try
            {
                return tokeclaims.Where(x => x.Type == key).Select(x => x.Value).First();
            }
            catch (Exception)
            {

                return "";
            }
        }
    }
}
