using VFi.Domain.SO.Models.Spider;
using VFi.Domain.SO.Models.Spider.Mercari;
using VFi.Domain.SO.Models.Spider.YahooAucton;

namespace VFi.Domain.SO.Interfaces;

public interface IPartnerRepository
{
    Task<AuctionProductPublish?> DetailAuction(string? productId);
    Task<MercariProductPublish?> DetailMercari(string? productId);
    Task<MercariTrackingInfo?> DetailTrackingMercari(string? productId, string? authorizationToken = "");
    Task<ProductDetail?> FindProduct(string productUrl);
}
