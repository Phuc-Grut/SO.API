using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Interfaces.Extented;
using VFi.NetDevPack.Context;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Web;
using DocumentFormat.OpenXml.Office2010.Excel;
using VFi.NetDevPack.Mediator;
using System.Threading;


namespace VFi.Infra.SO.MassTransit.Consumers;

public class OrderFetchDomesticDeliveryConsumer : IConsumer<OrderFetchDomesticDeliveryQueueEvent>
{
    private readonly ILogger<OrderFetchDomesticDeliveryQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;
    private readonly IPublishEndpoint _publishEndpoint;


    public OrderFetchDomesticDeliveryConsumer(ILogger<OrderFetchDomesticDeliveryQueueEvent> logger,
         IServiceProvider serviceFactory,
         IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Consume(ConsumeContext<OrderFetchDomesticDeliveryQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));
        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        //var _shippingCarrierRepository = _serviceFactory.GetService(typeof(IShippingCarrierRepository)) as IShippingCarrierRepository;
        var _procedures = _serviceFactory.GetService(typeof(ISOExtProcedures)) as ISOExtProcedures;
        var _repository = _serviceFactory.GetService(typeof(IOrderRepository)) as IOrderRepository;

        //var allShippingCarrier = await _shippingCarrierRepository!.GetAll();

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

        await FetchOrder(_repository!, _procedures!, msg, currentTime, minTime, maxTime, msg.MaxItems);

    }


    private async Task FetchOrder(
        IOrderRepository repository,
        ISOExtProcedures procedures,
        OrderFetchDomesticDeliveryQueueEvent message,
        DateTime currentTime,
        DateTime minTime,
        DateTime maxTime,
        int? maxItems)
    {
        _logger.LogInformation("Fetch DomesticDelivery Order: minTime {@minTime}", minTime);
        _logger.LogInformation("Fetch DomesticDelivery Order: maxTime {@maxTime}", maxTime);
        _logger.LogInformation("Fetch DomesticDelivery Order: maxItems {@maxItems}", maxItems);
        var orders = await repository.GetOrderWithDomesticTracking(minTime, maxTime, maxItems);
        if (orders != null && orders.Any())
        {
            _logger.LogInformation("Fetch DomesticDelivery Order - Order count: {count}", orders.Count());
            foreach (var order in orders)
            {
                var ev = new OrderItemFetchDomesticDeliveryQueueEvent()
                {
                    Id = order.Id,
                    DomesticCarrier = order.DomesticCarrier,
                    DomesticTracking = order.DomesticTracking,
                    CreatedDate = order.CreatedDate,
                    Data = message.Data,
                    Data_Zone = message.Data_Zone,
                    Tenant = message.Tenant,
                    CreatedBy = order.CreatedBy,
                    CreatedName = order.CreatedByName,
                };

                await _publishEndpoint.Publish(ev);
            }
        }
    }


}
