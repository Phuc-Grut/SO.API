using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CustomerBankQueryAll : IQuery<IEnumerable<CustomerBankDto>>
{
    public CustomerBankQueryAll()
    {
    }
}

public class CustomerBankQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CustomerBankQueryComboBox(int? status, Guid? customerId)
    {
        Status = status;
        CustomerId = customerId;
    }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}
public class CustomerBankQueryById : IQuery<CustomerBankDto>
{
    public CustomerBankQueryById()
    {
    }

    public CustomerBankQueryById(Guid employeeId)
    {
        CustomerBankId = employeeId;
    }

    public Guid CustomerBankId { get; set; }
}
public class CustomerBankPagingQuery : ListQuery, IQuery<PagingResponse<CustomerBankDto>>
{
    public CustomerBankPagingQuery(string? keyword, int? status, Guid? customerId, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        CustomerId = customerId;
        Status = status;
    }

    public CustomerBankPagingQuery(string? keyword, int? status, Guid? customerId, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Status = status;
        CustomerId = customerId;
    }

    public string? Keyword { get; set; }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}

public class CustomerBankQueryByCustomerId : IQuery<IEnumerable<CustomerBankDto>>
{
    public CustomerBankQueryByCustomerId(int? status, Guid? customerId)
    {
        Status = status;
        CustomerId = customerId;
    }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}
public class CustomerBankQueryByAccountId : IQuery<IEnumerable<CustomerBankDto>>
{
    public CustomerBankQueryByAccountId(int? status, Guid accountId)
    {
        Status = status;
        AccountId = accountId;
    }
    public int? Status { get; set; }
    public Guid AccountId { get; set; }
}
public class CustomerBankQueryHandler : IQueryHandler<CustomerBankQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CustomerBankQueryByCustomerId, IEnumerable<CustomerBankDto>>,
                                         IQueryHandler<CustomerBankQueryByAccountId, IEnumerable<CustomerBankDto>>,
                                         IQueryHandler<CustomerBankQueryAll, IEnumerable<CustomerBankDto>>,
                                         IQueryHandler<CustomerBankQueryById, CustomerBankDto>,
                                         IQueryHandler<CustomerBankPagingQuery, PagingResponse<CustomerBankDto>>
{
    private readonly ICustomerBankRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    public CustomerBankQueryHandler(ICustomerBankRepository repository, ICustomerRepository customerRepository)
    {
        _repository = repository;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerBankDto> Handle(CustomerBankQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.CustomerBankId);
        var result = new CustomerBankDto()
        {
            Id = item.Id,
            CustomerId = item.CustomerId,
            Name = item.Name,
            BankCode = item.BankCode,
            BankName = item.BankName,
            BankBranch = item.BankBranch,
            AccountName = item.AccountName,
            AccountNumber = item.AccountNumber,
            Default = item.Default,
            Status = item.Status,
            SortOrder = item.SortOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagingResponse<CustomerBankDto>> Handle(CustomerBankPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<CustomerBankDto>();
        var count = await _repository.FilterCount(request.Keyword, request.Status, request.CustomerId);
        var items = await _repository.Filter(request.Keyword, request.Status, request.CustomerId, request.PageSize, request.PageIndex);
        var data = items.Select(item => new CustomerBankDto()
        {

            Id = item.Id,
            CustomerId = item.CustomerId,
            Name = item.Name,
            BankCode = item.BankCode,
            BankName = item.BankName,
            BankBranch = item.BankBranch,
            AccountName = item.AccountName,
            AccountNumber = item.AccountNumber,
            Default = item.Default,
            Status = item.Status,
            SortOrder = item.SortOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<CustomerBankDto>> Handle(CustomerBankQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new CustomerBankDto()
        {

            Id = item.Id,
            CustomerId = item.CustomerId,
            Name = item.Name,
            BankBranch = item.BankBranch,
            AccountName = item.AccountName,
            AccountNumber = item.AccountNumber,
            Default = item.Default,
            Status = item.Status,
            SortOrder = item.SortOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(CustomerBankQueryComboBox request, CancellationToken cancellationToken)
    {

        var items = await _repository.GetListCbx(request.Status, request.CustomerId);
        var result = items.Where(x => x.Status == request.Status).Select(x => new ComboBoxDto()
        {
            Key = x.AccountNumber,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }

    public async Task<IEnumerable<CustomerBankDto>> Handle(CustomerBankQueryByCustomerId request, CancellationToken cancellationToken)
    {
        var items = await _repository.Filter(request.Status, request.CustomerId);
        var result = items.Select(item => new CustomerBankDto()
        {

            Id = item.Id,
            CustomerId = item.CustomerId,
            Name = item.Name,
            BankCode = item.BankCode,
            BankName = item.BankName,
            BankBranch = item.BankBranch,
            AccountName = item.AccountName,
            AccountNumber = item.AccountNumber,
            Default = item.Default,
            Status = item.Status,
            SortOrder = item.SortOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).OrderBy(x => x.SortOrder);
        return result;
    }
    public async Task<IEnumerable<CustomerBankDto>> Handle(CustomerBankQueryByAccountId request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();

        var customer = await _customerRepository.GetByAccountId(request.AccountId);

        filter.Add("customerId", customer.Id);
        if (request.Status.HasValue)
            filter.Add("status", request.Status.Value);
        var customerBanks = await _repository.Filter("", filter, 1000, 1);
        var result = customerBanks.Select(x => new CustomerBankDto()
        {

            Id = x.Id,
            CustomerId = x.CustomerId,
            Name = x.Name,
            BankCode = x.BankCode,
            BankName = x.BankName,
            BankBranch = x.BankBranch,
            AccountName = x.AccountName,
            AccountNumber = x.AccountNumber,
            Default = x.Default,
            Status = x.Status,
            SortOrder = x.SortOrder,
            CreatedDate = x.CreatedDate,
            CreatedBy = x.CreatedBy,
            UpdatedDate = x.UpdatedDate,
            UpdatedBy = x.UpdatedBy,
            CreatedByName = x.CreatedByName,
            UpdatedByName = x.UpdatedByName
        }).OrderBy(x => x.SortOrder);
        return result;
    }
}
