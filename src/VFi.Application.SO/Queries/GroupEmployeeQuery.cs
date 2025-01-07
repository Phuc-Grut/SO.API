using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class GroupEmployeeQueryAll : IQuery<IEnumerable<GroupEmployeeDto>>
{
    public GroupEmployeeQueryAll()
    {
    }
}

public class GroupEmployeeQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public GroupEmployeeQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class GroupEmployeeQueryCheckCode : IQuery<bool>
{

    public GroupEmployeeQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class GroupEmployeeQueryById : IQuery<GroupEmployeeDto>
{
    public GroupEmployeeQueryById()
    {
    }

    public GroupEmployeeQueryById(Guid groupEmployeeId)
    {
        GroupEmployeeId = groupEmployeeId;
    }

    public Guid GroupEmployeeId { get; set; }
}
public class GroupEmployeeQueryByListId : IQuery<IEnumerable<GroupEmployeeDto>>
{
    public GroupEmployeeQueryByListId()
    {
    }

    public GroupEmployeeQueryByListId(List<Guid> listGroupEmployeeId)
    {
        ListGroupEmployeeId = listGroupEmployeeId;
    }

    public List<Guid> ListGroupEmployeeId { get; set; }
}
public class GroupEmployeePagingQuery : FopQuery, IQuery<PagedResult<List<GroupEmployeeDto>>>
{
    public GroupEmployeePagingQuery(int? status, string? keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
}

public class GroupEmployeeQueryHandler : IQueryHandler<GroupEmployeeQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<GroupEmployeeQueryAll, IEnumerable<GroupEmployeeDto>>,
                                         IQueryHandler<GroupEmployeeQueryByListId, IEnumerable<GroupEmployeeDto>>,
                                         IQueryHandler<GroupEmployeeQueryCheckCode, bool>,
                                         IQueryHandler<GroupEmployeeQueryById, GroupEmployeeDto>,
                                         IQueryHandler<GroupEmployeePagingQuery, PagedResult<List<GroupEmployeeDto>>>
{
    private readonly IGroupEmployeeRepository _groupEmployeeRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IContextUser _contextUser;

    public GroupEmployeeQueryHandler(IGroupEmployeeRepository groupEmployeeRespository,
                                     IEmployeeRepository employeeRepository,
                                     IContextUser contextUser)
    {
        _groupEmployeeRepository = groupEmployeeRespository;
        _employeeRepository = employeeRepository;
        _contextUser = contextUser;
    }
    public async Task<bool> Handle(GroupEmployeeQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _groupEmployeeRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<GroupEmployeeDto> Handle(GroupEmployeeQueryById request, CancellationToken cancellationToken)
    {
        var item = await _groupEmployeeRepository.GetById(request.GroupEmployeeId);
        var result = new GroupEmployeeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<GroupEmployeeDto>>> Handle(GroupEmployeePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<GroupEmployeeDto>>();

        var fopRequest = FopExpressionBuilder<GroupEmployee>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status.ToString());
        }
        var (datas, count) = await _groupEmployeeRepository.Filter(request.Keyword, filter, fopRequest);
        var result = datas.Select(groupEmployee =>
        {
            return new GroupEmployeeDto()
            {
                Id = groupEmployee.Id,
                Code = groupEmployee.Code,
                Name = groupEmployee.Name,
                Description = groupEmployee.Description,
                Status = groupEmployee.Status,
                CreatedDate = groupEmployee.CreatedDate,
                CreatedBy = groupEmployee.CreatedBy,
                UpdatedDate = groupEmployee.UpdatedDate,
                UpdatedBy = groupEmployee.UpdatedBy,
                CreatedByName = groupEmployee.CreatedByName,
                UpdatedByName = groupEmployee.UpdatedByName,
                Leader = groupEmployee.GroupEmployeeMapping.Any(x => x.IsLeader == true && x.GroupEmployeeId == groupEmployee.Id)
            };
        }).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<GroupEmployeeDto>> Handle(GroupEmployeeQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _groupEmployeeRepository.GetAll();
        var result = items.Select(item => new GroupEmployeeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(GroupEmployeeQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status.ToString());
        }
        Guid? leadId = null;
        if (_contextUser.QueryMyData())
        {
            leadId = _employeeRepository.GetByAccountId(_contextUser.GetUserId()).Result.Id;
            filter.Add("leadId", leadId);
        }
        var GroupEmployees = await _groupEmployeeRepository.GetListCbx(filter);
        var result = GroupEmployees.Select(x => new GroupEmployeeComboboxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            CreatedDate = x.CreatedDate,
        });
        return result;
    }

    public async Task<IEnumerable<GroupEmployeeDto>> Handle(GroupEmployeeQueryByListId request, CancellationToken cancellationToken)
    {
        var items = await _groupEmployeeRepository.GetByListId(request.ListGroupEmployeeId);
        var result = items.Select(item => new GroupEmployeeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Leader = item.GroupEmployeeMapping.Any(x => x.IsLeader == true && x.GroupEmployeeId == item.Id)
        }).ToList();
        return result;
    }
}
