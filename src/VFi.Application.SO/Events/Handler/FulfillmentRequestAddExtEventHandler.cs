
using MassTransit.Internals;
using MediatR;
using VFi.Domain.SO.Interfaces;
namespace VFi.Application.SO.Events.Handler;

public class FulfillmentRequestAddExtEventHandler : INotificationHandler<FulfillmentRequestAddExtEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public FulfillmentRequestAddExtEventHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(FulfillmentRequestAddExtEvent notification, CancellationToken cancellationToken)
    {
        var message = new VFi.Domain.SO.Events.FulfillmentRequestAddExtQueueEvent();
        message.ItemData = notification.ItemData;
        message.Data = notification.Data;
        message.Tenant = notification.Tenant;
        message.Data_Zone = notification.Data_Zone;
        message.Id = notification.Id;
        _ = await eventRepository.FulfillmentRequestAddExt(message);
    }
}
