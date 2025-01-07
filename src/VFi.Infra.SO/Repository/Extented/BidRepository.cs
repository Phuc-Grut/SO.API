using Flurl.Http;
using VFi.Domain.SO.Interfaces.Extented;
using VFi.Domain.SO.Models.Bids;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository.Extented;

public class BidRepository : IBidRepository
{
    private readonly BidApiContext _apiContext;
    private const string PATH_BID = "/api/bid/bid";
    private const string PATH_BID_LAST_TIME_REGISTER = "/api/bid/bid-last-time";
    private const string PATH_BID_CREDIT_INFO = "/api/bid/bid-credit/{0}";
    private const string PATH_AGGREGATE_BID_CREDIT = "/api/event/aggregate-bid-credit";
    private const string PATH_GET_BID_TRACKING_ITEM = "/api/event/bid-tracking-item";
    private readonly IContextUser _context;
    public BidRepository(BidApiContext apiContext, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }

    public async Task<EntityResponse<CreditInfoModel>> CreditInfo(string accountId)
    {
        _apiContext.Token = _context.GetToken();
        var result = await _apiContext
            .Client
            .Request(string.Format(PATH_BID_CREDIT_INFO, accountId))
            .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
            .GetAsync()
            .ReceiveJson<EntityResponse<CreditInfoModel>>();
        return result;
    }
    public async Task<EntityResponse<BidResponseModel>> Bid(BidRequest request)
    {
        _apiContext.Token = _context.GetToken();
        var result = await _apiContext
            .Client
            .Request(PATH_BID)
            .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
            .PostJsonAsync(request)
            .ReceiveJson<EntityResponse<BidResponseModel>>();
        return result;
    }

    public async Task<EntityResponse<BidLastTimeDetailModel>> SniperBidRegister(SniperBidRegisterRequest request)
    {
        _apiContext.Token = _context.GetToken();
        var result = await _apiContext
             .Client
             .Request(PATH_BID_LAST_TIME_REGISTER)
             .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
             .PostJsonAsync(request)
             .ReceiveJson<EntityResponse<BidLastTimeDetailModel>>();
        return result;
    }

    public async Task AggregateBidCredit(Guid accountId)
    {
        await _apiContext
             .Client
             .Request(PATH_AGGREGATE_BID_CREDIT)
             .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
             .PutJsonAsync(new
             {
                 accountId = accountId.ToString()
             });
    }


    public async Task<EntityResponse<BidOrderTrackingModel>> BidOrderTrackingItem(string productId, string sellerId, string yaUserName)
    {
        _apiContext.Token = _context.GetToken();
        var result = await _apiContext
             .Client
             .Request(PATH_GET_BID_TRACKING_ITEM)
             .SetQueryParam("productId", productId)
             .SetQueryParam("sellerId", sellerId)
             .SetQueryParam("yaUserName", yaUserName)
             .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
             .GetJsonAsync<EntityResponse<BidOrderTrackingModel>>();
        return result;
    }
}
