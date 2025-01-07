using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class PurchaseNotifyAllEvent : Event
{

    public PurchaseNotifyAllEvent()
    {
        base.MessageType = GetType().Name;
    }
}
