using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models.Bids;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class MyRepository : IMyRepository
{
    private readonly MyApiContext _apiContext;
    private const string PATH_DEPOSIT_NOTIFY = "/api/hub/deposit-wallet";
    private readonly IContextUser _context;
    public MyRepository(MyApiContext apiContext, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }
    public async Task DepositPushNotify(Guid accountId, decimal amount, string code, string description)
    {
        _apiContext.Token = _context.GetToken();
        var result = await _apiContext
             .Client
             .Request(PATH_DEPOSIT_NOTIFY)
             .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
             .PostJsonAsync(new
             {
                 AccountId = accountId.ToString(),
                 Method = "WALLET",
                 Wallet = "VND",
                 Amount = amount,
                 Code = code,
                 Description = description
             });
        ;
    }

}
