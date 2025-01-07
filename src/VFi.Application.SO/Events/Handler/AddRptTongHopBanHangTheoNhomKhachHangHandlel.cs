
using MassTransit.Internals;
using MediatR;
using VFi.Application.SO.Events;
using VFi.Domain.SO.Interfaces;
namespace VFi.Application.SO.Events.Handler;

public class AddRptTongHopBanHangTheoNhomKhachHangHandler : INotificationHandler<AddRptTongHopBanHangTheoNhomKhachHangEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public AddRptTongHopBanHangTheoNhomKhachHangHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(AddRptTongHopBanHangTheoNhomKhachHangEvent notification, CancellationToken cancellationToken)
    {

        var message = new VFi.Domain.SO.Events.AddRptTongHopBanHangTheoNhomKhachHangQueueEvent();
        message.ReportId = notification.ReportId;
        message.EmployeeId = notification.EmployeeId;
        message.CustomerGroupId = notification.CustomerGroupId;
        message.FromDate = notification.FromDate;
        message.ToDate = notification.ToDate;

        message.Tenant = notification.Tenant;
        message.Data = notification.Data;
        message.Data_Zone = notification.Data_Zone;
        var result = await eventRepository.AddRptTongHopBanHangTheoNhomKhachHang(message);

    }
}
