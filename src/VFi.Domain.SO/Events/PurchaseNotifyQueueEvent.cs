using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Domain.SO.Events;

public class PurchaseNotifyQueueEvent : QueueEvent
{
    public PurchaseNotifyQueueEvent()
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

public class PurchaseNotifyAllQueueEvent : QueueEvent
{
    public PurchaseNotifyAllQueueEvent()
    {
        base.MessageType = GetType().Name;
    }
}

////Mã đơn hàng: <strong>MG0000001XX</strong>
////Khách hàng: <strong>Nguyễn Văn A</strong>
////Tiền hàng: <strong>￥100.000</strong>
////Link: https://auction.megabuy.jp/product/v1141494789
////Ngày tạo: 2024/06/25 23:01
////Trạng thái: Chờ mua hàng
