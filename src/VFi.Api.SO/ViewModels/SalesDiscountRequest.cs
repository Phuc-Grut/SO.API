

namespace VFi.Api.SO.ViewModels;

public class SalesDiscountPagingRequest : FilterQuery
{
    public Guid? CustomerId { get; set; }
    public int? Status { get; set; }
}

public class SalesDiscountRequest
{
    public string? Code { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public Guid? CustomerAddressId { get; set; }
    public string? SalesOrderCode { get; set; }
    public Guid? SalesOrderId { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? ExchangeRate { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }
    public string? Note { get; set; }
    public int? TypeDiscount { get; set; }
    public int? Status { get; set; }
    public DateTime? DiscountDate { get; set; }
    public Guid? ApproveBy { get; set; }
    public DateTime? ApproveDate { get; set; }
    public string? ApproveByName { get; set; }
    public string? ApproveComment { get; set; }
    public string? ModuleCode { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsAuto { get; set; }
    public Boolean? IsUploadFile { get; set; }
    public List<FileRequest>? Files { get; set; }
    public List<AddSalesDiscountProductRequest>? ListDetail { get; set; }
}
public class AddSalesDiscountRequest : SalesDiscountRequest
{
}
public class EditSalesDiscountRequest : SalesDiscountRequest
{
    public Guid Id { get; set; }
}
public class ProcessSalesDiscountRequest
{
    public string Id { get; set; }
    public string? ApproveComment { get; set; }
    public int? Status { get; set; }
}
public class DuplicateSalesDiscount
{
    public Guid SalesDiscountId { get; set; }
    public string? Code { get; set; }
    public bool? IsAuto { get; set; }
    public string? ModuleCode { get; set; }
}
