
using MassTransit.Internals;
using MediatR;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;
namespace VFi.Application.SO.Events.Handler;

public class PurchaseNotifyAllEventHandler : INotificationHandler<PurchaseNotifyAllEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public PurchaseNotifyAllEventHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(PurchaseNotifyAllEvent notification, CancellationToken cancellationToken)
    {
        var message = new VFi.Domain.SO.Events.PurchaseNotifyAllQueueEvent();
        message.Data = notification.Data;
        message.Tenant = notification.Tenant;
        message.Data_Zone = notification.Data_Zone;
        _ = await eventRepository.PurchaseNotifyAll(message);
    }
}

