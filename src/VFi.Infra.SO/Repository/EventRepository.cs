
using MassTransit;
using Microsoft.Extensions.Logging;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;

namespace VFi.Infra.SO.Repository;

public partial class EventRepository : IEventRepository
{
    private readonly ILogger<EventRepository> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public EventRepository()
    {
    }

    public EventRepository(ILogger<EventRepository> logger, IPublishEndpoint publishEndpoint = null)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> AddRptSoChiTietBanHang(AddRptSoChiTietBanHangQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTinhHinhThucHienDonBanHang(AddRptTinhHinhThucHienDonBanHangQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTinhHinhThucHienHopDongBan(AddRptTinhHinhThucHienHopDongBanQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTongHopBanHangTheoKhachHang(AddRptTongHopBanHangTheoKhachHangQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTongHopBanHangTheoKhachHangVaDonBanHang(AddRptTongHopBanHangTheoKhachHangVaDonBanHangQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTongHopBanHangTheoMatHang(AddRptTongHopBanHangTheoMatHangQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTongHopBanHangTheoMatHangVaKhachHang(AddRptTongHopBanHangTheoMatHangVaKhachHangQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTongHopBanHangTheoNhanVien(AddRptTongHopBanHangTheoNhanVienQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> AddRptTongHopBanHangTheoNhomKhachHang(AddRptTongHopBanHangTheoNhomKhachHangQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> FulfillmentRequestAddExt(FulfillmentRequestAddExtQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> CustomerRevenueLoad(CustomerRevenueLoadQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> PaymentNotify(PaymentNotifyQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> PurchaseNotify(PurchaseNotifyQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> PurchaseNotifyAll(PurchaseNotifyAllQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> OrderFetchTracking(OrderFetchTrackingQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> OrderFetchDomesticDelivery(OrderFetchDomesticDeliveryQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> OrderItemFetchDomesticDelivery(OrderItemFetchDomesticDeliveryQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> OrderStatusChangedEvent(OrderStatusChangedQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }

    public async Task<bool> PurchaseRequestStatusChangedEvent(PurchaseRequestStatusChangedQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
}
