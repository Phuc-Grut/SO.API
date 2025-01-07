using System.Collections.Generic;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Application.SO.Commands;

internal class EmployeeCommandHandler : CommandHandler, IRequestHandler<EmployeeAddCommand, ValidationResult>, IRequestHandler<EmployeeDeleteCommand, ValidationResult>, IRequestHandler<EmployeeEditCommand, ValidationResult>
{
    private readonly IEmployeeRepository _repository;
    private readonly IGroupEmployeeMappingRepository _groupEmpRepository;
    private readonly IContextUser _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerAddressRepository _addressRepository;
    private readonly ICustomerContactRepository _contactRepository;
    private readonly ICustomerBankRepository _bankRepository;
    private readonly IRequestQuoteRepository _requestQuoteRepository;
    private readonly IProductionOrderRepository _productionOrderRepository;
    private readonly IQuotationRepository _quotationRepository;
    private readonly IContractRepository _contractRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentInvoiceRepository _paymentInvoiceRepository;
    private readonly IRequestPurchaseRepository _requestPurchaseRepository;
    private readonly IReturnOrderRepository _returnOrderRepository;
    private readonly IExportWarehouseRepository _exportWarehouseRepository;

    public EmployeeCommandHandler(IEmployeeRepository employeeRepository,
                                  IGroupEmployeeMappingRepository groupEmpRepository,
                                  IContextUser contextUser,
                                  ICustomerRepository customerRepository,
                                  ICustomerAddressRepository addressRepository,
                                  ICustomerContactRepository contactRepository,
                                  ICustomerBankRepository bankRepository,
                                  IRequestQuoteRepository requestQuoteRepository,
                                  IProductionOrderRepository productionOrderRepository,
                                  IQuotationRepository quotationRepository,
                                  IContractRepository contractRepository,
                                  IOrderRepository orderRepository,
                                  IPaymentInvoiceRepository paymentInvoiceRepository,
                                  IRequestPurchaseRepository requestPurchaseRepository,
                                  IReturnOrderRepository returnOrderRepository,
                                  IExportWarehouseRepository exportWarehouseRepository)
    {
        _repository = employeeRepository;
        _groupEmpRepository = groupEmpRepository;
        _context = contextUser;
        _customerRepository = customerRepository;
        _addressRepository = addressRepository;
        _contactRepository = contactRepository;
        _bankRepository = bankRepository;
        _requestQuoteRepository = requestQuoteRepository;
        _productionOrderRepository = productionOrderRepository;
        _quotationRepository = quotationRepository;
        _contractRepository = contractRepository;
        _orderRepository = orderRepository;
        _paymentInvoiceRepository = paymentInvoiceRepository;
        _requestPurchaseRepository = requestPurchaseRepository;
        _returnOrderRepository = returnOrderRepository;
        _exportWarehouseRepository = exportWarehouseRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(EmployeeAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Employee
        {
            Id = request.Id,
            IsCustomer = request.IsCustomer,
            Code = request.Code,
            AccountId = request.AccountId,
            AccountName = request.AccountName,
            Email = request.Email,
            Name = request.Name,
            Image = request.Image,
            Phone = request.Phone,
            Country = request.Country,
            Province = request.Province,
            District = request.District,
            Ward = request.Ward,
            Address = request.Address,
            Gender = request.Gender,
            Year = request.Year,
            Month = request.Month,
            Day = request.Day,
            TaxCode = request.TaxCode,
            GroupEmployee = request.GroupEmployee,
            Status = request.Status,
            Description = request.Description,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName,
            CustomerAddresses = request.ListAddress?.Select(x => new CustomerAddress()
            {
                Id = (Guid)x.Id,
                EmployeeId = request.Id,
                Name = x.Name,
                Country = x.Country,
                Province = x.Province,
                District = x.District,
                Ward = x.Ward,
                Address = x.Address,
                BillingDefault = x.BillingDefault,
                ShippingDefault = x.ShippingDefault,
                Email = x.Email,
                Phone = x.Phone,
                //ZipCode = x.ZipCode,
                Note = x.Note,
                SortOrder = x.SortOrder,
                Status = x.Status ?? 1
            }).ToList(),
            CustomerBanks = request.ListBank?.Select(x => new CustomerBank()
            {
                Id = (Guid)x.Id,
                EmployeeId = request.Id,
                Name = x.Name,
                BankBranch = x.BankBranch,
                AccountName = x.AccountName,
                AccountNumber = x.AccountNumber,
                Default = x.Default,
                Status = x.Status ?? 1,
                SortOrder = x.SortOrder
            }).ToList(),
            CustomerContacts = request.ListContact?.Select(x => new CustomerContact()
            {
                Id = (Guid)x.Id,
                EmployeeId = request.Id,
                Gender = x.Gender,
                Facebook = x.Facebook,
                Tags = x.Tags,
                Address = x.Address,
                Name = x.Name,
                JobTitle = x.JobTitle,
                Phone = x.Phone,
                Email = x.Email,
                Note = x.Note,
                Status = x.Status ?? 1,
                SortOrder = x.SortOrder
            }).ToList()
        };
        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.Group?.Count > 0)
        {
            List<GroupEmployeeMapping> list = new List<GroupEmployeeMapping>();
            foreach (var u in request.Group)
            {
                list.Add(new GroupEmployeeMapping()
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = request.Id,
                    GroupEmployeeId = u.GroupEmployeeId,
                    IsLeader = u.IsLeader,
                    CreatedDate = createdDate,
                    CreatedBy = createdBy,
                    CreatedByName = createName
                });
            }
            _groupEmpRepository.Add(list);
            _ = await CommitNoCheck(_groupEmpRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(EmployeeDeleteCommand request, CancellationToken cancellationToken)
    {

        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var employee = await _repository.GetById(request.Id);
        var filter = new Dictionary<string, object> { { "accountId", employee.AccountId } };

        var customers = await _customerRepository.Filter(filter);
        var quotations = await _quotationRepository.Filter(filter);
        var productionOrders = await _productionOrderRepository.Filter(filter);
        var requestQuotes = await _requestQuoteRepository.Filter(filter);
        var contracts = await _contractRepository.Filter(filter);
        var orders = await _orderRepository.Filter(filter);
        var paymentInvoices = await _paymentInvoiceRepository.Filter(filter, null);
        var requestPurchases = await _requestPurchaseRepository.Filter(filter);
        var returnOrders = await _returnOrderRepository.Filter(filter);
        var exportWarehouses = await _exportWarehouseRepository.Filter(filter);

        if (customers.Any() || quotations.Any() || productionOrders.Any() || requestQuotes.Any() ||
            contracts.Any() || orders.Any() || paymentInvoices.Any() || requestPurchases.Any() ||
            returnOrders.Any() || exportWarehouses.Any())
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
        }

        var item = new Employee
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EmployeeEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var obj = await _repository.GetById(request.Id);

        var employeeAddress = await _addressRepository.GetByParentId(request.Id);
        var employeeContact = await _contactRepository.GetByParentId(request.Id);
        var employeeBank = await _bankRepository.GetByParentId(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Employee is not exist") } };
        }
        obj.IsCustomer = request.IsCustomer;
        obj.AccountId = request.AccountId;
        obj.AccountName = request.AccountName;
        obj.Email = request.Email;
        obj.Name = request.Name;
        obj.Image = request.Image;
        obj.Phone = request.Phone;
        obj.Country = request.Country;
        obj.Province = request.Province;
        obj.District = request.District;
        obj.Ward = request.Ward;
        obj.Address = request.Address;
        obj.Gender = request.Gender;
        obj.Year = request.Year;
        obj.Month = request.Month;
        obj.Day = request.Day;
        obj.TaxCode = request.TaxCode;
        obj.GroupEmployee = request.GroupEmployee;
        obj.Description = request.Description;
        obj.Status = request.Status;
        obj.UpdatedBy = updatedBy;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedByName = updateName;

        _repository.Update(obj);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.Delete?.Count > 0)
        {
            foreach (var d in request.Delete)
            {
                var item = await _groupEmpRepository.GetById(d.Id);
                if (item is not null)
                {
                    _groupEmpRepository.Remove(item);
                    _ = await CommitNoCheck(_groupEmpRepository.UnitOfWork);
                }
            }
        }

        if (request.Group?.Count > 0)
        {
            List<GroupEmployeeMapping> listAdd = new List<GroupEmployeeMapping>();
            List<GroupEmployeeMapping> listUpdate = new List<GroupEmployeeMapping>();

            var i = 1;
            foreach (var u in request.Group)
            {
                var item = await _groupEmpRepository.GetById((Guid)u.Id);
                if (item is not null)
                {
                    item.Id = (Guid)u.Id;
                    item.EmployeeId = request.Id;
                    item.GroupEmployeeId = u.GroupEmployeeId;
                    item.IsLeader = u.IsLeader;
                    item.CreatedDate = updatedDate;
                    item.CreatedBy = updatedBy;
                    item.CreatedByName = updateName;
                    listUpdate.Add(item);
                }
                else
                {
                    listAdd.Add(new GroupEmployeeMapping()
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = request.Id,
                        GroupEmployeeId = u.GroupEmployeeId,
                        IsLeader = u.IsLeader,
                        CreatedDate = updatedDate,
                        CreatedBy = updatedBy,
                        CreatedByName = updateName
                    });
                }
                i++;
            }
            _groupEmpRepository.Add(listAdd);
            _groupEmpRepository.Update(listUpdate);
            _ = await CommitNoCheck(_groupEmpRepository.UnitOfWork);
        }

