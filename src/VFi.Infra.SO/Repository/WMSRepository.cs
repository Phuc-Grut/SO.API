
using Consul;
using FluentValidation.Results;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.Repository;

public class WMSRepository : IWMSRepository
{

    private readonly WMSApiContext _apiContext;
    private readonly string PATH_REQUEST_ADD_EXT = "/api/fulfillmentrequest/add-ext";
    private readonly IContextUser _context;
    private readonly ITokenService _tokenService;

    public WMSRepository(WMSApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context, ITokenService tokenService)
    {
        _apiContext = apiContext;
        _context = context;
        _tokenService = tokenService;

    }

    public async Task<ValidationResult> FulfillmentRequestAddExt(FulfillmentRequestAddExt t)
    {
        _apiContext.Token = _context.GetToken();
        var response = await _apiContext.Client.Request(PATH_REQUEST_ADD_EXT).PostJsonAsync(t);
        var responseContent = await response.ResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ValidationResult>(responseContent) ?? new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("Id", "error") } };

    }
    public async Task<ValidationResult> FulfillmentRequestAddExtEndpoint(FulfillmentRequestAddExt t)
    {
        var token = _tokenService.BuildTenantToken(_apiContext.Endpoint.JwtKey, _apiContext.Endpoint.JwtIssuer, _apiContext.Endpoint.JwtAudience, _context.Tenant, _context.Data, _context.Data_Zone);
        _apiContext.Token = token;
        var response = await _apiContext.Client.Request(PATH_REQUEST_ADD_EXT).PostJsonAsync(t);
        var responseContent = await response.ResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ValidationResult>(responseContent) ?? new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("Id", "error") } };

    }
}
