using Flurl.Http;
using Flurl.Http.Configuration;
using VFi.Domain.SO.Interfaces;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.Repository;

public class ExchangeRateRepository : IExchangeRateRepository
{

    private readonly MasterApiContext _apiContext;
    private const string PATH_GET_LIST_TO = "/api/exchangerate/get-list-to";
    private readonly IContextUser _context;
    public ExchangeRateRepository(MasterApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }


    public Task<double> GetRate(string currency)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(PATH_GET_LIST_TO)
                             .SetQueryParam("$currency", currency)
                             .GetJsonAsync().Result;
        return Task.FromResult(t.rate);
    }
}
