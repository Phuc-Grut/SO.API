using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class LeadActivityQueryAll : IQuery<IEnumerable<LeadActivityDto>>
{
    public LeadActivityQueryAll()
    {
    }
}

public class LeadActivityQueryById : IQuery<LeadActivityDto>
{
    public LeadActivityQueryById()
    {
    }

    public LeadActivityQueryById(Guid leadActivityId)
    {
        LeadActivityId = leadActivityId;
    }

    public Guid LeadActivityId { get; set; }
}
public class LeadActivityPagingQuery : FopQuery, IQuery<PagedResult<List<LeadActivityDto>>>
{
    public LeadActivityPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class LeadActivityQueryHandler : IQueryHandler<LeadActivityQueryAll, IEnumerable<LeadActivityDto>>,
                                         IQueryHandler<LeadActivityQueryById, LeadActivityDto>,
                                         IQueryHandler<LeadActivityPagingQuery, PagedResult<List<LeadActivityDto>>>
{
    private readonly ILeadActivityRepository _LeadActivityRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public LeadActivityQueryHandler(ILeadActivityRepository LeadActivityRespository, IEmployeeRepository employeeRespository)
    {
        _LeadActivityRepository = LeadActivityRespository;
        _employeeRepository = employeeRespository;
    }

    public async Task<LeadActivityDto> Handle(LeadActivityQueryById request, CancellationToken cancellationToken)
    {
        var item = await _LeadActivityRepository.GetById(request.LeadActivityId);
        var filter = new Dictionary<string, object>();
        var result = new LeadActivityDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            CampaignId = item.CampaignId,
            Campaign = item.Campaign,
            Type = item.Type,
            Name = item.Name,
            Body = item.Body,
            Status = item.Status,
            ActualDate = item.ActualDate,
            Attachment = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(item.Attachment) ? "" : item.Attachment),
        };
        return result;
    }

    public async Task<PagedResult<List<LeadActivityDto>>> Handle(LeadActivityPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<LeadActivityDto>>();

        var fopRequest = FopExpressionBuilder<LeadActivity>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _LeadActivityRepository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new LeadActivityDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            CampaignId = item.CampaignId,
            Campaign = item.Campaign,
            Type = item.Type,
            Name = item.Name,
            Body = item.Body,
            ActualDate = item.ActualDate,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).OrderByDescending(x => x.CreatedDate).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<LeadActivityDto>> Handle(LeadActivityQueryAll request, CancellationToken cancellationToken)
    {
        var datas = await _LeadActivityRepository.GetAll();
        var result = datas.Select(item => new LeadActivityDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            CampaignId = item.CampaignId,
            Campaign = item.Campaign,
            Type = item.Type,
            Name = item.Name,
            Body = item.Body,
            ActualDate = item.ActualDate,
            Status = item.Status,
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
