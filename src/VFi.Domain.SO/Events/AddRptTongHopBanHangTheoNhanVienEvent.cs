using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class AddRptTongHopBanHangTheoNhanVienQueueEvent : QueueEvent
{
    public AddRptTongHopBanHangTheoNhanVienQueueEvent()
    {
        base.MessageType = GetType().Name;
    }

    public Guid ReportId { get; set; }
    public string? CustomerCode { get; set; }
    public string? EmployeeId { get; set; }
    public string? CategoryRootId { get; set; }
    public string? ProductCode { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
