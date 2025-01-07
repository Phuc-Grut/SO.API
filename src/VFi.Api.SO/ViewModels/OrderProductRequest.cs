using VFi.Domain.SO.Models;

namespace VFi.Api.SO.ViewModels;

public class AddOrderProductRequest
{
    public string? Id { get; set; }
    public Guid? OrderId { get; set; }
    public string? OrderCode { get; set; }
    public Guid? ContractId { get; set; }
    public string? ContractName { get; set; }
    public Guid? QuotationId { get; set; }
    public string? QuotationName { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string? ProductImage { get; set; }
    public string? Origin { get; set; }
    public Guid? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? WarehouseName { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? DiscountAmountDistribution { get; set; }
    public int? DiscountType { get; set; }
    public double? DiscountPercent { get; set; }
    public decimal? AmountDiscount { get; set; }
    public double? DiscountTotal { get; set; }
    public double? TaxRate { get; set; }
    public string? Tax { get; set; }
    public string? TaxCode { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string? Note { get; set; }
    public int? DisplayOrder { get; set; }
    public int? DeliveryStatus { get; set; }
    public int? DeliveryQuantity { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public string? SpecificationCode1 { get; set; }
    public string? SpecificationCode2 { get; set; }
    public string? SpecificationCode3 { get; set; }
    public string? SpecificationCode4 { get; set; }
    public string? SpecificationCode5 { get; set; }
    public string? SpecificationCode6 { get; set; }
    public string? SpecificationCode7 { get; set; }
    public string? SpecificationCode8 { get; set; }
    public string? SpecificationCode9 { get; set; }
    public string? SpecificationCode10 { get; set; }
    public string? SpecificationCodeJson { get; set; }
    public Guid? Guid { get; set; }
    public string? BidUsername { get; set; }
    public string? SourceCode {  get; set; }
    public string? SourceLink { get; set; }
}
public class EditOrderProductRequest : AddOrderProductRequest
{
    public Guid Id { get; set; }
}

public class DeleteOrderProductRequest
{
    public Guid? Id { get; set; }
}

public class ImportExcelOrderProductRequest
{
    public IFormFile File { get; set; } = null!;
    public string SheetId { get; set; } = null!;
    public int HeaderRow { get; set; }

    public int ProductName { get; set; }
    public int UnitCode { get; set; }
    public int ProductCode { get; set; }
    public int UnitName { get; set; }
    public int ProductPrice { get; set; }
    public int TranferRate { get; set; }
    public int ProductId { get; set; }
    public int ProductImage { get; set; }
    public int ProductRate { get; set; }
    public int AllocationRate { get; set; }
    public int DiscountAmountDistribution { get; set; }
    public int AmountNoTaxQD { get; set; }
    public int AmountNoDiscountQD { get; set; }
    public int AmountNoTax { get; set; }
    public int DiscountPercent { get; set; }
    public int DiscountType { get; set; }
    public int TotalAmountDiscountQD { get; set; }
    public int TotalAmountTaxQD { get; set; }
    public int Tax { get; set; }
    public int AmountDiscount { get; set; }
    public int AmountNoDiscount { get; set; }
    public int AmountTax { get; set; }
    public int DeliveryDate { get; set; }
    public int Quantity { get; set; }
    public int TotalAmountDiscount { get; set; }
    public int TotalAmountTax { get; set; }
    public int UnitPrice { get; set; }
}
