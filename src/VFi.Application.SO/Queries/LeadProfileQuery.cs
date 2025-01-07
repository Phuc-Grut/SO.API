using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class LeadProfileQueryAll : IQuery<IEnumerable<LeadProfileDto>>
{
    public LeadProfileQueryAll()
    {
    }
}

public class LeadProfileQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public LeadProfileQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}

public class LeadProfileQueryById : IQuery<LeadProfileDto>
{
    public LeadProfileQueryById()
    {
    }

    public LeadProfileQueryById(Guid LeadProfileId)
    {
        LeadProfileId = LeadProfileId;
    }

    public Guid LeadProfileId { get; set; }
}
public class LeadProfilePagingQuery : FopQuery, IQuery<PagedResult<List<LeadProfileDto>>>
{
    public LeadProfilePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }

}

public class LeadProfileQueryHandler : IQueryHandler<LeadProfileQueryAll, IEnumerable<LeadProfileDto>>,
                                         IQueryHandler<LeadProfileQueryById, LeadProfileDto>,
                                         IQueryHandler<LeadProfilePagingQuery, PagedResult<List<LeadProfileDto>>>
{
    private readonly ILeadProfileRepository _LeadProfileRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public LeadProfileQueryHandler(ILeadProfileRepository LeadProfileRespository, IEmployeeRepository employeeRespository)
    {
        _LeadProfileRepository = LeadProfileRespository;
        _employeeRepository = employeeRespository;
    }

    public async Task<LeadProfileDto> Handle(LeadProfileQueryById request, CancellationToken cancellationToken)
    {
        var item = await _LeadProfileRepository.GetById(request.LeadProfileId);
        var filter = new Dictionary<string, object>();
        var result = new LeadProfileDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            Key = item.Key,
            Value = item.Value,
            Description = item.Description,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<LeadProfileDto>>> Handle(LeadProfilePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<LeadProfileDto>>();

        var fopRequest = FopExpressionBuilder<LeadProfile>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _LeadProfileRepository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new LeadProfileDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            Key = item.Key,
            Value = item.Value,
            Description = item.Description,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<LeadProfileDto>> Handle(LeadProfileQueryAll request, CancellationToken cancellationToken)
    {
        var datas = await _LeadProfileRepository.GetAll();
        var result = datas.Select(item => new LeadProfileDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            Key = item.Key,
            Value = item.Value,
            Description = item.Description,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

}
