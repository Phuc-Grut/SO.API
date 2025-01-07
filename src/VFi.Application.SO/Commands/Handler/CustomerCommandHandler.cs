using DocumentFormat.OpenXml.Office2010.Excel;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Utilities;

namespace VFi.Application.SO.Commands;

internal class CustomerCommandHandler : CommandHandler, IRequestHandler<CustomerAddCommand, ValidationResult>,
                                                        IRequestHandler<CustomerDeleteCommand, ValidationResult>,
                                                        IRequestHandler<CustomerEditCommand, ValidationResult>,
                                                        IRequestHandler<CustomerUpdateAccountCommand, ValidationResult>,
                                                        IRequestHandler<CustomerImageEditCommand, ValidationResult>,
                                                        IRequestHandler<ImportExcelCustomerCommand, ValidationResult>,
                                                        IRequestHandler<CustomerUpdateFinanceCommand, ValidationResult>

{
    private readonly ICustomerRepository _repository;
    private readonly ICustomerGroupMappingRepository _groupMapping;
    private readonly ICustomerBusinessMappingRepository _businessMapping;
    private readonly ICustomerAddressRepository _addressRepository;
    private readonly ICustomerContactRepository _contactRepository;
    private readonly ICustomerBankRepository _bankRepository;
    private readonly ILeadRepository _leadRepository;


    private readonly ISOContextProcedures _repositoryProcedure;
    private readonly IContextUser _context;
    public CustomerCommandHandler(
        ICustomerRepository customerRepository,
        ICustomerGroupMappingRepository groupMapping,
        ICustomerBusinessMappingRepository businessMappingRepository,
        ICustomerAddressRepository addressRepository,
        ICustomerContactRepository contactRepository,
        ICustomerBankRepository bankRepository,
        ISOContextProcedures repositoryProcedure,
        IContextUser contextUser,
        ILeadRepository leadRepository
        )
    {
        _repository = customerRepository;
        _groupMapping = groupMapping;
        _businessMapping = businessMappingRepository;
        _addressRepository = addressRepository;
        _contactRepository = contactRepository;
        _bankRepository = bankRepository;
        _repositoryProcedure = repositoryProcedure;
        _context = contextUser;
        _leadRepository = leadRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(CustomerAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var createdByName = _context.UserClaims.FullName;
        var customer = new Customer
        {
            Id = request.Id,
            CustomerSourceId = request.CustomerSourceId,
            Image = request.Image,
            Type = request.Type,
            Code = request.Code,
            Alias = request.Alias,
            Name = request.Name,
            Phone = request.Phone,
            Email = request.Email,
            Fax = request.Fax,
            Country = request.Country,
            Province = request.Province,
            District = request.District,
            Ward = request.Ward,
            ZipCode = request.ZipCode,
            Address = request.Address,
            Website = request.Website,
            TaxCode = request.TaxCode,
            //BusinessSector = request.BusinessSector,
            CompanyName = request.CompanyName,
            CompanyPhone = request.CompanyPhone,
            CompanySize = request.CompanySize,
            Capital = request.Capital,
            EstablishedDate = request.EstablishedDate,
            Tags = request.Tags,
            Note = request.Note,
            Status = request.Status,
            EmployeeId = request.EmployeeId,
            GroupEmployeeId = request.GroupEmployeeId,
            IsVendor = request.IsVendor,
            IsAuto = request.IsAuto,
            Gender = request.Gender,
            Year = request.Year,
            Month = request.Month,
            Day = request.Day,
            //CustomerGroup = request.CustomerGroup,
            Representative = request.Representative,
            Revenue = request.Revenue,
            IdName = request.IdName,
            IdNumber = request.IdNumber,
            IdDate = request.IdDate,
            IdIssuer = request.IdIssuer,
            IdImage1 = request.IdImage1,
            IdImage2 = request.IdImage2,
            IdStatus = request.IdStatus,
            CccdNumber = request.CccdNumber,
            DateRange = request.DateRange,
            Birthday = request.Birthday,
            IssuedBy = request.IssuedBy,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createdByName,
        };

        customer.AccountId = request.AccountId;
        customer.AccountUsername = request.AccountUsername;
        customer.AccountEmail = request.AccountEmail;
        customer.AccountEmailVerified = request.AccountEmailVerified;
        customer.AccountPhone = request.AccountPhone;
        customer.AccountPhoneVerified = request.AccountPhoneVerified;
        if (request.AccountId.HasValue)
        {
            customer.AccountCreatedDate = DateTime.Now;
        }
        _repository.Add(customer);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        //group
        List<CustomerGroupMapping> groupMapping = new List<CustomerGroupMapping>();
        if (!String.IsNullOrEmpty(request.CustomerGroup))
        {
            var data = request.CustomerGroup.Split(',').ToList();
            foreach (var item in data)
            {
                groupMapping.Add(new CustomerGroupMapping()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    CustomerGroupId = new Guid(item)
                });
            }
            if (groupMapping.Count > 0)
            {
                _groupMapping.Add(groupMapping);
                _ = await CommitNoCheck(_groupMapping.UnitOfWork);
            }
        }

        //business
        List<CustomerBusinessMapping> businessMappings = new List<CustomerBusinessMapping>();
        if (!String.IsNullOrEmpty(request.BusinessSector))
        {
            var data = request.BusinessSector.Split(',').ToList();
            foreach (string item in data)
            {
                businessMappings.Add(new CustomerBusinessMapping()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    BusinessId = new Guid(item)
                });
            }
            if (businessMappings.Count > 0)
            {
                _businessMapping.Add(businessMappings);
                _ = await CommitNoCheck(_businessMapping.UnitOfWork);
            }
        }

        //địa chỉ
        List<CustomerAddress> listAddress = new List<CustomerAddress>();
        if (request.ListAddress?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.ListAddress)
            {
                listAddress.Add(new CustomerAddress()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    Name = u.Name,
                    Country = u.Country,
                    Province = u.Province,
                    District = u.District,
                    Ward = u.Ward,
                    Address = u.Address,
                    Phone = u.Phone,
                    Email = u.Email,
                    ShippingDefault = u.ShippingDefault,
                    BillingDefault = u.BillingDefault,
                    Status = u.Status,
                    Note = u.Note,
                    SortOrder = i,
                    CreatedDate = createdDate,
                    CreatedBy = createdBy,
                    CreatedByName = createdByName
                });
                i++;
            }
            _addressRepository.Add(listAddress);
            _ = await CommitNoCheck(_addressRepository.UnitOfWork);
        }

        //liên hệ
        List<CustomerContact> listContact = new List<CustomerContact>();
        if (request.ListContact?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.ListContact)
            {
                listContact.Add(new CustomerContact()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    Name = u.Name,
                    Gender = u.Gender,
                    Phone = u.Phone,
                    Email = u.Email,
                    Facebook = u.Facebook,
                    JobTitle = u.JobTitle,
                    Tags = u.Tags,
                    Address = u.Address,
                    Status = u.Status,
                    SortOrder = i,
                    CreatedDate = createdDate,
                    CreatedBy = createdBy,
                    CreatedByName = createdByName
                });
                i++;
            }
            _contactRepository.Add(listContact);
            _ = await CommitNoCheck(_contactRepository.UnitOfWork);
        }

        //ngân hàng
        List<CustomerBank> listBank = new List<CustomerBank>();
        if (request.ListBank?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.ListBank)
            {
                listBank.Add(new CustomerBank()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    Name = u.Name,
                    BankCode = u.BankCode,
                    BankName = u.BankName,
                    BankBranch = u.BankBranch,
                    AccountName = u.AccountName,
                    AccountNumber = u.AccountNumber,
                    Default = u.Default,
                    Status = u.Status,
                    SortOrder = i,
                    CreatedDate = createdDate,
                    CreatedBy = createdBy,
                    CreatedByName = createdByName
                });
                i++;
            }
            _bankRepository.Add(listBank);
            _ = await CommitNoCheck(_bankRepository.UnitOfWork);
        }

        //Cập nhật mã mã khách hàng vào Lead
        if (request.LeadId is not null)
        {
            var item = await _leadRepository.GetById((Guid)request.LeadId);

            if (item is null)
            {
                return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Lead is not exist") } };
            }
            item.CustomerCode = request.Code;
            _leadRepository.Update(item);
            await Commit(_leadRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var Customer = new Customer
        {
            Id = request.Id
        };

        _repository.Remove(Customer);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var updatedByName = _context.UserClaims.FullName;
        item.CustomerSourceId = request.CustomerSourceId;
        item.Image = request.Image;
        item.Type = request.Type;
        item.Code = request.Code;
        item.Alias = request.Alias;
        item.Name = request.Name;
        item.Phone = request.Phone;
        item.Email = request.Email;
        item.Fax = request.Fax;
        item.Country = request.Country;
        item.Province = request.Province;
        item.District = request.District;
        item.Ward = request.Ward;
        item.ZipCode = request.ZipCode;
        item.Address = request.Address;
        item.Website = request.Website;
        item.TaxCode = request.TaxCode;
        //BusinessSector = request.BusinessSector,
        item.CompanyName = request.CompanyName;
        item.CompanyPhone = request.CompanyPhone;
        item.CompanySize = request.CompanySize;
        item.Capital = request.Capital;
        item.EstablishedDate = request.EstablishedDate;
        item.Tags = request.Tags;
        item.Note = request.Note;
        item.Status = request.Status;
        item.EmployeeId = request.EmployeeId;
        item.GroupEmployeeId = request.GroupEmployeeId;
        item.IsVendor = request.IsVendor;
        item.Gender = request.Gender;
        item.Year = request.Year;
        item.Month = request.Month;
        item.CurrencyId = request.CurrencyId;
        item.Currency = request.Currency;
        item.CurrencyName = request.CurrencyName;
        item.PriceListId = request.PriceListId;
        item.PriceListName = request.PriceListName;
        item.DebtLimit = request.DebtLimit;
        //CustomerGroup = request.CustomerGroup,
        item.Representative = request.Representative;
        item.Revenue = request.Revenue;
        item.IdName = request.IdName;
        item.IdNumber = request.IdNumber;
        item.IdDate = request.IdDate;
        item.IdIssuer = request.IdIssuer;
        item.IdImage1 = request.IdImage1;
        item.IdImage2 = request.IdImage2;
        item.IdStatus = request.IdStatus;
        item.CccdNumber = request.CccdNumber;
        item.DateRange = request.DateRange;
        item.Birthday = request.Birthday;
        item.IssuedBy = request.IssuedBy;
        item.UpdatedBy = updatedBy;
        item.UpdatedDate = updatedDate;
        item.UpdatedByName = updatedByName;

        item.AccountId = request.AccountId;
        item.AccountEmail = request.AccountEmail;
        item.AccountEmailVerified = request.AccountEmailVerified;
        item.AccountUsername = request.AccountUsername;
        item.AccountCreatedDate = request.AccountCreatedDate;
        item.AccountPhone = request.AccountPhone;
        item.AccountPhoneVerified = request.AccountPhoneVerified;

        _repository.Update(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;
        //groups
        var dataGroup = await _groupMapping.Filter(request.Id);
        _groupMapping.Remove(dataGroup);
        List<CustomerGroupMapping> groupMapping = new List<CustomerGroupMapping>();
        if (!String.IsNullOrEmpty(request.CustomerGroup))
        {
            var groups = request.CustomerGroup.Split(',').ToList();
            foreach (string i in groups)
            {
                groupMapping.Add(new CustomerGroupMapping()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    CustomerGroupId = new Guid(i)
                });
            }
            if (groupMapping.Count > 0)
            {
                _groupMapping.Add(groupMapping);
            }
        }
        _ = await CommitNoCheck(_groupMapping.UnitOfWork);

        //business
        var dataBusiness = await _businessMapping.Filter(request.Id);
        _businessMapping.Remove(dataBusiness);

        List<CustomerBusinessMapping> businessMappings = new List<CustomerBusinessMapping>();
        if (!String.IsNullOrEmpty(request.BusinessSector))
        {
            var data = request.BusinessSector.Split(',').ToList();
            foreach (string i in data)
            {
                businessMappings.Add(new CustomerBusinessMapping()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.Id,
                    BusinessId = new Guid(i)
                });
            }
            if (businessMappings.Count > 0)
            {
                _businessMapping.Add(businessMappings);
            }
        }
        _ = await CommitNoCheck(_businessMapping.UnitOfWork);

        //customerAddress
        List<CustomerAddress> listAddressUpdate = new List<CustomerAddress>();
        List<CustomerAddress> listAddressDelete = new List<CustomerAddress>();
        if (request.ListAddress.Any())
        {
            foreach (var address in item.CustomerAddress)
            {
                var u = request.ListAddress.Where(x => x.Id == address.Id).FirstOrDefault();
                if (u != null)
                {
                    address.Name = u.Name;
                    address.Country = u.Country;
                    address.Province = u.Province;
                    address.District = u.District;
                    address.Ward = u.Ward;
                    address.Address = u.Address;
                    address.Phone = u.Phone;
                    address.Email = u.Email;
                    address.ShippingDefault = u.ShippingDefault;
                    address.BillingDefault = u.BillingDefault;
                    address.Note = u.Note;
                    address.Status = u.Status;
                    address.SortOrder = u.SortOrder;
                    address.UpdatedBy = updatedBy;
                    address.UpdatedDate = updatedDate;
                    address.UpdatedByName = updatedByName;
                    listAddressUpdate.Add(address);
                    request.ListAddress.Remove(u);
                }
                else
                {
                    listAddressDelete.Add(address);
                }
            }
            List<CustomerAddress> listAddressAdd = new List<CustomerAddress>();
            for (int i = 0; i < request.ListAddress.Count; i++)
            {
                var u = request.ListAddress[i];
                var address = item.CustomerAddress.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (address is null)
                {
                    listAddressAdd.Add(new CustomerAddress()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = request.Id,
                        Name = u.Name,
                        Country = u.Country,
                        Province = u.Province,
                        District = u.District,
                        Ward = u.Ward,
                        Address = u.Address,
                        Phone = u.Phone,
                        Email = u.Email,
                        ShippingDefault = u.ShippingDefault,
                        BillingDefault = u.BillingDefault,
                        Status = u.Status,
                        Note = u.Note,
                        SortOrder = u.SortOrder,
                        CreatedDate = updatedDate,
                        CreatedBy = updatedBy,
                        CreatedByName = updatedByName
                    });

                    request.ListAddress.Remove(u);
                    i--;
                }
            }
            _addressRepository.Update(listAddressUpdate);
            _addressRepository.Add(listAddressAdd);
            _addressRepository.Remove(listAddressDelete);
            _ = await CommitNoCheck(_addressRepository.UnitOfWork);
        }
        else if (item.CustomerAddress.Any())
        {
            foreach (var address in item.CustomerAddress)
            {
                listAddressDelete.Add(address);
            }
            _addressRepository.Remove(listAddressDelete);
            _ = await CommitNoCheck(_addressRepository.UnitOfWork);
        }

        //customerContact
        List<CustomerContact> listContactUpdate = new List<CustomerContact>();
        List<CustomerContact> listContactDelete = new List<CustomerContact>();
        if (request.ListContact.Any())
        {
            foreach (var contact in item.CustomerContact)
            {
                var u = request.ListContact.Where(x => x.Id == contact.Id).FirstOrDefault();
                if (u != null)
                {
                    contact.Name = u.Name;
                    contact.Gender = u.Gender;
                    contact.Phone = u.Phone;
                    contact.Email = u.Email;
                    contact.Facebook = u.Facebook;
                    contact.Facebook = u.Facebook;
                    contact.JobTitle = u.JobTitle;
                    contact.Tags = u.Tags;
                    contact.Address = u.Address;
                    contact.Status = u.Status;
                    contact.SortOrder = u.SortOrder;
                    contact.UpdatedBy = updatedBy;
                    contact.UpdatedDate = updatedDate;
                    contact.UpdatedByName = updatedByName;
                    listContactUpdate.Add(contact);
                    request.ListContact.Remove(u);
                }
                else
                {
                    listContactDelete.Add(contact);
                }
            }
            List<CustomerContact> listContactAdd = new List<CustomerContact>();
            for (int i = 0; i < request.ListContact.Count; i++)
            {
                var u = request.ListContact[i];
                var contact = item.CustomerContact.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (contact is null)
                {
                    listContactAdd.Add(new CustomerContact()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = request.Id,
                        Name = u.Name,
                        Gender = u.Gender,
                        Phone = u.Phone,
                        Email = u.Email,
                        Facebook = u.Facebook,
                        JobTitle = u.JobTitle,
                        Tags = u.Tags,
                        Address = u.Address,
                        Status = u.Status,
                        SortOrder = u.SortOrder,
                        CreatedDate = updatedDate,
                        CreatedBy = updatedBy,
                        CreatedByName = updatedByName
                    });

                    request.ListContact.Remove(u);
                    i--;
                }
            }
            _contactRepository.Update(listContactUpdate);
            _contactRepository.Add(listContactAdd);
            _contactRepository.Remove(listContactDelete);
            _ = await CommitNoCheck(_contactRepository.UnitOfWork);
        }
        else if (item.CustomerContact.Any())
        {
            foreach (var contact in item.CustomerContact)
            {
                listContactDelete.Add(contact);
            }
            _contactRepository.Remove(listContactDelete);
            _ = await CommitNoCheck(_contactRepository.UnitOfWork);
        }

        //customerBank
        List<CustomerBank> listBankUpdate = new List<CustomerBank>();
        List<CustomerBank> listBankDelete = new List<CustomerBank>();
        if (request.ListBank.Any())
        {
            foreach (var bank in item.CustomerBank)
            {
                var u = request.ListBank.Where(x => x.Id == bank.Id).FirstOrDefault();
                if (u != null)
                {
                    bank.Name = u.Name;
                    bank.BankCode = u.BankCode;
                    bank.BankName = u.BankName;
                    bank.BankBranch = u.BankBranch;
                    bank.AccountName = u.AccountName;
                    bank.AccountNumber = u.AccountNumber;
                    bank.Default = u.Default;
                    bank.Status = u.Status;
                    bank.SortOrder = u.SortOrder;
                    bank.UpdatedBy = updatedBy;
                    bank.UpdatedDate = updatedDate;
                    bank.UpdatedByName = updatedByName;
                    listBankUpdate.Add(bank);
                    request.ListBank.Remove(u);
                }
                else
                {
                    listBankDelete.Add(bank);
                }
            }
            List<CustomerBank> listBankAdd = new List<CustomerBank>();
            for (int i = 0; i < request.ListBank.Count; i++)
            {
                var u = request.ListBank[i];
                var bank = item.CustomerBank.FirstOrDefault(x => x.Id.Equals(u.Id));
                if (bank is null)
                {
                    listBankAdd.Add(new CustomerBank()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = request.Id,
                        Name = u.Name,
                        BankCode = u.BankCode,
                        BankName = u.BankName,
                        BankBranch = u.BankBranch,
                        AccountName = u.AccountName,
                        AccountNumber = u.AccountNumber,
                        Default = u.Default,
                        Status = u.Status,
                        SortOrder = u.SortOrder,
                        CreatedDate = updatedDate,
                        CreatedBy = updatedBy,
                        CreatedByName = updatedByName
                    });

                    request.ListBank.Remove(u);
                    i--;
                }
            }
            _bankRepository.Update(listBankAdd);
            _bankRepository.Add(listBankAdd);
            _bankRepository.Remove(listBankDelete);
            _ = await CommitNoCheck(_bankRepository.UnitOfWork);
        }
        else if (item.CustomerBank.Any())
        {
            foreach (var bank in item.CustomerBank)
            {
                listBankDelete.Add(bank);
            }
            _bankRepository.Remove(listBankDelete);
            _ = await CommitNoCheck(_bankRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(CustomerUpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }

        item.AccountId = request.AccountId;
        item.AccountEmail = request.AccountEmail;
        item.AccountEmailVerified = request.AccountEmailVerified;
        item.AccountUsername = request.AccountUsername;
        item.AccountCreatedDate = request.AccountCreatedDate;
        item.AccountPhone = request.AccountPhone;
        item.AccountPhoneVerified = request.AccountPhoneVerified;

        _repository.UpdateAccount(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerImageEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }
        item.Image = request.Image;
        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CustomerUpdateFinanceCommand cmd, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(cmd.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Customer is not exist") } };
        }

        var obj = new Customer
        {
            Id = cmd.Id,
            CurrencyId = cmd.CurrencyId,
            Currency = cmd.Currency,
            CurrencyName = cmd.CurrencyName,
            PriceListId = cmd.PriceListId,
            PriceListName = cmd.PriceListName,
            DebtLimit = cmd.DebtLimit
        };
        _repository.UpdateFinance(obj);
        return await Commit(_repository.UnitOfWork);

    }

    public async Task<ValidationResult> Handle(ImportExcelCustomerCommand request, CancellationToken cancellationToken)
    {
        var data = request.customerImportDtos.Select(x =>
         {
             var id = Guid.NewGuid();
             var addData = new Customer()
             {
                 Id = id,
                 Code = x.Code,
                 Name = x.Name,
                 IsVendor = x.IsVendor == 1 ? true : false,
                 Type = x.Type,
                 Year = x.Year,
                 Month = x.Month,
                 Day = x.Day,
                 Gender = x.Gender,
                 Phone = x.Phone,
                 Email = x.Email,
                 TaxCode = x.TaxCode,
                 ZipCode = x.ZipCode,
                 Fax = x.Fax,
                 Website = x.Website,
                 BusinessSector = x.BusinessSector,
                 CompanySize = x.CompanySize,
                 Capital = x.Capital,
                 Country = x.Country,
                 Province = x.Province,
                 District = x.District,
                 Ward = x.Ward,
                 Address = x.Address,
                 IdNumber = x.IdNumber,
                 IdDate = x.IdDate,
                 IdIssuer = x.IdIssuer,
                 EmployeeId = x.EmployeeId,
                 CustomerSourceId = x.CustomerSourceId,
                 GroupEmployeeId = x.GroupEmployeeId,
                 Note = x.Note,
                 Status = x.Status
             };

             foreach (var item in request.customerImportDtos)
             {
                 if (!String.IsNullOrEmpty(item.CustomerGroupId))
                 {
                     var dataCG = item.CustomerGroupId.Split(',').ToList();
                     foreach (var item1 in dataCG)
                     {
                         addData.CustomerGroupMapping.Add(new CustomerGroupMapping()
                         {
                             Id = Guid.NewGuid(),
                             CustomerId = id,
                             CustomerGroupId = new Guid(item1)
                         });
                     }
                 }
             }
             return addData;
         });
        _repository.Add(data);
        return await Commit(_repository.UnitOfWork);
    }
}
