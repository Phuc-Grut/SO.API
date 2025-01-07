using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using static System.Net.Mime.MediaTypeNames;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Application.SO.Queries;

public class EmployeeQueryComboBox : IQuery<IEnumerable<EmployeeListBoxDto>>
{
    public EmployeeQueryComboBox(int? status, Guid? groupId)
    {
        Status = status;
        GroupId = groupId;
    }
    public int? Status { get; set; }
    public Guid? GroupId { get; set; }
}
public class EmployeeQueryByListId : IQuery<IEnumerable<EmployeeListBoxDto>>
{
    public EmployeeQueryByListId(string? listId)
    {
        ListId = listId;
    }
    public string? ListId { get; set; }
}
public class EmployeeQueryById : IQuery<EmployeeDto>
{
    public EmployeeQueryById()
    {
    }

    public EmployeeQueryById(Guid employeeId)
    {
        EmployeeId = employeeId;
    }

    public Guid EmployeeId { get; set; }
}

public class EmployeePagingQuery : FopQuery, IQuery<PagedResult<List<EmployeeDto>>>
{
    public EmployeePagingQuery(int? status, string? keyword, Guid? groupId, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
        GroupId = groupId;
    }
    public Guid? GroupId { get; set; }

}


public class EmployeeQueryHandler : IQueryHandler<EmployeeQueryComboBox, IEnumerable<EmployeeListBoxDto>>,
                                         IQueryHandler<EmployeeQueryByListId, IEnumerable<EmployeeListBoxDto>>,
                                         IQueryHandler<EmployeeQueryById, EmployeeDto>,
                                         IQueryHandler<EmployeePagingQuery, PagedResult<List<EmployeeDto>>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IGroupEmployeeMappingRepository _groupEmpRepository;
    public EmployeeQueryHandler(IEmployeeRepository employeeRespository, IGroupEmployeeMappingRepository groupEmpRepository)
    {
        _employeeRepository = employeeRespository;
        _groupEmpRepository = groupEmpRepository;
    }

    public async Task<EmployeeDto> Handle(EmployeeQueryById request, CancellationToken cancellationToken)
    {
        var item = await _employeeRepository.GetByIdQuery(request.EmployeeId);
        var groupEmp = await _groupEmpRepository.Filter(request.EmployeeId);
        var result = new EmployeeDto()
        {
            Id = item.Id,
            IsCustomer = item.IsCustomer,
            Code = item.Code,
            AccountId = item.AccountId,
            AccountName = item.AccountName,
            Email = item.Email,
            Name = item.Name,
            Image = item.Image,
            Phone = item.Phone,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            Address = item.Address,
            Gender = item.Gender,
            Year = item.Year,
            Month = item.Month,
            Day = item.Day,
            TaxCode = item.TaxCode,
            GroupEmployee = item.GroupEmployee,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            Groups = groupEmp.Select(x => new GroupEmployeeMappingDto()
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                GroupEmployeeId = x.GroupEmployeeId,
                GroupEmployeeName = x.GroupEmployee?.Name,
                IsLeader = x.IsLeader,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                CreatedByName = x.CreatedByName
            }).ToList(),
            ListAddress = item.CustomerAddresses?.Select(x => new CustomerAddressDto()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                EmployeeId = x.EmployeeId,
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
                Note = x.Note,
                SortOrder = x.SortOrder,
                Status = x.Status,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName
            }).ToList(),
            ListContact = item.CustomerContacts.Select(x => new CustomerContactDto()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                EmployeeId = x.EmployeeId,
                Gender = x.Gender,
                Facebook = x.Facebook,
                Tags = x.Tags,
                Address = x.Address,
                Name = x.Name,
                JobTitle = x.JobTitle,
                Phone = x.Phone,
                Email = x.Email,
                Note = x.Note,
                Status = x.Status,
                SortOrder = x.SortOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate
            }).ToList(),
            ListBank = item.CustomerBanks.Select(x => new CustomerBankDto()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                EmployeeId = x.EmployeeId,
                AccountName = x.AccountName,
                AccountNumber = x.AccountNumber,
                BankBranch = x.BankBranch,
                Default = x.Default,
                Name = x.Name,
                Status = x.Status,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                CreatedByName = x.CreatedByName,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                UpdatedByName = x.UpdatedByName,
                SortOrder = x.SortOrder
            }).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<EmployeeDto>>> Handle(EmployeePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<EmployeeDto>>();

        var fopRequest = FopExpressionBuilder<Employee>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status.ToString());
        }
        if (request.GroupId != null)
        {
            filter.Add("groupId", request.GroupId);
        }
        var (datas, count) = await _employeeRepository.Filter(request.Keyword, filter, fopRequest);
        var result = datas.Select(obj =>
        {
            return new EmployeeDto()
            {
                Id = obj.Id,
                AccountId = obj.AccountId,
                Email = obj.Email,
                Code = obj.Code,
                Name = obj.Name,
                Image = obj.Image,
                GroupEmployee = obj.GroupEmployee,
                Description = obj.Description,
                Status = obj.Status,
                IsLeader = request.GroupId != null && obj.GroupEmployeeMapping.Any(x => x.IsLeader == true && x.GroupEmployeeId == request.GroupId)
            };
        }).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<EmployeeListBoxDto>> Handle(EmployeeQueryComboBox request, CancellationToken cancellationToken)
    {

        var items = await _employeeRepository.GetListCbx(request.Status, request.GroupId);
        var result = items.Where(x => x.Status == request.Status).Select(x => new EmployeeListBoxDto()
        {
            Key = x.Email,
            Value = x.Id,
            Code = x.Code,
            Name = x.Name,
            Label = x.Code + " - " + x.Name,
            AccountId = x.AccountId,
            Image = x.Image,
            ListGroup = x.GroupEmployeeMapping?.Select(y => new GroupEmployeeDto()
            {
                Id = (Guid)y.GroupEmployeeId,
                Code = y.GroupEmployee?.Code,
                Name = y.GroupEmployee?.Name,
            }).ToList()
        });
        return result;
    }
    public async Task<IEnumerable<EmployeeListBoxDto>> Handle(EmployeeQueryByListId request, CancellationToken cancellationToken)
    {

        var data = await _employeeRepository.GetByListId(request.ListId);
        var result = data.Select(x => new EmployeeListBoxDto()
        {
            Key = x.Email,
            Value = x.Id,
            Code = x.Code,
            Name = x.Name,
            Label = x.Code + " - " + x.Name,
            AccountId = x.AccountId,
            Image = x.Image
        });
        return result;
    }
}
