using System.Net.Http.Headers;

namespace VFi.Gateway.SO.Handlers
{
	public class SOTokenHandler : DelegatingHandler
	{
		private readonly IConfiguration _config;

		public SOTokenHandler(IConfiguration config)
		{
			_config = config;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{

			var tokenPim = _config["SO:Token"]?.ToString();
			request.Headers.Remove("Authorization");
			request.Headers.Add("Authorization", "Bearer " + tokenPim);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenPim);
			request.Headers.Remove("AccessToken");
			return await base.SendAsync(request, cancellationToken);
		}

	}
}
