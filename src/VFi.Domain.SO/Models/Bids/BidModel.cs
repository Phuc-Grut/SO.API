using VFi.Domain.SO.Models.Spider.YahooAucton;

namespace VFi.Domain.SO.Models.Bids;
public class BidResponseModel
{
    public bool Status { get; set; }
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public long? SuggestPrice { get; set; }
    public AuctionProductPublish? Product { get; set; }
}

public class CreditInfoModel
{
    public int TotalBidCredit { get; set; }
    public int TotalBidLastTimeCredit { get; set; }
    public int TotalOrderPending { get; set; }
    public int MaxBid { get; set; }
    public int TotalCredit { get; set; }
    public int CreditAvailable { get; set; }
    public bool IsMaxBid { get; set; }
}
public record BidRequest(string Id, long Price, string SellerId, bool IsHasTax, DateTime EndTime, int MaxBidCount);

public record SniperBidRegisterRequest(string Id, long Price, bool IsHasTax, int MaxBidCount);

public class BidLastTimeDetailModel
{
    public enum State
    {
        New = 0,
        Success = 1,
        Fail = 2
    }
    public string ProductId { get; set; }
    public DateTime BidTimeSchedule { get; set; }
    public bool IsProcessed { get; set; }
    public string Title { get; set; }
    public string SellerId { get; set; }
    public DateTime EndDate { get; set; }
    public string PreviewImage { get; set; }

    public string Id { get; set; }
    public string UserId { get; set; }
    public long Price { get; set; }
    public State Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RegisteredDate { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public TimeSpan TimeRemaining { get; set; }
    public decimal? CurrentPrice { get; set; }
}

public record BidOrderTrackingModel
{
    public string? ProductId { get; set; }
    public string? ShippingMethod { get; set; }
    public string? TrackingNumber { get; set; }
    public string? TrackUrl { get; set; }
    public decimal? ShippingFee { get; set; }
}
