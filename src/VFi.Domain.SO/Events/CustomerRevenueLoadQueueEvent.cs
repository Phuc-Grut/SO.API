using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class CustomerRevenueLoadQueueEvent : QueueEvent
{
    public CustomerRevenueLoadQueueEvent()
    {
        base.MessageType = GetType().Name;
    }
    public int? BackHour { set; get; }
    public Guid? AccountId { set; get; }
    public Guid? CustomerId { set; get; }
}
