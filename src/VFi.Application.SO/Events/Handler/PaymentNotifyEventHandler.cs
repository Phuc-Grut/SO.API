
using MassTransit.Internals;
using MediatR;
using VFi.Domain.SO.Interfaces;
namespace VFi.Application.SO.Events.Handler;

public class PaymentNotifyEventHandler : INotificationHandler<PaymentNotifyEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public PaymentNotifyEventHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(PaymentNotifyEvent notification, CancellationToken cancellationToken)
    {

        var message = new VFi.Domain.SO.Events.PaymentNotifyQueueEvent();
        message.Data = notification.Data;
        message.Tenant = notification.Tenant;
        message.Data_Zone = notification.Data_Zone;
        message.CustomerName = notification.CustomerName;
        message.Body = notification.Body;
        message.Amount = notification.Amount;
        message.Date = notification.Date;
        message.PaymentType = notification.PaymentType;
        _ = await eventRepository.PaymentNotify(message);
    }
}
