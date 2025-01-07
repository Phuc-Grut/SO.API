using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;
namespace VFi.Application.SO.Events.OrderCross;

public class OrderFetchTrackingEvent : Event
{

    public OrderFetchTrackingEvent()
    {
        MessageType = GetType().Name;
    }
    public int? MinDays { get; set; }
    public int? MaxDays { get; set; }
    public int? MaxItems { get; set; }
    public string? OrderType { get; set; }
    public string? AuthorizationToken { get; set; }
    public string? CreatedName { get; set; }
    public Guid? CreatedBy { get; set; }
}

public class OrderFetchTrackingEventHandler : INotificationHandler<OrderFetchTrackingEvent>
{

    private readonly IEventRepository _eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public OrderFetchTrackingEventHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(OrderFetchTrackingEvent notification, CancellationToken cancellationToken)
    {
        var message = new VFi.Domain.SO.Events.OrderFetchTrackingQueueEvent();
        message.Data = notification.Data;
        message.Tenant = notification.Tenant;
        message.Data_Zone = notification.Data_Zone;
        message.CreatedBy = notification.CreatedBy;
        message.CreatedName = notification.CreatedName;

        message.MaxItems = notification.MaxItems;
        message.MaxDays = notification.MaxDays;
        message.MinDays = notification.MinDays;
        message.OrderType = notification.OrderType;
        message.AuthorizationToken = notification.AuthorizationToken;

        _ = await _eventRepository.OrderFetchTracking(message);
    }
}

