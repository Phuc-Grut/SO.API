using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Interfaces.Extented;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.MassTransit.Consumers;

public class OrderFetchTrackingConsumer : IConsumer<OrderFetchTrackingQueueEvent>
{
    private readonly ILogger<OrderFetchTrackingQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;
    private readonly IBidRepository _bidRepository;
    private readonly IPartnerRepository _partnerRepository;

    public OrderFetchTrackingConsumer(ILogger<OrderFetchTrackingQueueEvent> logger,
         IServiceProvider serviceFactory,
         IBidRepository bidRepository,
         IPartnerRepository partnerRepository)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
        _bidRepository = bidRepository;
        _partnerRepository = partnerRepository;
    }
    public async Task Consume(ConsumeContext<OrderFetchTrackingQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));
        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        //object? db = _serviceFactory.GetService(typeof(SqlCoreContext));
        var _procedures = _serviceFactory.GetService(typeof(ISOExtProcedures)) as ISOExtProcedures;
        var _repository = _serviceFactory.GetService(typeof(IOrderRepository)) as IOrderRepository;

        _logger.LogInformation("OrderFetchTrackingQueueEvent: msg {@msg}", msg);

        if (msg == null
             || !msg.MinDays.HasValue
             || !msg.MaxDays.HasValue
             || !msg.MaxItems.HasValue
             || msg.MaxItems.Value < 0
             || msg.MinDays.Value < 0
             || msg.MaxDays.Value <= msg.MinDays.Value
             )
        {
            return;
        }
        var currentTime = DateTime.UtcNow;
        var minTime = msg.MaxDays.Value == 0 ? currentTime : currentTime.AddDays(-msg.MaxDays.Value);
        var maxTime = msg.MinDays.Value == 0 ? currentTime : currentTime.AddDays(-msg.MinDays.Value);

        switch (msg.OrderType)
        {
            case "AUC":
                await FetchAuctionOrder(_repository!, _procedures!, msg, msg.OrderType, currentTime, minTime, maxTime, msg.MaxItems);
                break;

            case "MER":
                await FetchMercariOrder(_repository!, _procedures!, msg, msg.OrderType, currentTime, minTime, maxTime, msg.MaxItems);
                break;
            default:
                break;
        }
    }

    private async Task FetchAuctionOrder(
        IOrderRepository repository,
        ISOExtProcedures procedures,
        OrderFetchTrackingQueueEvent message,
        string orderType,
        DateTime currentTime,
        DateTime minTime,
        DateTime maxTime,
        int? maxItems)
    {
        _logger.LogInformation("FetchAuctionOrder: minTime {@minTime}", minTime);
        _logger.LogInformation("FetchAuctionOrder: maxTime {@maxTime}", maxTime);
        _logger.LogInformation("FetchAuctionOrder: maxItems {@maxItems}", maxItems);
        var orders = await repository.GetAuctionWithoutDomesticTracking(minTime, maxTime, maxItems);
        if (orders != null && orders.Any())
        {
            _logger.LogInformation("FetchAuctionOrder - Order count: {count}", orders.Count());
            foreach (var order in orders)
            {
                _logger.LogInformation("FetchAuctionOrder - Order product count: {count}", order.OrderProduct.Count());
                foreach (var product in order.OrderProduct)
                {
                    if (!string.IsNullOrEmpty(product.SellerId)
                        && !string.IsNullOrEmpty(product.BidUsername)
                        && !string.IsNullOrEmpty(product.SourceCode))
                    {
                        try
                        {
                            _logger.LogInformation("FetchAuctionOrder - get BidOrderTrackingItem");
                            var trackingResponse = await _bidRepository.BidOrderTrackingItem(product.SourceCode, product.SellerId, product.BidUsername);
                            if (trackingResponse != null
                                && trackingResponse.Status
                                && trackingResponse.Data != null
                                && !string.IsNullOrEmpty(trackingResponse.Data.TrackingNumber))
                            {
                                if (string.IsNullOrEmpty(order.DomesticTracking))
                                {
                                    order.DomesticTracking = trackingResponse.Data.TrackingNumber;
                                }
                                else
                                {
                                    order.DomesticTracking += $"; {trackingResponse.Data.TrackingNumber}";
                                }

                                if (!string.IsNullOrEmpty(trackingResponse.Data.ShippingMethod))
                                {
                                    if (string.IsNullOrEmpty(order.DomesticCarrier))
                                    {
                                        order.DomesticCarrier = trackingResponse.Data.ShippingMethod;
                                    }
                                    else
                                    {
                                        order.DomesticCarrier += $"; {trackingResponse.Data.ShippingMethod}";
                                    }
                                }

                                var carrier = ConvertCarrierAuction(trackingResponse.Data.ShippingMethod);
                                await procedures.SP_ORDER_UPDATE_ADD_TRACKINGAsync(order.Id,
                                    trackingResponse.Data.TrackingNumber,
                                    carrier,
                                    message.CreatedBy,
                                    message.CreatedName);

                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }
                        await Delay(8, 30);
                    }

                }
            }
        }
    }



    private async Task FetchMercariOrder(
       IOrderRepository repository,
       ISOExtProcedures procedures,
       OrderFetchTrackingQueueEvent message,
       string orderType,
       DateTime currentTime,
       DateTime minTime,
       DateTime maxTime,
       int? maxItems)
    {
        _logger.LogInformation("FetchMercariOrder: minTime {@minTime}", minTime);
        _logger.LogInformation("FetchMercariOrder: maxTime {@maxTime}", maxTime);
        _logger.LogInformation("FetchMercariOrder: maxItems {@maxItems}", maxItems);
        var orders = await repository.GetMercariWithoutDomesticTracking(minTime, maxTime, maxItems);
        if (orders != null && orders.Any())
        {
            _logger.LogInformation("FetchMercariOrder - Order count: {count}", orders.Count());
            foreach (var order in orders)
            {
                _logger.LogInformation("FetchMercariOrder - Order product count: {count}", order.OrderProduct.Count());
                foreach (var product in order.OrderProduct)
                {
                    if (!string.IsNullOrEmpty(product.SourceCode))
                    {
                        try
                        {
                            _logger.LogInformation("FetchMercariOrder - get Mercari TrackingItem");
                            var item = await _partnerRepository.DetailTrackingMercari(product.SourceCode, message.AuthorizationToken);
                            if (item is not null && !string.IsNullOrEmpty(item.DomesticTracking))
                            {
                                var carrier = ConvertCarrierMercari(item.DeliveryCarrier);
                                await procedures.SP_ORDER_UPDATE_ADD_TRACKINGAsync(order.Id,
                                    item.DomesticTracking,
                                    carrier,
                                    message.CreatedBy,
                                    message.CreatedName);
                            }
                            else
                            {
                                if (item is not null && !string.IsNullOrEmpty(item.DeliveryCarrier))
                                {
                                    var carrier = ConvertCarrierMercari(item.DeliveryCarrier);
                                    if (carrier != order.DomesticCarrier)
                                    {
                                        await procedures.SP_ORDER_UPDATE_ADD_TRACKINGAsync(order.Id,
                                            item.DomesticTracking,
                                            carrier,
                                            message.CreatedBy,
                                            message.CreatedName);
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }
                        await Delay(8, 30);
                    }

                }
            }
        }
    }


    private string ConvertCarrierMercari(string? carrier)
    {
        if (!string.IsNullOrEmpty(carrier))
        {

            if (carrier.Contains("yamato"))
            {
                return "Yamato";
            }

            if (carrier.Contains("japan_post"))
            {
                return "JapanPost";
            }

            return carrier.Trim();
        }

        return string.Empty;
    }


    private string ConvertCarrierAuction(string? carrier)
    {
        if (!string.IsNullOrEmpty(carrier))
        {
            if (carrier.Contains("佐川急便"))
            {
                return "Sagawa";
            }
            if (carrier.Contains("佐川"))
            {
                return "Sagawa";
            }
            if (carrier.Contains("ヤマト運輸"))
            {
                return "Yamato";
            }
            if (carrier.Contains("ネコポス"))
            {
                return "Yamato";
            }
            if (carrier.Contains("EAZY"))
            {
                return "Yamato";
            }
            if (carrier.Contains("らくらく家財便"))
            {
                return "Yamato";
            }
            if (carrier.Contains("日本郵便"))
            {
                return "JapanPost";
            }
            if (carrier.Contains("ゆうパック"))
            {
                return "JapanPost";
            }
            if (carrier.Contains("レターパックライト"))
            {
                return "JapanPost";
            }
            if (carrier.Contains("ゆうパケット"))
            {
                return "JapanPost";
            }
            if (carrier.Contains("レターパックプラス"))
            {
                return "JapanPost";
            }
            if (carrier.Contains("□指定便"))
            {
                return "JapanPost";
            }
            if (carrier.Trim().Equals("指定便"))
            {
                return "JapanPost";
            }
            return carrier.Trim();
        }

        return string.Empty;
    }

    private Task Delay(int minSecondDelay = 0, int maxSecondDelay = 30)
    {
        var random = new Random();
        var timeDelay = random.Next(minSecondDelay, maxSecondDelay);
        var timespanDelay = TimeSpan.FromSeconds(timeDelay);

        return Task.Delay(timespanDelay);
    }
}


