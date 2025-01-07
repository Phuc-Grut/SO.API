using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events
{
    public class PurchaseRequestStatusChangedQueueEvent : QueueEvent
    {
        public PurchaseRequestStatusChangedQueueEvent()
        {
            base.MessageType = GetType().Name;
        }

        public Guid PurchaseRequestId { get; set; }
        public int? Status { get; set; }
        public string? ApproveComment { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
