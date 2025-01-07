using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.DTOs;

public class PagedResultPaymentInvoice<T> : PagedResult<T>
{
    public PagedResultPaymentInvoice()
    {

    }
    public PagedResultPaymentInvoice(
        T data,
        int totalCount,
        int pageNumber,
        int pageSize,
        decimal? tongThu,
        decimal? tongGiamGia,
        decimal? tongTraHang,
        decimal? tongChi,
        decimal? tongHuyDon
    )
    {
        Items = data;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TongThu = tongThu;
        TongGiamGia = tongGiamGia;
        TongTraHang = tongTraHang;
        TongChi = tongChi;
        TongHuyDon = tongHuyDon;
    }
    public decimal? TongThu { get; set; }
    public decimal? TongGiamGia { get; set; }
    public decimal? TongTraHang { get; set; }
    public decimal? TongChi { get; set; }
    public decimal? TongTTDon { get; set; }
    public decimal? TongHuyDon { get; set; }
    public decimal? TongThuHuyDon { get; set; }
    public decimal? TongNapTien { get; set; }
    public decimal? TongRutTien { get; set; }
}
public class PaymentInvoiceDto
{
    public Guid? Id { get; set; }
    public string? Type { get; set; }
    public string? Code { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? SaleDiscountId { get; set; }
    public Guid? ReturnOrderId { get; set; }
    public Guid? RerferenceId { get; set; }
    public string? RerferenceCode { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentMethodName { get; set; }
    public string? PaymentMethodCode { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public string? Bank { get; set; }
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? BankNumber { get; set; }
    public string? PaymentCode { get; set; }
    public string? PaymentNote { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public int? Locked { get; set; }
    public int? PaymentStatus { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public Guid? OrderExpressId { get; set; }
    public string? OrderExpressCode { get; set; }
    public List<FileDto>? File { get; set; }
}
public class DeletePaymentInvoiceDto
{
    public Guid Id { get; set; }
}
public class PaymentInvoiceQueryParams
{
    public int? Status { get; set; }
    public Guid? OrderId { get; set; }
}
