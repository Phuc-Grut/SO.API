using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CustomerContactQueryAll : IQuery<IEnumerable<CustomerContactDto>>
{
    public CustomerContactQueryAll()
    {
    }
}

public class CustomerContactQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CustomerContactQueryComboBox(int? status, Guid? customerId)
    {
        Status = status;
        CustomerId = customerId;
    }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}
public class CustomerContactQueryById : IQuery<CustomerContactDto>
{
    public CustomerContactQueryById()
    {
    }

    public CustomerContactQueryById(Guid employeeId)
    {
        CustomerContactId = employeeId;
    }

    public Guid CustomerContactId { get; set; }
}
public class CustomerContactPagingQuery : ListQuery, IQuery<PagingResponse<CustomerContactDto>>
{
    public CustomerContactPagingQuery(string? keyword, int? status, Guid? customerId, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        CustomerId = customerId;
        Status = status;
    }

    public CustomerContactPagingQuery(string? keyword, int? status, Guid? customerId, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Status = status;
        CustomerId = customerId;
    }

    public string? Keyword { get; set; }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}

public class CustomerContactQueryByCustomerId : IQuery<IEnumerable<CustomerContactDto>>
{
    public CustomerContactQueryByCustomerId(int? status, Guid? customerId)
    {
        Status = status;
        CustomerId = customerId;
    }
    public int? Status { get; set; }
    public Guid? CustomerId { get; set; }
}

public class CustomerContactQueryHandler : IQueryHandler<CustomerContactQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CustomerContactQueryByCustomerId, IEnumerable<CustomerContactDto>>,
                                         IQueryHandler<CustomerContactQueryAll, IEnumerable<CustomerContactDto>>,
                                         IQueryHandler<CustomerContactQueryById, CustomerContactDto>,
                                         IQueryHandler<CustomerContactPagingQuery, PagingResponse<CustomerContactDto>>
{
    private readonly ICustomerContactRepository _CustomerContactRepository;
    public CustomerContactQueryHandler(ICustomerContactRepository CustomerContactRespository)
    {
        _CustomerContactRepository = CustomerContactRespository;
    }

    public async Task<CustomerContactDto> Handle(CustomerContactQueryById request, CancellationToken cancellationToken)
    {
        var CustomerContact = await _CustomerContactRepository.GetById(request.CustomerContactId);
        var result = new CustomerContactDto()
        {
            Id = CustomerContact.Id,
            CustomerId = CustomerContact.CustomerId,
            Name = CustomerContact.Name,
            Gender = CustomerContact.Gender,
            Phone = CustomerContact.Phone,
            Email = CustomerContact.Email,
            Facebook = CustomerContact.Facebook,
            Tags = CustomerContact.Tags,
            Address = CustomerContact.Address,
            Status = CustomerContact.Status,
            SortOrder = CustomerContact.SortOrder,
            CreatedDate = CustomerContact.CreatedDate,
            CreatedBy = CustomerContact.CreatedBy,
            UpdatedDate = CustomerContact.UpdatedDate,
            UpdatedBy = CustomerContact.UpdatedBy,
            CreatedByName = CustomerContact.CreatedByName,
            UpdatedByName = CustomerContact.UpdatedByName
        };
        return result;
    }

    public async Task<PagingResponse<CustomerContactDto>> Handle(CustomerContactPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<CustomerContactDto>();
        var count = await _CustomerContactRepository.FilterCount(request.Keyword, request.Status, request.CustomerId);
        var CustomerContacts = await _CustomerContactRepository.Filter(request.Keyword, request.Status, request.CustomerId, request.PageSize, request.PageIndex);
        var data = CustomerContacts.Select(CustomerContact => new CustomerContactDto()
        {

            Id = CustomerContact.Id,
            CustomerId = CustomerContact.CustomerId,
            Name = CustomerContact.Name,
            Gender = CustomerContact.Gender,
            Phone = CustomerContact.Phone,
            Email = CustomerContact.Email,
            Facebook = CustomerContact.Facebook,
            Tags = CustomerContact.Tags,
            Address = CustomerContact.Address,
            Status = CustomerContact.Status,
            SortOrder = CustomerContact.SortOrder,
            CreatedDate = CustomerContact.CreatedDate,
            CreatedBy = CustomerContact.CreatedBy,
            UpdatedDate = CustomerContact.UpdatedDate,
            UpdatedBy = CustomerContact.UpdatedBy,
            CreatedByName = CustomerContact.CreatedByName,
            UpdatedByName = CustomerContact.UpdatedByName
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<CustomerContactDto>> Handle(CustomerContactQueryAll request, CancellationToken cancellationToken)
    {
        var CustomerContacts = await _CustomerContactRepository.GetAll();
        var result = CustomerContacts.Select(CustomerContact => new CustomerContactDto()
        {

            Id = CustomerContact.Id,
            CustomerId = CustomerContact.CustomerId,
            Name = CustomerContact.Name,
            Gender = CustomerContact.Gender,
            Phone = CustomerContact.Phone,
            Email = CustomerContact.Email,
            Facebook = CustomerContact.Facebook,
            Tags = CustomerContact.Tags,
            Address = CustomerContact.Address,
            Status = CustomerContact.Status,
            SortOrder = CustomerContact.SortOrder,
            CreatedDate = CustomerContact.CreatedDate,
            CreatedBy = CustomerContact.CreatedBy,
            UpdatedDate = CustomerContact.UpdatedDate,
            UpdatedBy = CustomerContact.UpdatedBy,
            CreatedByName = CustomerContact.CreatedByName,
            UpdatedByName = CustomerContact.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(CustomerContactQueryComboBox request, CancellationToken cancellationToken)
    {

        var CustomerContacts = await _CustomerContactRepository.GetListCbx(request.Status, request.CustomerId);
        var result = CustomerContacts.Where(x => x.Status == request.Status).Select(x => new ComboBoxDto()
        {
            Key = x.Email,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }

    public async Task<IEnumerable<CustomerContactDto>> Handle(CustomerContactQueryByCustomerId request, CancellationToken cancellationToken)
    {
        var CustomerContacts = await _CustomerContactRepository.Filter(request.Status, request.CustomerId);
        var result = CustomerContacts.Select(CustomerContact => new CustomerContactDto()
        {

            Id = CustomerContact.Id,
            CustomerId = CustomerContact.CustomerId,
            Name = CustomerContact.Name,
            Gender = CustomerContact.Gender,
            Phone = CustomerContact.Phone,
            Email = CustomerContact.Email,
            Facebook = CustomerContact.Facebook,
            Tags = CustomerContact.Tags,
            Address = CustomerContact.Address,
            Status = CustomerContact.Status,
            SortOrder = CustomerContact.SortOrder,
            CreatedDate = CustomerContact.CreatedDate,
            CreatedBy = CustomerContact.CreatedBy,
            UpdatedDate = CustomerContact.UpdatedDate,
            UpdatedBy = CustomerContact.UpdatedBy,
            CreatedByName = CustomerContact.CreatedByName,
            UpdatedByName = CustomerContact.UpdatedByName
        });
        return result;
    }
}
