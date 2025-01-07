
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public partial interface IEventRepository
{
    Task<Boolean> AddRptSoChiTietBanHang(AddRptSoChiTietBanHangQueueEvent message);
    Task<Boolean> AddRptTinhHinhThucHienDonBanHang(AddRptTinhHinhThucHienDonBanHangQueueEvent message);
    Task<Boolean> AddRptTinhHinhThucHienHopDongBan(AddRptTinhHinhThucHienHopDongBanQueueEvent message);
    Task<Boolean> AddRptTongHopBanHangTheoKhachHang(AddRptTongHopBanHangTheoKhachHangQueueEvent message);
    Task<Boolean> AddRptTongHopBanHangTheoKhachHangVaDonBanHang(AddRptTongHopBanHangTheoKhachHangVaDonBanHangQueueEvent message);
    Task<Boolean> AddRptTongHopBanHangTheoMatHang(AddRptTongHopBanHangTheoMatHangQueueEvent message);
    Task<Boolean> AddRptTongHopBanHangTheoMatHangVaKhachHang(AddRptTongHopBanHangTheoMatHangVaKhachHangQueueEvent message);
    Task<Boolean> AddRptTongHopBanHangTheoNhanVien(AddRptTongHopBanHangTheoNhanVienQueueEvent message);
    Task<Boolean> AddRptTongHopBanHangTheoNhomKhachHang(AddRptTongHopBanHangTheoNhomKhachHangQueueEvent message);
    Task<Boolean> FulfillmentRequestAddExt(FulfillmentRequestAddExtQueueEvent message);
    Task<bool> CustomerRevenueLoad(CustomerRevenueLoadQueueEvent message);
    Task<bool> PaymentNotify(PaymentNotifyQueueEvent message);
    Task<bool> PurchaseNotify(PurchaseNotifyQueueEvent message);
    Task<bool> PurchaseNotifyAll(PurchaseNotifyAllQueueEvent message);
    Task<bool> OrderFetchTracking(OrderFetchTrackingQueueEvent message);
    Task<bool> OrderFetchDomesticDelivery(OrderFetchDomesticDeliveryQueueEvent message);
    Task<bool> OrderItemFetchDomesticDelivery(OrderItemFetchDomesticDeliveryQueueEvent message);
    Task<bool> OrderStatusChangedEvent(OrderStatusChangedQueueEvent message);
    Task<bool> PurchaseRequestStatusChangedEvent(PurchaseRequestStatusChangedQueueEvent message);
}
