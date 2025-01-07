using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CustomerAddressQueryAll : IQuery<IEnumerable<CustomerAddressDto>>
{
    public CustomerAddressQueryAll()
    {
    }
}

public class CustomerAddressQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CustomerAddressQueryComboBox(int? status, Guid? customerId)
    {
        Status = status;
        CustomerId = customerId;
    }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}
public class CustomerAddressQueryById : IQuery<CustomerAddressDto>
{
    public CustomerAddressQueryById()
    {
    }

    public CustomerAddressQueryById(Guid employeeId)
    {
        CustomerAddressId = employeeId;
    }

    public Guid CustomerAddressId { get; set; }
}
public class CustomerAddressPagingQuery : ListQuery, IQuery<PagingResponse<CustomerAddressDto>>
{
    public CustomerAddressPagingQuery(string? keyword, int? status, Guid? customerId, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        CustomerId = customerId;
        Status = status;
    }

    public CustomerAddressPagingQuery(string? keyword, int? status, Guid? customerId, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Status = status;
        CustomerId = customerId;
    }

    public string? Keyword { get; set; }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}

public class CustomerAddressQueryByCustomerId : IQuery<IEnumerable<CustomerAddressDto>>
{
    public CustomerAddressQueryByCustomerId(int? status, Guid? customerId)
    {
        Status = status;
        CustomerId = customerId;
    }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}
public class CustomerAddressQueryByAccountId : IQuery<IEnumerable<CustomerAddressDto>>
{
    public CustomerAddressQueryByAccountId(int? status, Guid accountId)
    {
        Status = status;
        AccountId = accountId;
    }
    public int? Status { get; set; }
    public Guid AccountId { get; set; }
}
public class CustomerAddressQueryHandler : IQueryHandler<CustomerAddressQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CustomerAddressQueryByCustomerId, IEnumerable<CustomerAddressDto>>,
                                         IQueryHandler<CustomerAddressQueryByAccountId, IEnumerable<CustomerAddressDto>>,
                                         IQueryHandler<CustomerAddressQueryAll, IEnumerable<CustomerAddressDto>>,
                                         IQueryHandler<CustomerAddressQueryById, CustomerAddressDto>,
                                         IQueryHandler<CustomerAddressPagingQuery, PagingResponse<CustomerAddressDto>>
{
    private readonly ICustomerAddressRepository _customerAddressRepository;
    private readonly ICustomerRepository _customerRepository;
    public CustomerAddressQueryHandler(ICustomerAddressRepository customerAddressRespository, ICustomerRepository customerRepository)
    {
        _customerAddressRepository = customerAddressRespository;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerAddressDto> Handle(CustomerAddressQueryById request, CancellationToken cancellationToken)
    {
        var item = await _customerAddressRepository.GetById(request.CustomerAddressId);
        var result = new CustomerAddressDto()
        {
            Id = item.Id,
            CustomerId = (Guid)item.CustomerId,
            Name = item.Name,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            Address = item.Address,
            Phone = item.Phone,
            Email = item.Email,
            ShippingDefault = item.ShippingDefault,
            BillingDefault = item.BillingDefault,
            Status = item.Status,
            SortOrder = item.SortOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        };
        return result;
    }

    public async Task<PagingResponse<CustomerAddressDto>> Handle(CustomerAddressPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<CustomerAddressDto>();
        var filter = new Dictionary<string, object>();
        if (request.CustomerId.HasValue)
            filter.Add("customerId", request.CustomerId.Value);
        if (request.Status.HasValue)
            filter.Add("status", request.Status.Value);
        var count = await _customerAddressRepository.FilterCount(request.Keyword, filter);
        var datas = await _customerAddressRepository.Filter(request.Keyword, filter, request.PageSize, request.PageIndex);
        var data = datas.Select(item => new CustomerAddressDto()
        {

            Id = item.Id,
            CustomerId = (Guid)item.CustomerId,
            Name = item.Name,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            Address = item.Address,
            Phone = item.Phone,
            Email = item.Email,
            ShippingDefault = item.ShippingDefault,
            BillingDefault = item.BillingDefault,
            Status = item.Status,
            SortOrder = item.SortOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<CustomerAddressDto>> Handle(CustomerAddressQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _customerAddressRepository.GetAll();
        var result = items.Select(item => new CustomerAddressDto()
        {

            Id = item.Id,
            CustomerId = (Guid)item.CustomerId,
            Name = item.Name,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            Address = item.Address,
            Phone = item.Phone,
            Email = item.Email,
            ShippingDefault = item.ShippingDefault,
            BillingDefault = item.BillingDefault,
            Status = item.Status,
            SortOrder = item.SortOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(CustomerAddressQueryComboBox request, CancellationToken cancellationToken)
    {

        var CustomerAddresss = await _customerAddressRepository.GetListCbx(request.Status, request.CustomerId);
        var result = CustomerAddresss.Where(x => x.Status == request.Status).Select(x => new ComboBoxDto()
        {
            Key = x.Email,
            Value = x.Id,
            Label = x.Address
        });
        return result;
    }

    public async Task<IEnumerable<CustomerAddressDto>> Handle(CustomerAddressQueryByCustomerId request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.CustomerId.HasValue)
            filter.Add("customerId", request.CustomerId.Value);
        if (request.Status.HasValue)
            filter.Add("status", request.Status.Value);
        var customerAddresss = await _customerAddressRepository.Filter("", filter, 1000, 1);
        var result = customerAddresss.Select(customerAddress => new CustomerAddressDto()
        {

            Id = customerAddress.Id,
            CustomerId = (Guid)customerAddress.CustomerId,
            Name = customerAddress.Name,
            Country = customerAddress.Country,
            Province = customerAddress.Province,
            District = customerAddress.District,
            Ward = customerAddress.Ward,
            Address = customerAddress.Address,
            Phone = customerAddress.Phone,
            Email = customerAddress.Email,
            ShippingDefault = customerAddress.ShippingDefault,
            BillingDefault = customerAddress.BillingDefault,
            Status = customerAddress.Status,
            SortOrder = customerAddress.SortOrder,
            CreatedDate = customerAddress.CreatedDate,
            CreatedBy = customerAddress.CreatedBy,
            UpdatedDate = customerAddress.UpdatedDate,
            UpdatedBy = customerAddress.UpdatedBy
        });
        return result;
    }
    public async Task<IEnumerable<CustomerAddressDto>> Handle(CustomerAddressQueryByAccountId request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();

        var customer = await _customerRepository.GetByAccountId(request.AccountId);

        filter.Add("customerId", customer.Id);
        if (request.Status.HasValue)
            filter.Add("status", request.Status.Value);
        var customerAddresss = await _customerAddressRepository.Filter("", filter, 1000, 1);
        var result = customerAddresss.Select(x => new CustomerAddressDto()
        {

            Id = x.Id,
            CustomerId = (Guid)x.CustomerId,
            Name = x.Name,
            Country = x.Country,
            Province = x.Province,
            District = x.District,
            Ward = x.Ward,
            Address = x.Address,
            Phone = x.Phone,
            Email = x.Email,
            ShippingDefault = x.ShippingDefault,
            BillingDefault = x.BillingDefault,
            Status = x.Status,
            SortOrder = x.SortOrder,
            CreatedDate = x.CreatedDate,
            CreatedBy = x.CreatedBy,
            UpdatedDate = x.UpdatedDate,
            UpdatedBy = x.UpdatedBy
        });
        return result;
    }
}
