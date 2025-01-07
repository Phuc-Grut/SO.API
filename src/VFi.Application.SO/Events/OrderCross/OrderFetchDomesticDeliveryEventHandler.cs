using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;
namespace VFi.Application.SO.Events.OrderCross;

public class OrderFetchDomesticDeliveryEvent : Event
{

    public OrderFetchDomesticDeliveryEvent()
    {
        MessageType = GetType().Name;
    }
    public int? MinDays { get; set; }
    public int? MaxDays { get; set; }
    public int? MaxItems { get; set; }
    public string? CreatedName { get; set; }
    public Guid? CreatedBy { get; set; }
}

public class OrderFetchDomesticDeliveryEventHandler : INotificationHandler<OrderFetchDomesticDeliveryEvent>
{

    private readonly IEventRepository _eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public OrderFetchDomesticDeliveryEventHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(OrderFetchDomesticDeliveryEvent notification, CancellationToken cancellationToken)
    {
        var message = new Domain.SO.Events.OrderFetchDomesticDeliveryQueueEvent();
        message.Data = notification.Data;
        message.Tenant = notification.Tenant;
        message.Data_Zone = notification.Data_Zone;
        message.CreatedBy = notification.CreatedBy;
        message.CreatedName = notification.CreatedName;

        message.MaxItems = notification.MaxItems;
        message.MaxDays = notification.MaxDays;
        message.MinDays = notification.MinDays;

        _ = await _eventRepository.OrderFetchDomesticDelivery(message);
    }
}

