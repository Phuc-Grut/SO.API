using Flurl.Http;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models.Spider;
using VFi.Domain.SO.Models.Spider.Mercari;
using VFi.Domain.SO.Models.Spider.YahooAucton;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository.Extented;

public class PartnerRepository : IPartnerRepository
{
    private readonly SpiderApiContext _apiContext;

    private const string PATH_AUCTION_GET_DETAIL = "/api/auction/get-item";
    private const string PATH_MERCARI_GET_DETAIL = "/api/mercari/get-item";
    private const string PATH_MERCARI_GET_DETAIL_TRACKING = "/api/mercari/get-item-tracking-info";
    private const string PATH_GET_DETAIL_BY_URL = "/api/product/get-by-url";
    public PartnerRepository(SpiderApiContext apiContext)
    {
        _apiContext = apiContext;
    }
    public async Task<AuctionProductPublish?> DetailAuction(string? productId)
    {
        var result = await _apiContext.Client
            .Request(PATH_AUCTION_GET_DETAIL)
            .SetQueryParam("itemId", productId)
            .GetJsonAsync<AuctionProductPublish>();
        if (result != null)
        {
            return result;
        }
        return null;
    }

    public async Task<MercariProductPublish?> DetailMercari(string? productId)
    {
        var result = await _apiContext.Client
            .Request(PATH_MERCARI_GET_DETAIL)
            .SetQueryParam("itemId", productId)
            .GetJsonAsync<MercariProductPublish>();
        if (result != null)
        {
            return result;
        }
        return null;
    }


    public async Task<MercariTrackingInfo?> DetailTrackingMercari(string? productId, string? authorizationToken = "")
    {
        var result = await _apiContext.Client
            .Request(PATH_MERCARI_GET_DETAIL_TRACKING)
            .SetQueryParam("itemId", productId)
            .SetQueryParam("authorizationToken", authorizationToken)
            .GetJsonAsync<MercariTrackingInfo>();
        if (result != null)
        {
            return result;
        }
        return null;
    }


    public async Task<ProductDetail?> FindProduct(string productUrl)
    {
        productUrl = Uri.EscapeDataString(productUrl);
        var result = await _apiContext.Client
            .Request(PATH_GET_DETAIL_BY_URL)
            .SetQueryParam("url", productUrl)
            .GetJsonAsync<EntityResponse<ProductDetail>>();
        if (result != null)
        {
            return result.Data;
        }
        return null;
    }
}
