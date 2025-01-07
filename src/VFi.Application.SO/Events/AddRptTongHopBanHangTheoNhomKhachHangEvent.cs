using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Events;

public class AddRptTongHopBanHangTheoNhomKhachHangEvent : Event
{
    public AddRptTongHopBanHangTheoNhomKhachHangEvent()
    {
        base.MessageType = GetType().Name;
    }

    public Guid ReportId { get; set; }
    public string? EmployeeId { get; set; }
    public string? CustomerGroupId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
