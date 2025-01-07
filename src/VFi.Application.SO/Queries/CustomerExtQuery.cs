using VFi.Application.SO.DTOs;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CustomerIdQueryByAccountId : IQuery<Guid>
{
    public CustomerIdQueryByAccountId()
    {
    }

    public CustomerIdQueryByAccountId(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; set; }
}
public class CustomerQueryByAccountId : IQuery<CustomerSimpleDto>
{
    public CustomerQueryByAccountId()
    {
    }

    public CustomerQueryByAccountId(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; set; }
}

public class CustomerQueryByAccountIds : IQuery<IList<CustomerSimpleDto>?>
{
    public CustomerQueryByAccountIds(Guid[] accountIds)
    {
        AccountIds = accountIds;
    }

    public Guid[] AccountIds { get; set; }
}
public class CustomerCheckExistAccountId : IQuery<bool>
{
    public CustomerCheckExistAccountId()
    {
    }

    public CustomerCheckExistAccountId(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; set; }
}
public class CustomerQueryByAccountEmail : IQuery<CustomerSimpleDto>
{
    public CustomerQueryByAccountEmail()
    {
    }

    public CustomerQueryByAccountEmail(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}
public class CustomerQueryByAccountUsername : IQuery<CustomerSimpleDto>
{
    public CustomerQueryByAccountUsername()
    {
    }

    public CustomerQueryByAccountUsername(string username)
    {
        Username = username;
    }

