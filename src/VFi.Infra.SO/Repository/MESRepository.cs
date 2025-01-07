
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

public class MESRepository : IMESRepository
{

    private readonly MESApiContext _apiContext;
    private readonly string PATH_REQUEST_ADD_EXT = "/api/productionorder/add-ext";
    private readonly IContextUser _context;
    public MESRepository(MESApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }

    public async Task<ValidationResult> AddExt(MESProductionOrder t)
    {
        _apiContext.Token = _context.GetToken();
        var response = await _apiContext.Client.Request(PATH_REQUEST_ADD_EXT).PostJsonAsync(t);
        var responseContent = await response.ResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ValidationResult>(responseContent) ?? new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("Id", "error") } };

    }
}
