using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Models
{
    public class POExternalPurchaseRequestAdd
    {
        public string Code { get; set; } = null!;
        public Guid? RequestBy { get; set; }
        public string? RequestByName { get; set; }
        public string? RequestByEmail { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencyName { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string? Calculation { get; set; }
        public string? Proposal { get; set; }
        public string? Note { get; set; }
        public DateTime? ApproveDate { get; set; }
        public Guid? ApproveBy { get; set; }
        public string? ApproveByName { get; set; }
        public string? ApproveComment { get; set; }
        public int? Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public Guid? PurchaseTypeId { get; set; }
        public string? PurchaseType { get; set; }
        public int? SoStatus { get; set; }
        public string? Socode { get; set; }
        public decimal? QuantityRequest { get; set; }
        public decimal? QuantityApproved { get; set; }
        public int? StatusPurchase { get; set; }
        public decimal? QuantityPurchase { get; set; }
        public decimal? QuantityPurchased { get; set; }
        public string? File { get; set; }
        public List<POPurchaseRequestDetail>? PurchaseRequestDetails { get; set; }
    }

    public class POPurchaseRequestDetail
    {
        public Guid Id { get; set; }
        public Guid PurchaseRequestDetailId { get; set; }
        public Guid PurchaseRequestId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductImage { get; set; }
        public string SourceLink { get; set; }
        public decimal? ShippingFee { get; set; }
        public string? Origin { get; set; }
        public string? UnitType { get; set; }
        public Guid? UnitId { get; set; }
        public string? UnitCode { get; set; }
        public string? UnitName { get; set; }
        public decimal? UnitRate { get; set; }
        public Guid? GroupUnitId { get; set; }
        public decimal? QuantityRequest { get; set; }
        public decimal? QuantityApproved { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? ServiceFee { get; set; }
        public string? Currency { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? PriorityLevel { get; set; }
        public string? Socode { get; set; }
        public string? Note { get; set; }
        public Guid? VendorId { get; set; }
        public string? VendorCode { get; set; }
        public string? VendorName { get; set; }
        public int? DisplayOrder { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public string? CreatedByName { get; set; }
        public int? Status { get; set; }
        public Guid? ChooseVendorId { get; set; }
        public string? ChooseVendorCode { get; set; }
        public string? ChooseVendorName { get; set; }
        public decimal? ChooseQuantity { get; set; }
        public decimal? ChoosePrice { get; set; }
        public string? ChooseCurrency { get; set; }
        public decimal? ChooseServiceFee { get; set; }
        public Guid? ChooseBy { get; set; }
        public string? ChooseByName { get; set; }
        public DateTime? ChooseByDate { get; set; }
        public int? StatusPurchase { get; set; }
        public decimal? QuantityPurchase { get; set; }
        public decimal? QuantityPurchased { get; set; }
        public Guid? PriceListDetailId { get; set; }
        public string? PriceListName { get; set; }
    }
}