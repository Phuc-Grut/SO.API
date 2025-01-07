using VFi.NetDevPack.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace VFi.Gateway.SO.Handlers
{
    public class AuthExSecurityHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public AuthExSecurityHandler(IConfiguration config, ITokenService tokenService, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _config = config;
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

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
            validUser.Product = "PIM";
            validUser.Exp = token.Payload.Exp;
            var generatedToken = _tokenService.BuildToken(_config["ExSecurity:Key"].ToString(), _config["ExSecurity :Issuer"].ToString(), validUser);
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
