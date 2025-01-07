using AutoMapper.Internal;
using Consul;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CampaignQueryAll : IQuery<IEnumerable<CampaignDto>>
{
    public CampaignQueryAll()
    {
    }
}

public class CampaignQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CampaignQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class CampaignQueryCheckCode : IQuery<bool>
{

    public CampaignQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class CampaignQueryById : IQuery<CampaignDto>
{
    public CampaignQueryById()
    {
    }

    public CampaignQueryById(Guid campaignId)
    {
        CampaignId = campaignId;
    }

    public Guid CampaignId { get; set; }
}
public class CampaignPagingQuery : FopQuery, IQuery<PagedResult<List<CampaignDto>>>
{
    public CampaignPagingQuery(string? keyword, int? status, string? employeeId, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
        EmployeeId = employeeId;
    }
    public string? EmployeeId { get; set; }

}

public class CampaignQueryHandler : IQueryHandler<CampaignQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CampaignQueryAll, IEnumerable<CampaignDto>>,
                                         IQueryHandler<CampaignQueryCheckCode, bool>,
                                         IQueryHandler<CampaignQueryById, CampaignDto>,
                                         IQueryHandler<CampaignPagingQuery, PagedResult<List<CampaignDto>>>
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly ICampaignStatusRepository _campaignStatusRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public CampaignQueryHandler(ICampaignRepository campaignRespository, ICampaignStatusRepository campaignStatusRepository, IEmployeeRepository employeeRespository)
    {
        _campaignRepository = campaignRespository;
        _campaignStatusRepository = campaignStatusRepository;
        _employeeRepository = employeeRespository;
    }
    public async Task<bool> Handle(CampaignQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _campaignRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<CampaignDto> Handle(CampaignQueryById request, CancellationToken cancellationToken)
    {
        var item = await _campaignRepository.GetById(request.CampaignId);
        var filter = new Dictionary<string, object>();
        if (item.Member != null)
        {
            filter.Add("accountId", item.Member);
        }
        var em = item.Member is not null ? await _employeeRepository.Filter(filter) : new List<Employee>();
        var result = new CampaignDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            Leader = item.Leader,
            LeaderName = item.LeaderName,
            Member = !string.IsNullOrEmpty(item.Member) ? item.Member.Split(',').ToList() : new List<string>(),
            ListMember = em.ToList(),
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            Details = item.CampaignStatuses.Select(x => new CampaignStatusDto()
            {
                Id = x.Id,
                CampaignId = x.CampaignId,
                Name = x.Name,
                Color = x.Color,
                TextColor = x.TextColor,
                IsDefault = x.IsDefault,
                IsClose = x.IsClose,
                Status = x.Status,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                CreatedByName = x.CreatedByName,
            }).OrderBy(x => x.DisplayOrder).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<CampaignDto>>> Handle(CampaignPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<CampaignDto>>();

        var fopRequest = FopExpressionBuilder<Campaign>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filterListBox = new Dictionary<string, object>();
        if (request.Status != null)
            filterListBox.Add("status", request.Status.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filterListBox.Add("employeeId", request.EmployeeId);
        var (datas, count) = await _campaignRepository.Filter(request.Keyword, filterListBox, fopRequest);
        var data = datas.Select(item => new CampaignDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            Leader = item.Leader,
            LeaderName = item.LeaderName,
            Member = !string.IsNullOrEmpty(item.Member) ? item.Member.Split(',').ToList() : new List<string>(),
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            CountLeadCampaigns = item.LeadCount,
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<CampaignDto>> Handle(CampaignQueryAll request, CancellationToken cancellationToken)
    {
        var datas = await _campaignRepository.GetAll();
        var result = datas.Select(item => new CampaignDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            Leader = item.Leader,
            LeaderName = item.LeaderName,
            Member = !string.IsNullOrEmpty(item.Member) ? item.Member.Split(',').ToList() : new List<string>(),
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

    public async Task<IEnumerable<ComboBoxDto>> Handle(CampaignQueryComboBox request, CancellationToken cancellationToken)
    {

        var datas = await _campaignRepository.GetListCbx(request.Status);
        var result = datas.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
