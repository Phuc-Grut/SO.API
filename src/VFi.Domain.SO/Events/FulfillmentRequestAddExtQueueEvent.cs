using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Utilities;
namespace VFi.Domain.SO.Events;

public class FulfillmentRequestAddExtQueueEvent : QueueEvent
{

    public FulfillmentRequestAddExtQueueEvent()
    {
        base.MessageType = GetType().Name;
    }
    public FulfillmentRequestAddExt? ItemData { set; get; }
    public Guid Id { set; get; }


}
