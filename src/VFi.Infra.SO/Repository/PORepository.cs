
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

public class PORepository : IPORepository
{

    private readonly POApiContext _apiContext;
    private readonly string PATH_REQUEST_ADD_EXT = "/api/purchaserequest/add-ext";
    private readonly string PATH_GET_QUANTITY_PURCHASED = "/api/purchaseorder/get-quantity-purchased";
    private readonly IContextUser _context;
    public PORepository(POApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }

    public async Task<ValidationResult> AddExt(PORequestPurchase t)
    {
        _apiContext.Token = _context.GetToken();
        var response = await _apiContext.Client.Request(PATH_REQUEST_ADD_EXT).PostJsonAsync(t);
        var responseContent = await response.ResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<ValidationResult>(responseContent) ?? new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("Id", "error") } };

    }
    public async Task<IEnumerable<QuantityPuchased>> GetQuantityPuchased(int? notCancel, string? purchaseRequestCode)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(PATH_GET_QUANTITY_PURCHASED)
                             .SetQueryParam("$notCancel", notCancel)
                             .SetQueryParam("$purchaseRequestCode", purchaseRequestCode)
                             .GetAsync().Result;
        var responseContent = await t.ResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<QuantityPuchased>>(responseContent);

    }
}
