using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class OrderStatusChangedQueueEvent : QueueEvent
{
    public OrderStatusChangedQueueEvent()
    {
        base.MessageType = GetType().Name;
    }

    public Guid OrderId { get; set; }
    public int? FromStatus { get; set; }
    public int? ToStatus { get; set; }
    public DateTime ChangeDate { get; set; }
    public Guid RequestBy {  get; set; }
    public string RequestByName { get; set; }
    public string RequestByEmail { get; set; }
}
