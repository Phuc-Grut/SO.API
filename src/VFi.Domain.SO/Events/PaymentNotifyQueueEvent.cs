using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class PaymentNotifyQueueEvent : QueueEvent
{
    public PaymentNotifyQueueEvent()
    {
        base.MessageType = GetType().Name;
    }
    public string Body { set; get; }
    public string PaymentType { set; get; }
    public string CustomerName { set; get; }
    public string Amount { set; get; }
    public string Date { set; get; }
}
