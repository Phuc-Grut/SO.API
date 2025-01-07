using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class CustomerDto
{
    public Guid Id { get; set; }
    public Guid? CustomerSourceId { get; set; }
    public string? CustomerSourceName { get; set; }
    public string? Image { get; set; }
    public int? Type { get; set; }
    public string? Code { get; set; }
    public string? TypeName { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Fax { get; set; }
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
    public string? StatusString { get; set; }
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
    public int? STT { get; set; }
    public string? IdName { get; set; }
    public string? IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string? IdIssuer { get; set; }
    public string? IdImage1 { get; set; }
    public string? IdImage2 { get; set; }
    public int? IdStatus { get; set; }
    public string? CccdNumber { get; set; }
    public DateTime? DateRange { get; set; }
    public DateTime? Birthday { get; set; }
    public string? IssuedBy { get; set; }
    public int? BmapGroup { get; set; }
    public List<CustomerMappingDto>? ListGroup { get; set; }
    public string? Groups { get; set; }
    public List<CustomerMappingDto>? ListBusiness { get; set; }
    public string? Businesses { get; set; }
    public List<CustomerAddressDto>? ListAddress { get; set; }
    public List<CustomerContactDto>? ListContact { get; set; }
    public List<CustomerBankDto>? ListBank { get; set; }
}
public class CustomerCbxDto
{
    public Guid Value { get; set; }
    public string? Label { get; set; }
    public string? Key { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? ZipCode { get; set; }
    public string? TaxCode { get; set; }
    public Guid? PriceListId { get; set; }
    public decimal? DebtLimit { get; set; }

}
public class CustomerMappingDto
{
    public Guid? Value { get; set; }
}
public class CustomerSimpleDto
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
    public decimal? Revenue { get; set; }
    public decimal? RevenueMonth { get; set; }
    public int? BmapGroup { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    //extend 
    public Guid? AccountId { get; set; }
    public string AccountEmail { get; set; }
    public string AccountUsername { get; set; }
    public DateTime? AccountCreatedDate { get; set; }
    public string AccountPhone { get; set; }
    public bool? AccountPhoneVerified { get; set; }
    public bool? AccountEmailVerified { get; set; }
    public string Tenant { get; set; }
    public bool? BidActive { get; set; }
    public int? BidQuantity { get; set; }
    public bool? TranActive { get; set; }


    public string IdName { get; set; }
    public string IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string IdIssuer { get; set; }
    public string IdImage1 { get; set; }
    public string IdImage2 { get; set; }
    public int? IdStatus { get; set; }

    public string? Groups { get; set; }
    public string? Businesses { get; set; }
}


public class CustomerPricePuchaseDto
{
    public Guid Id { get; set; }
    public Guid PriceListPurchaseId { get; set; }
    public string PriceListPurchase { get; set; }
    public string PurchaseGroupCode { get; set; }
    public int? BuyFee { get; set; }
    public decimal? BuyFeeMin { get; set; }
    public decimal? BuyFeeFix { get; set; }
    public string Currency { get; set; }
    public decimal? CalculatePrice { get; set; }
}
public class CustomerParams
{
    public int? Status { get; set; }
    public int? Type { get; set; }
    public string? CustomerGroupId { get; set; }
    public string? EmployeeId { get; set; }
    public string? GroupEmployeeId { get; set; }
}

public class CustomerValidateDto
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
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? IsVendor { get; set; }
    public int? Type { get; set; }
    public string? Year { get; set; }
    public string? Month { get; set; }
    public string? Day { get; set; }
    public int? Gender { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }

    public string? TaxCode { get; set; }
    public string? ZipCode { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string? BusinessSector { get; set; }

    public string? BusinessSectorId { get; set; }
    public string? CompanySize { get; set; }
    public string? Capital { get; set; }
    public string? Country { get; set; }
    public Guid? CountryId { get; set; }
    public string? Province { get; set; }
    public Guid? StateProvinceId { get; set; }
    public string? District { get; set; }
    public Guid? DistrictId { get; set; }
    public string? Ward { get; set; }
    public Guid? WardId { get; set; }
    public string? Address { get; set; }
    public string? IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string? IdIssuer { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? CustomerGroup { get; set; }
    public Guid? CustomerGroupId { get; set; }
    public List<string>? CustomerGroupNames { get; set; } = new List<string?> { };
    public List<Guid?> CustomerGroupIds { get; set; } = new List<Guid?> { };
    public Guid? SalesGroupId { get; set; }
    public string? SalesGroup { get; set; }
    public string? CustomerSource { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public Guid? CustomerSourceId { get; set; }
    public string? Note { get; set; }
    public int? BmapGroup { get; set; }
}

public class CustomerImportDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? IsVendor { get; set; }
    public int? Type { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Gender { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }

    public string? TaxCode { get; set; }
    public string? ZipCode { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string? BusinessSector { get; set; }
    public string? BusinessSectorId { get; set; }
    public string? CompanySize { get; set; }
    public decimal? Capital { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? District { get; set; }
    public string? Ward { get; set; }
    public string? Address { get; set; }
    public string? IdNumber { get; set; }
    public DateTime? IdDate { get; set; }
    public string? IdIssuer { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? CustomerGroup { get; set; }
    public string? CustomerGroupId { get; set; }
    public Guid? SalesGroupId { get; set; }
    public string? SalesGroup { get; set; }
    public string? CustomerSource { get; set; }
    public string? GroupEmployeeName { get; set; }
    public Guid? GroupEmployeeId { get; set; }
    public Guid? CustomerSourceId { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedName { get; set; }
}
public class DataFormat
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? DistrictId { get; set; }
    public Guid? StateProvinceId { get; set; }
}
