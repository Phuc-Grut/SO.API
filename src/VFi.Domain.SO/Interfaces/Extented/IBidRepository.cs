using VFi.Domain.SO.Models.Bids;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces.Extented;

public interface IBidRepository
{
    Task<EntityResponse<CreditInfoModel>> CreditInfo(string accountId);
    Task AggregateBidCredit(Guid accountId);
    Task<EntityResponse<BidResponseModel>> Bid(BidRequest request);
    Task<EntityResponse<BidLastTimeDetailModel>> SniperBidRegister(SniperBidRegisterRequest request);
    Task<EntityResponse<BidOrderTrackingModel>> BidOrderTrackingItem(string productId, string sellerId, string yaUserName);
}
