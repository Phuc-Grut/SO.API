using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class PurchaseNotifyEvent : Event
{

    public PurchaseNotifyEvent()
    {
        base.MessageType = GetType().Name;
    }
    public string OrderCode { set; get; }
    public string CustomerName { set; get; }
    public string Price { set; get; }
    public string Link { set; get; }
    public string Date { set; get; }
    public string Status { set; get; }
}
