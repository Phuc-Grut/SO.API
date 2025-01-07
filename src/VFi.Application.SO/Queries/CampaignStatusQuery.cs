using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class CampaignStatusQueryAll : IQuery<IEnumerable<CampaignStatusDto>>
{
    public CampaignStatusQueryAll()
    {
    }
}

public class CampaignStatusQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public CampaignStatusQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class CampaignStatusQueryCheckCode : IQuery<bool>
{

    public CampaignStatusQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class CampaignStatusQueryById : IQuery<CampaignStatusDto>
{
    public CampaignStatusQueryById()
    {
    }

    public CampaignStatusQueryById(Guid campaignStatusId)
    {
        CampaignStatusId = campaignStatusId;
    }

    public Guid CampaignStatusId { get; set; }
}
public class CampaignStatusPagingQuery : FopQuery, IQuery<PagedResult<List<CampaignStatusDto>>>
{
    public CampaignStatusPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize, Guid? campaignId)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
        CampaignId = campaignId;
    }

    public Guid? CampaignId { get; set; }
}

public class CampaignStatusQueryHandler : IQueryHandler<CampaignStatusQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<CampaignStatusQueryAll, IEnumerable<CampaignStatusDto>>,
                                         IQueryHandler<CampaignStatusQueryById, CampaignStatusDto>,
                                         IQueryHandler<CampaignStatusPagingQuery, PagedResult<List<CampaignStatusDto>>>
{
    private readonly ICampaignStatusRepository _CampaignStatusRepository;
    public CampaignStatusQueryHandler(ICampaignStatusRepository CampaignStatusRespository)
    {
        _CampaignStatusRepository = CampaignStatusRespository;
    }

    public async Task<CampaignStatusDto> Handle(CampaignStatusQueryById request, CancellationToken cancellationToken)
    {
        var item = await _CampaignStatusRepository.GetById(request.CampaignStatusId);
        var result = new CampaignStatusDto()
        {
            Id = item.Id,
            CampaignId = item.CampaignId,
            Name = item.Name,
            Color = item.Color,
            Description = item.Description,
            IsDefault = item.IsDefault,
            IsClose = item.IsClose,
            Status = item.Status,
            DisplayOrder = item.DisplayOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<CampaignStatusDto>>> Handle(CampaignStatusPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<CampaignStatusDto>>();

        var fopRequest = FopExpressionBuilder<CampaignStatus>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _CampaignStatusRepository.Filter(request.Keyword, request.Status, request.CampaignId, fopRequest);
        var data = datas.Select(item => new CampaignStatusDto()
        {
            Id = item.Id,
            CampaignId = item.CampaignId,
            Name = item.Name,
            Color = item.Color,
            TextColor = item.TextColor,
            Description = item.Description,
            IsDefault = item.IsDefault,
            IsClose = item.IsClose,
            Status = item.Status,
            DisplayOrder = item.DisplayOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<CampaignStatusDto>> Handle(CampaignStatusQueryAll request, CancellationToken cancellationToken)
    {
        var datas = await _CampaignStatusRepository.GetAll();
        var result = datas.Select(item => new CampaignStatusDto()
        {
            Id = item.Id,
            CampaignId = item.CampaignId,
            Name = item.Name,
            Color = item.Color,
            Description = item.Description,
            IsDefault = item.IsDefault,
            IsClose = item.IsClose,
            Status = item.Status,
            DisplayOrder = item.DisplayOrder,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(CampaignStatusQueryComboBox request, CancellationToken cancellationToken)
    {

        var datas = await _CampaignStatusRepository.GetListCbx(request.Status);
        var result = datas.Select(x => new ComboBoxDto()
        {
            Key = x.Name,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
