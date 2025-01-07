using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class CustomerExDto
{
    public Guid Id { get; set; }
    public Guid? CustomerSourceId { get; set; }
    public string? CustomerSourceName { get; set; }
    public string? Image { get; set; }
    public int? Type { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? ZipCode { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? TaxCode { get; set; }
    public string? BusinessSector { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanySize { get; set; }
    public decimal? Capital { get; set; }
    public DateTime? EstablishedDate { get; set; }
    public string? Tags { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public string? GroupEmployeeName { get; set; }
    public bool? IsVendor { get; set; }
    public bool? IsAuto { get; set; }
    public int? Gender { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public decimal? DebtLimit { get; set; }
    public string? CustomerGroup { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public string? Representative { get; set; }
    public decimal? Revenue { get; set; }
    public decimal? RevenueMonth { get; set; }
    //extend 
    public Guid? AccountId { get; set; }
    public string? AccountEmail { get; set; }
    public bool? AccountEmailVerified { get; set; }
    public string? AccountUsername { get; set; }
    public DateTime? AccountCreatedDate { get; set; }
    public string? AccountPhone { get; set; }
    public bool? AccountPhoneVerified { get; set; }
    public string? Tenant { get; set; }
    public bool? BidActive { get; set; }
    public int? BidQuantity { get; set; }
    public bool? TranActive { get; set; }
    public string? PriceListPurchaseName { get; set; }
    public List<CustomerMappingDto>? ListGroup { get; set; }
    public string? Groups { get; set; }
    public List<CustomerMappingDto>? ListBusiness { get; set; }
    public string? Businesses { get; set; }
    public List<CustomerAddressDto>? ListAddress { get; set; }
    public List<CustomerContactDto>? ListContact { get; set; }
    public List<CustomerBankDto>? ListBank { get; set; }
}

public class CustomerAuctionDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }

}
public class BidCreditSetupDto
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public bool? BidActive { get; set; }
    public int? BidQuantity { get; set; }
    public int? OrderPending { get; set; }

}
