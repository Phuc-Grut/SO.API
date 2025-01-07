using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;


namespace VFi.Application.SO.Queries;

public class SalesChannelQueryById : IQuery<SalesChannelDto>
{
    public SalesChannelQueryById()
    {
    }

    public SalesChannelQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class SalesChannelQueryAll : IQuery<IEnumerable<SalesChannelDto>>
{
    public SalesChannelQueryAll()
    {
    }
}

public class SalesChannelQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public SalesChannelQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class SalesChannelQueryCheckCode : IQuery<bool>
{

    public SalesChannelQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class SalesChannelPagingQuery : FopQuery, IQuery<PagedResult<List<SalesChannelDto>>>
{
    public SalesChannelPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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
// lấy dữ liệu ,khai báo
public class SalesChannelQueryHandler : IQueryHandler<SalesChannelQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<SalesChannelQueryAll, IEnumerable<SalesChannelDto>>,
                                         IQueryHandler<SalesChannelQueryCheckCode, bool>,
                                         IQueryHandler<SalesChannelQueryById, SalesChannelDto>,
                                         IQueryHandler<SalesChannelPagingQuery, PagedResult<List<SalesChannelDto>>>
{
    private readonly ISalesChannelRepository _salesChannelRepository;
    public SalesChannelQueryHandler(ISalesChannelRepository salesChannelRespository)
    {
        _salesChannelRepository = salesChannelRespository;
    }
    public async Task<bool> Handle(SalesChannelQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _salesChannelRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<SalesChannelDto> Handle(SalesChannelQueryById request, CancellationToken cancellationToken)
    {
        var salesChannel = await _salesChannelRepository.GetById(request.Id);
        var result = new SalesChannelDto()
        {
            Id = salesChannel.Id,
            Code = salesChannel.Code,
            Name = salesChannel.Name,
            Description = salesChannel.Description,
            Status = salesChannel.Status,
            IsDefault = salesChannel.IsDefault,
            DisplayOrder = salesChannel.DisplayOrder,
            CreatedDate = salesChannel.CreatedDate,
            UpdatedDate = salesChannel.UpdatedDate,
            CreatedBy = salesChannel.CreatedBy,
            UpdatedBy = salesChannel.UpdatedBy,
            CreatedByName = salesChannel.CreatedByName,
            UpdatedByName = salesChannel.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<SalesChannelDto>>> Handle(SalesChannelPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<SalesChannelDto>>();

        var fopRequest = FopExpressionBuilder<SalesChannel>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (SalesChannels, count) = await _salesChannelRepository.Filter(request.Keyword, request.Status, fopRequest);

        var data = SalesChannels.Select(salesChannel => new SalesChannelDto()
        {
            Id = salesChannel.Id,
            Code = salesChannel.Code,
            Name = salesChannel.Name,
            Description = salesChannel.Description,
            Status = salesChannel.Status,
            IsDefault = salesChannel.IsDefault,
            DisplayOrder = salesChannel.DisplayOrder,
            CreatedDate = salesChannel.CreatedDate,
            UpdatedDate = salesChannel.UpdatedDate,
            CreatedBy = salesChannel.CreatedBy,
            UpdatedBy = salesChannel.UpdatedBy,
            CreatedByName = salesChannel.CreatedByName,
            UpdatedByName = salesChannel.UpdatedByName,

        }).OrderBy(x => x.DisplayOrder).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<SalesChannelDto>> Handle(SalesChannelQueryAll request, CancellationToken cancellationToken)
    {
        var SalesChannels = await _salesChannelRepository.GetAll();

        var result = SalesChannels.Select(salesChannel => new SalesChannelDto()
        {
            Id = salesChannel.Id,
            Code = salesChannel.Code,
            Name = salesChannel.Name,
            Description = salesChannel.Description,
            Status = salesChannel.Status,
            IsDefault = salesChannel.IsDefault,
            DisplayOrder = salesChannel.DisplayOrder,
            CreatedDate = salesChannel.CreatedDate,
            UpdatedDate = salesChannel.UpdatedDate,
            CreatedBy = salesChannel.CreatedBy,
            UpdatedBy = salesChannel.UpdatedBy,
            CreatedByName = salesChannel.CreatedByName,
            UpdatedByName = salesChannel.UpdatedByName,

        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(SalesChannelQueryComboBox request, CancellationToken cancellationToken)
    {

        var salesChannels = await _salesChannelRepository.GetListCbx(request.Status);
        var result = salesChannels.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