        if (request.ListBank.Any())
        {
            var listUpdate = new List<CustomerBank>();
            var listDelete = new List<CustomerBank>();
            foreach (var element in employeeBank)
            {
                var objectBank = request.ListBank.FirstOrDefault(x => x.Id.Equals(element.Id));
                if (objectBank != null)
                {
                    element.BankBranch = objectBank.BankBranch;
                    element.AccountName = objectBank.AccountName;
                    element.AccountNumber = objectBank.AccountNumber;
                    element.Default = objectBank.Default;
                    element.Status = objectBank.Status;
                    element.SortOrder = objectBank.SortOrder;
                    element.UpdatedBy = updatedBy;
                    element.UpdatedByName = updateName;
                    element.UpdatedDate = updatedDate;

                    listUpdate.Add(element);
                    request.ListBank.Remove(objectBank);
                }
                else
                {
                    listDelete.Add(element);
                }
            }

            var listAdd = new List<CustomerBank>();
            for (int i = 0; i < request.ListBank.Count; i++)
            {
                var objectBank = employeeBank.FirstOrDefault(x => x.Id.Equals(request.ListBank[i].Id));
                if (objectBank is null)
                {
                    listAdd.Add(new CustomerBank()
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = request.Id,
                        Name = request.ListBank[i].Name,
                        BankBranch = request.ListBank[i].BankBranch,
                        AccountName = request.ListBank[i].AccountName,
                        AccountNumber = request.ListBank[i].AccountNumber,
                        Default = request.ListBank[i].Default,
                        Status = request.ListBank[i].Status,
                        SortOrder = request.ListBank[i].SortOrder,
                        CreatedBy = updatedBy,
                        CreatedByName = updateName,
                        CreatedDate = updatedDate,
                    });

                    request.ListBank.Remove(request.ListBank[i]);
                    i--;
                }
            }

