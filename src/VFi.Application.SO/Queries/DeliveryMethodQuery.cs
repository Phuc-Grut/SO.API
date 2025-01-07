using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;


namespace VFi.Application.SO.Queries;

public class DeliveryMethodQueryById : IQuery<DeliveryMethodDto>
{
    public DeliveryMethodQueryById()
    {
    }

    public DeliveryMethodQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class DeliveryMethodQueryAll : IQuery<IEnumerable<DeliveryMethodDto>>
{
    public DeliveryMethodQueryAll()
    {
    }
}

public class DeliveryMethodQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public DeliveryMethodQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class DeliveryMethodQueryCheckCode : IQuery<bool>
{

    public DeliveryMethodQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class DeliveryMethodPagingQuery : FopQuery, IQuery<PagedResult<List<DeliveryMethodDto>>>
{
    public DeliveryMethodPagingQuery(int? status, string? keyword, string filter, string order, int pageNumber, int pageSize)
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
public class DeliveryMethodQueryHandler : IQueryHandler<DeliveryMethodQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<DeliveryMethodQueryAll, IEnumerable<DeliveryMethodDto>>,
                                         IQueryHandler<DeliveryMethodQueryCheckCode, bool>,
                                         IQueryHandler<DeliveryMethodQueryById, DeliveryMethodDto>,
                                         IQueryHandler<DeliveryMethodPagingQuery, PagedResult<List<DeliveryMethodDto>>>
{
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;
    public DeliveryMethodQueryHandler(IDeliveryMethodRepository deliveryMethodRespository)
    {
        _deliveryMethodRepository = deliveryMethodRespository;
    }
    public async Task<bool> Handle(DeliveryMethodQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _deliveryMethodRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<DeliveryMethodDto> Handle(DeliveryMethodQueryById request, CancellationToken cancellationToken)
    {
        var item = await _deliveryMethodRepository.GetById(request.Id);
        var result = new DeliveryMethodDto()
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
            UpdatedByName = item.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<DeliveryMethodDto>>> Handle(DeliveryMethodPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<DeliveryMethodDto>>();

        var fopRequest = FopExpressionBuilder<DeliveryMethod>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status.ToString());
        }
        var (datas, count) = await _deliveryMethodRepository.Filter(request.Keyword, filter, fopRequest);

        var data = datas.Select(item => new DeliveryMethodDto()
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
            UpdatedByName = item.UpdatedByName,

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<DeliveryMethodDto>> Handle(DeliveryMethodQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _deliveryMethodRepository.GetAll();

        var result = items.Select(item => new DeliveryMethodDto()
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
            UpdatedByName = item.UpdatedByName,

        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(DeliveryMethodQueryComboBox request, CancellationToken cancellationToken)
    {

        var items = await _deliveryMethodRepository.GetListCbx(request.Status);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
