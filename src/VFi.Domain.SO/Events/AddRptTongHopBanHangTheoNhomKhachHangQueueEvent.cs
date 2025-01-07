using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class AddRptTongHopBanHangTheoNhomKhachHangQueueEvent : QueueEvent
{
    public AddRptTongHopBanHangTheoNhomKhachHangQueueEvent()
    {
        base.MessageType = GetType().Name;
    }

    public Guid ReportId { get; set; }
    public string? EmployeeId { get; set; }
    public string? CustomerGroupId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