            _bankRepository.Update(listUpdate);
            _bankRepository.Add(listAdd);
            _bankRepository.Remove(listDelete);
            _ = await CommitNoCheck(_bankRepository.UnitOfWork);
        }
        else
        {
            _bankRepository.Remove(employeeBank);
            _ = await CommitNoCheck(_bankRepository.UnitOfWork);
        }

        if (request.ListAddress.Any())
        {
            var listUpdate = new List<CustomerAddress>();
            var listDelete = new List<CustomerAddress>();
            foreach (var element in employeeAddress)
            {
                var objectAddress = request.ListAddress.FirstOrDefault(x => x.Id.Equals(element.Id));
                if (objectAddress != null)
                {
                    element.Name = objectAddress.Name;
                    element.Country = objectAddress.Country;
                    element.Province = objectAddress.Province;
                    element.District = objectAddress.District;
                    element.Ward = objectAddress.Ward;
                    element.Address = objectAddress.Address;
                    element.Phone = objectAddress.Phone;
                    element.Email = String.Join(",", objectAddress.Email);
                    element.ShippingDefault = objectAddress.ShippingDefault;
                    element.BillingDefault = objectAddress.BillingDefault;
                    element.Note = objectAddress.Note;
                    element.Status = objectAddress.Status;
                    element.SortOrder = objectAddress.SortOrder;
                    element.UpdatedBy = updatedBy;
                    element.UpdatedByName = updateName;
                    element.UpdatedDate = updatedDate;

                    listUpdate.Add(element);
                    request.ListAddress.Remove(objectAddress);
                }
                else
                {
                    listDelete.Add(element);
                }
            }

            var listAdd = new List<CustomerAddress>();
            for (int i = 0; i < request.ListAddress.Count; i++)
            {
                var objectAddress = employeeAddress.FirstOrDefault(x => x.Id.Equals(request.ListAddress[i].Id));
                if (objectAddress is null)
                {
                    listAdd.Add(new CustomerAddress()
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = request.Id,
                        Name = request.ListAddress[i].Name,
                        Country = request.ListAddress[i].Country,
                        Province = request.ListAddress[i].Province,
                        District = request.ListAddress[i].District,
                        Ward = request.ListAddress[i].Ward,
                        Address = request.ListAddress[i].Address,
                        Phone = request.ListAddress[i].Phone,
                        Email = String.Join(",", request.ListAddress[i].Email),
                        ShippingDefault = request.ListAddress[i].ShippingDefault,
                        BillingDefault = request.ListAddress[i].BillingDefault,
                        Note = request.ListAddress[i].Note,
                        Status = request.ListAddress[i].Status,
                        SortOrder = request.ListAddress[i].SortOrder,
                        CreatedBy = updatedBy,
                        CreatedByName = updateName,
                        CreatedDate = updatedDate,
                    });

                    request.ListAddress.Remove(request.ListAddress[i]);
                    i--;
                }
            }

            _addressRepository.Update(listUpdate);
            _addressRepository.Add(listAdd);
            _addressRepository.Remove(listDelete);
            _ = await CommitNoCheck(_addressRepository.UnitOfWork);
        }
        else
        {
            _addressRepository.Remove(employeeAddress);
            _ = await CommitNoCheck(_addressRepository.UnitOfWork);
        }

        if (request.ListContact.Any())
        {
            var listUpdate = new List<CustomerContact>();
            var listDelete = new List<CustomerContact>();
            foreach (var element in employeeContact)
            {
                var objectContact = request.ListContact.FirstOrDefault(x => x.Id.Equals(element.Id));
                if (objectContact != null)
                {
                    element.Gender = objectContact.Gender;
                    element.Facebook = objectContact.Facebook;
                    element.Tags = objectContact.Tags;
                    element.Address = objectContact.Address;
                    element.Name = objectContact.Name;
                    element.JobTitle = objectContact.JobTitle;
                    element.Phone = objectContact.Phone;
                    element.Email = String.Join(",", objectContact.Email);
                    element.Note = objectContact.Note;
                    element.Status = objectContact.Status;
                    element.SortOrder = objectContact.SortOrder;
                    element.UpdatedBy = updatedBy;
                    element.UpdatedByName = updateName;
                    element.UpdatedDate = updatedDate;

                    listUpdate.Add(element);
                    request.ListContact.Remove(objectContact);
                }
                else
                {
                    listDelete.Add(element);
                }
            }

            var listAdd = new List<CustomerContact>();
            for (int i = 0; i < request.ListContact.Count; i++)
            {
                var objectContact = employeeContact.FirstOrDefault(x => x.Id.Equals(request.ListAddress[i].Id));
                if (objectContact is null)
                {
                    listAdd.Add(new CustomerContact()
                    {
                        Id = (Guid)request.ListContact[i].Id,
                        EmployeeId = request.Id,
                        Gender = request.ListContact[i].Gender,
                        Facebook = request.ListContact[i].Facebook,
                        Tags = request.ListContact[i].Tags,
                        Address = request.ListContact[i].Address,
                        Name = request.ListContact[i].Name,
                        JobTitle = request.ListContact[i].JobTitle,
                        Phone = request.ListContact[i].Phone,
                        Email = request.ListContact[i].Email,
                        Note = request.ListContact[i].Note,
                        Status = request.ListContact[i].Status ?? 1,
                        SortOrder = request.ListContact[i].SortOrder,
                        CreatedBy = updatedBy,
                        CreatedByName = updateName,
                        CreatedDate = updatedDate
                    });

                    request.ListContact.Remove(request.ListContact[i]);
                    i--;
                }
            }

            _contactRepository.Update(listUpdate);
            _contactRepository.Add(listAdd);
            _contactRepository.Remove(listDelete);
            _ = await CommitNoCheck(_contactRepository.UnitOfWork);
        }
        else
        {
            _contactRepository.Remove(employeeContact);
            _ = await CommitNoCheck(_contactRepository.UnitOfWork);
        }

        return result;

    }
}
