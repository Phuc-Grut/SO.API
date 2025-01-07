using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Events;
public class CustomerRevenueLoadEvent : Event
{
    public CustomerRevenueLoadEvent()
    {
        base.MessageType = GetType().Name;
    }
    public int? BackHour { set; get; }
    public Guid? AccountId { set; get; }
    public Guid? CustomerId { set; get; }
}

