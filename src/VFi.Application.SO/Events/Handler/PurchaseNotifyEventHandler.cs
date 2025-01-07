
using MassTransit.Internals;
using MediatR;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;
namespace VFi.Application.SO.Events.Handler;

public class PurchaseNotifyEventHandler : INotificationHandler<PurchaseNotifyEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public PurchaseNotifyEventHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(PurchaseNotifyEvent notification, CancellationToken cancellationToken)
    {
        var message = new VFi.Domain.SO.Events.PurchaseNotifyQueueEvent();
        message.Data = notification.Data;
        message.Tenant = notification.Tenant;
        message.Data_Zone = notification.Data_Zone;
        message.OrderCode = notification.OrderCode;
        message.CustomerName = notification.CustomerName;
        message.Price = notification.Price;
        message.Link = notification.Link;
        message.Date = notification.Date;
        message.Status = notification.Status;
        _ = await eventRepository.PurchaseNotify(message);
    }
}

