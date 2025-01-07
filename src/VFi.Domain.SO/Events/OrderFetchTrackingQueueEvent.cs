using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class OrderFetchTrackingQueueEvent : QueueEvent
{
    public OrderFetchTrackingQueueEvent()
    {
        MessageType = GetType().Name;
    }

    public Guid? CreatedBy { get; set; }
    public string? CreatedName { get; set; }
    public int? MinDays { get; set; }
    public int? MaxDays { get; set; }
    public int? MaxItems { get; set; }
    public string? OrderType { get; set; }
    public string? AuthorizationToken { get; set; }
}
