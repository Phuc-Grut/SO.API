using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class ContractDto
{
    public Guid? Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public Guid? ContractTypeId { get; set; }
    public string? ContractTypeCode { get; set; }
    public string? ContractTypeName { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationName { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? SignDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? Calculation { get; set; }
    public decimal? ExchangeRate { get; set; }
    public int? Status { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public string? AccountName { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? AccountId { get; set; }
    /// <summary>
    /// Điều khoản thanh toán
    /// </summary>
    public Guid? ContractTermId { get; set; }
    public string? ContractTermName { get; set; }
    public string? ContractTermContent { get; set; }
    public DateTime? ApproveDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public string? ApproveComment { get; set; }
    public DateTime? PaymentDueDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? Buyer { get; set; }
    public string? Saler { get; set; }
    public decimal? AmountLiquidation { get; set; }
    public DateTime? LiquidationDate { get; set; }
    public string? LiquidationReason { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public List<FileDto>? File { get; set; }
    public int? CreateOderStatus { get; set; }
    public bool? HasPreviousContract { get; set; }
    public decimal? Paid { get; set; }
    public decimal? Received { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public List<OrderProductDto>? OrderProduct { get; set; }
}
public class ContractParams
{
    public int? Status { get; set; }
    public DateTime? Date { get; set; }
    public Guid? CustomerId { get; set; }
    public int? IsOrder { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
public class ContractListBoxDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public DateTime? SignDate { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationCode { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public string? QuotationName { get; set; }
    public string? AccountName { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public int? TypeDiscount { get; set; }
    public double? DiscountRate { get; set; }
    public int? TypeCriteria { get; set; }
    public decimal? AmountDiscount { get; set; }
    public decimal? TotalAmountTax { get; set; }
    public int? Status { get; set; }
}
public class ContractValidateDto
{
    public UInt32 Row { get; set; }
    public bool IsValid
    {
        get
        {
            return Errors.Count == 0;
        }
    }
    public List<string> Errors { get; set; } = new List<string>();
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public Guid? UnitId { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public string? UnitPrice { get; set; }
    public string? Quantity { get; set; }
    public string? StockQuantity { get; set; }
    public string? Note { get; set; }
    public string? UnitType { get; set; }
    public string? ReasonName { get; set; }
    public Guid? ReasonId { get; set; }
    public string? TaxCategoryId { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public string? TaxRate { get; set; }
    public string? DiscountPercent { get; set; }

}