    public string Username { get; set; }
}
public partial class CustomerQueryHandler :
                                         IQueryHandler<CustomerQueryByAccountId, CustomerSimpleDto>,
                                         IQueryHandler<CustomerCheckExistAccountId, bool>,
                                         IQueryHandler<CustomerQueryByAccountEmail, CustomerSimpleDto>,
                                         IQueryHandler<CustomerQueryByAccountUsername, CustomerSimpleDto>,
                                         IQueryHandler<CustomerIdQueryByAccountId, Guid>,
                                         IQueryHandler<CustomerQueryByAccountIds, IList<CustomerSimpleDto>>
{

    public async Task<CustomerSimpleDto> Handle(CustomerQueryByAccountId request, CancellationToken cancellationToken)
    {
        var obj = await _customerRepository.GetByAccountId(request.AccountId);
        if (obj is { })
        {
            var result = new CustomerSimpleDto()
            {
                Id = obj.Id,
                CustomerSourceId = obj.CustomerSourceId,
                Image = obj.Image,
                Type = obj.Type,
                Code = obj.Code,
                Name = obj.Name,
                Phone = obj.Phone,
                Email = obj.Email,
                Country = obj.Country,
                Province = obj.Province,
                District = obj.District,
                Ward = obj.Ward,
                ZipCode = obj.ZipCode,
                Address = obj.Address,
                Website = obj.Website,
                TaxCode = obj.TaxCode,
                BusinessSector = obj.BusinessSector,
                CompanyName = obj.CompanyName,
                CompanyPhone = obj.CompanyPhone,
                CompanySize = obj.CompanySize,
                Capital = obj.Capital,
                EstablishedDate = obj.EstablishedDate,
                Tags = obj.Tags,
                Note = obj.Note,
                Status = obj.Status,
                EmployeeId = obj.EmployeeId,
                GroupEmployeeId = obj.GroupEmployeeId,
                IsVendor = obj.IsVendor,
                IsAuto = obj.IsAuto,
                Gender = obj.Gender,
                Year = obj.Year,
                Month = obj.Month,
                Day = obj.Day,
                CurrencyId = obj.CurrencyId,
                CurrencyName = obj.CurrencyName,
                PriceListId = obj.PriceListId,
                DebtLimit = obj.DebtLimit,
                CreatedBy = obj.CreatedBy,
                CreatedDate = obj.CreatedDate,
                UpdatedBy = obj.UpdatedBy,
                UpdatedDate = obj.UpdatedDate,
                AccountId = obj.AccountId,
                AccountEmail = obj.AccountEmail,
                AccountUsername = obj.AccountUsername,
                AccountCreatedDate = obj.AccountCreatedDate,
                AccountPhone = obj.AccountPhone,
                AccountPhoneVerified = obj.AccountPhoneVerified,
                AccountEmailVerified = obj.AccountEmailVerified,
                Tenant = obj.Tenant,
                BidActive = obj.BidActive,
                BidQuantity = obj.BidQuantity,
                TranActive = obj.TranActive,
                IdName = obj.IdName,
                IdNumber = obj.IdNumber,
                IdDate = obj.IdDate,
                IdIssuer = obj.IdIssuer,
                IdImage1 = obj.IdImage1,
                IdImage2 = obj.IdImage2,
                IdStatus = obj.IdStatus,
                BmapGroup = obj.BmapGroup
            };
            return result;
        }
        return null;
    }
    public async Task<Guid> Handle(CustomerIdQueryByAccountId request, CancellationToken cancellationToken)
    {
        var obj = await _customerRepository.GetIdByAccountId(request.AccountId);
        return obj;
    }
    public async Task<bool> Handle(CustomerCheckExistAccountId request, CancellationToken cancellationToken)
    {
        var obj = await _customerRepository.GetByAccountId(request.AccountId);
        return obj is { };
    }
    public async Task<CustomerSimpleDto> Handle(CustomerQueryByAccountEmail request, CancellationToken cancellationToken)
    {
        var obj = await _customerRepository.GetByAccountEmail(request.Email);
        var result = new CustomerSimpleDto()
        {
            Id = obj.Id,
            CustomerSourceId = obj.CustomerSourceId,
            Image = obj.Image,
            Type = obj.Type,
            Code = obj.Code,
            Name = obj.Name,
            Phone = obj.Phone,
            Email = obj.Email,
            Country = obj.Country,
            Province = obj.Province,
            District = obj.District,
            Ward = obj.Ward,
            ZipCode = obj.ZipCode,
            Address = obj.Address,
            Website = obj.Website,
            TaxCode = obj.TaxCode,
            BusinessSector = obj.BusinessSector,
            CompanyName = obj.CompanyName,
            CompanyPhone = obj.CompanyPhone,
            CompanySize = obj.CompanySize,
            Capital = obj.Capital,
            EstablishedDate = obj.EstablishedDate,
            Tags = obj.Tags,
            Note = obj.Note,
            Status = obj.Status,
            EmployeeId = obj.EmployeeId,
            GroupEmployeeId = obj.GroupEmployeeId,
            IsVendor = obj.IsVendor,
            IsAuto = obj.IsAuto,
            Gender = obj.Gender,
            Year = obj.Year,
            Month = obj.Month,
            Day = obj.Day,
            CurrencyId = obj.CurrencyId,
            CurrencyName = obj.CurrencyName,
            PriceListId = obj.PriceListId,
            DebtLimit = obj.DebtLimit,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            AccountId = obj.AccountId,
            AccountEmail = obj.AccountEmail,
            AccountUsername = obj.AccountUsername,
            AccountCreatedDate = obj.AccountCreatedDate,
            AccountPhone = obj.AccountPhone,
            AccountPhoneVerified = obj.AccountPhoneVerified,
            AccountEmailVerified = obj.AccountEmailVerified,
            Tenant = obj.Tenant,
            BidActive = obj.BidActive,
            BidQuantity = obj.BidQuantity,
            TranActive = obj.TranActive,
            BmapGroup = obj.BmapGroup
        };
        return result;
    }
    public async Task<CustomerSimpleDto> Handle(CustomerQueryByAccountUsername request, CancellationToken cancellationToken)
    {
        var obj = await _customerRepository.GetByAccountUsername(request.Username);
        var result = new CustomerSimpleDto()
        {
            Id = obj.Id,
            CustomerSourceId = obj.CustomerSourceId,
            Image = obj.Image,
            Type = obj.Type,
            Code = obj.Code,
            Name = obj.Name,
            Phone = obj.Phone,
            Email = obj.Email,
            Country = obj.Country,
            Province = obj.Province,
            District = obj.District,
            Ward = obj.Ward,
            ZipCode = obj.ZipCode,
            Address = obj.Address,
            Website = obj.Website,
            TaxCode = obj.TaxCode,
            BusinessSector = obj.BusinessSector,
            CompanyName = obj.CompanyName,
            CompanyPhone = obj.CompanyPhone,
            CompanySize = obj.CompanySize,
            Capital = obj.Capital,
            EstablishedDate = obj.EstablishedDate,
            Tags = obj.Tags,
            Note = obj.Note,
            Status = obj.Status,
            EmployeeId = obj.EmployeeId,
            GroupEmployeeId = obj.GroupEmployeeId,
            IsVendor = obj.IsVendor,
            IsAuto = obj.IsAuto,
            Gender = obj.Gender,
            Year = obj.Year,
            Month = obj.Month,
            Day = obj.Day,
            CurrencyId = obj.CurrencyId,
            CurrencyName = obj.CurrencyName,
            PriceListId = obj.PriceListId,
            DebtLimit = obj.DebtLimit,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            AccountId = obj.AccountId,
            AccountEmail = obj.AccountEmail,
            AccountUsername = obj.AccountUsername,
            AccountCreatedDate = obj.AccountCreatedDate,
            AccountPhone = obj.AccountPhone,
            AccountPhoneVerified = obj.AccountPhoneVerified,
            AccountEmailVerified = obj.AccountEmailVerified,
            Tenant = obj.Tenant,
            BidActive = obj.BidActive,
            BidQuantity = obj.BidQuantity,
            TranActive = obj.TranActive,
            BmapGroup = obj.BmapGroup
        };
        return result;
    }

    public async Task<IList<CustomerSimpleDto>?> Handle(CustomerQueryByAccountIds request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var customers = await _customerRepository.GetByAccountIds(request.AccountIds);
        var result = customers?.Select(customer => new CustomerSimpleDto()
        {
            Id = customer.Id,
            CustomerSourceId = customer.CustomerSourceId,
            Image = customer.Image,
            Type = customer.Type,
            Code = customer.Code,
            Name = customer.Name,
            Phone = customer.Phone,
            Email = customer.Email,
            Country = customer.Country,
            Province = customer.Province,
            District = customer.District,
            Ward = customer.Ward,
            ZipCode = customer.ZipCode,
            Address = customer.Address,
            Website = customer.Website,
            TaxCode = customer.TaxCode,
            BusinessSector = customer.BusinessSector,
            CompanyName = customer.CompanyName,
            CompanyPhone = customer.CompanyPhone,
            CompanySize = customer.CompanySize,
            Capital = customer.Capital,
            EstablishedDate = customer.EstablishedDate,
            Tags = customer.Tags,
            Note = customer.Note,
            Status = customer.Status,
            EmployeeId = customer.EmployeeId,
            GroupEmployeeId = customer.GroupEmployeeId,
            IsVendor = customer.IsVendor,
            IsAuto = customer.IsAuto,
            Gender = customer.Gender,
            Year = customer.Year,
            Month = customer.Month,
            Day = customer.Day,
            CurrencyId = customer.CurrencyId,
            CurrencyName = customer.CurrencyName,
            PriceListId = customer.PriceListId,
            DebtLimit = customer.DebtLimit,
            CreatedBy = customer.CreatedBy,
            CreatedDate = customer.CreatedDate,
            UpdatedBy = customer.UpdatedBy,
            UpdatedDate = customer.UpdatedDate,
            AccountId = customer.AccountId,
            AccountEmail = customer.AccountEmail,
            AccountUsername = customer.AccountUsername,
            AccountCreatedDate = customer.AccountCreatedDate,
            AccountPhone = customer.AccountPhone,
            AccountPhoneVerified = customer.AccountPhoneVerified,
            AccountEmailVerified = customer.AccountEmailVerified,
            Tenant = customer.Tenant,
            BidActive = customer.BidActive,
            BidQuantity = customer.BidQuantity,
            TranActive = customer.TranActive,
            IdName = customer.IdName,
            IdNumber = customer.IdNumber,
            IdDate = customer.IdDate,
            IdIssuer = customer.IdIssuer,
            IdImage1 = customer.IdImage1,
            IdImage2 = customer.IdImage2,
            IdStatus = customer.IdStatus,
            BmapGroup = customer.BmapGroup
        });
        return result?.ToList();
    }
}
