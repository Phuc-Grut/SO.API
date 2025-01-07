using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class OrderItemFetchDomesticDeliveryQueueEvent : QueueEvent
{
    public OrderItemFetchDomesticDeliveryQueueEvent()
    {
        MessageType = GetType().Name;
    }

    public Guid? Id { get; set; }
    public string? DomesticCarrier { get; set; }
    public string? DomesticTracking { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? CreatedName { get; set; }

}
