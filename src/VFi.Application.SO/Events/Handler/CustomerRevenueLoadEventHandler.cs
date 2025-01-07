
using MassTransit.Internals;
using MediatR;
using VFi.Domain.SO.Interfaces;
namespace VFi.Application.SO.Events.Handler;

public class CustomerRevenueLoadEventHandler : INotificationHandler<CustomerRevenueLoadEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public CustomerRevenueLoadEventHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(CustomerRevenueLoadEvent notification, CancellationToken cancellationToken)
    {
        var message = new VFi.Domain.SO.Events.CustomerRevenueLoadQueueEvent();
        message.Data = notification.Data;
        message.Tenant = notification.Tenant;
        message.Data_Zone = notification.Data_Zone;
        message.BackHour = notification.BackHour;
        message.CustomerId = notification.CustomerId;
        message.AccountId = notification.AccountId;
        _ = await eventRepository.CustomerRevenueLoad(message);
    }
}
