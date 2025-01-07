using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Utilities;

namespace VFi.Application.SO.Events;

public class PaymentNotifyEvent : Event
{

    public PaymentNotifyEvent()
    {
        base.MessageType = GetType().Name;
    }
    public string Body { set; get; }
    public string PaymentType { set; get; }
    public string CustomerName { set; get; }
    public string Amount { set; get; }
    public string Date { set; get; }
}
