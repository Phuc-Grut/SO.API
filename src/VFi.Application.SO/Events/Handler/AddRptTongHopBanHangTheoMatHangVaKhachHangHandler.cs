﻿
using MassTransit.Internals;
using MediatR;
using VFi.Application.SO.Events;
using VFi.Domain.SO.Interfaces;
namespace VFi.Application.SO.Events.Handler;

public class AddRptTongHopBanHangTheoMatHangVaKhachHangHandler : INotificationHandler<AddRptTongHopBanHangTheoMatHangVaKhachHangEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public AddRptTongHopBanHangTheoMatHangVaKhachHangHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(AddRptTongHopBanHangTheoMatHangVaKhachHangEvent notification, CancellationToken cancellationToken)
    {

        var message = new VFi.Domain.SO.Events.AddRptTongHopBanHangTheoMatHangVaKhachHangQueueEvent();
        message.ReportId = notification.ReportId;
        message.CustomerCode = notification.CustomerCode;
        message.EmployeeId = notification.EmployeeId;
        message.CategoryRootId = notification.CategoryRootId;
        message.ProductCode = notification.ProductCode;
        message.FromDate = notification.FromDate;
        message.ToDate = notification.ToDate;

        message.Tenant = notification.Tenant;
        message.Data = notification.Data;
        message.Data_Zone = notification.Data_Zone;
        var result = await eventRepository.AddRptTongHopBanHangTheoMatHangVaKhachHang(message);

    }
}
