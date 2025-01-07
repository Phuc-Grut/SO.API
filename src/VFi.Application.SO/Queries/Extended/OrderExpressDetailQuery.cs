using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class OrderExpressDetailQueryAll : IQuery<IEnumerable<OrderExpressDetailDto>>
{
    public OrderExpressDetailQueryAll()
    {
    }
}

public class OrderExpressDetailQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public OrderExpressDetailQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class OrderExpressDetailQueryCheckCode : IQuery<bool>
{

    public OrderExpressDetailQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class OrderExpressDetailQueryById : IQuery<OrderExpressDetailDto>
{
    public OrderExpressDetailQueryById()
    {
    }

    public OrderExpressDetailQueryById(Guid OrderExpressDetailId)
    {
        OrderExpressDetailId = OrderExpressDetailId;
    }

    public Guid OrderExpressDetailId { get; set; }
}
public class OrderExpressDetailPagingQuery : FopQuery, IQuery<PagedResult<List<OrderExpressDetailDto>>>
{
    public OrderExpressDetailPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class OrderExpressDetailQueryHandler : IQueryHandler<OrderExpressDetailQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<OrderExpressDetailQueryAll, IEnumerable<OrderExpressDetailDto>>,
                                         IQueryHandler<OrderExpressDetailQueryCheckCode, bool>,
                                         IQueryHandler<OrderExpressDetailQueryById, OrderExpressDetailDto>,
                                         IQueryHandler<OrderExpressDetailPagingQuery, PagedResult<List<OrderExpressDetailDto>>>
{
    private readonly IOrderExpressDetailRepository _repository;
    public OrderExpressDetailQueryHandler(IOrderExpressDetailRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(OrderExpressDetailQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<OrderExpressDetailDto> Handle(OrderExpressDetailQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.OrderExpressDetailId);
        var result = new OrderExpressDetailDto()
        {
            Id = item.Id,
            OrderExpressId = item.OrderExpressId,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Origin = item.Origin,
            UnitName = item.UnitName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            DisplayOrder = item.DisplayOrder,
            Note = item.Note,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            CommodityGroup = item.CommodityGroup,
            SurchargeGroup = item.SurchargeGroup,
            Surcharge = item.Surcharge
        };
        return result;
    }

    public async Task<PagedResult<List<OrderExpressDetailDto>>> Handle(OrderExpressDetailPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<OrderExpressDetailDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<OrderExpressDetail>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new OrderExpressDetailDto()
        {
            Id = item.Id,
            OrderExpressId = item.OrderExpressId,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Origin = item.Origin,
            UnitName = item.UnitName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            DisplayOrder = item.DisplayOrder,
            Note = item.Note,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            CommodityGroup = item.CommodityGroup,
            SurchargeGroup = item.SurchargeGroup,
            Surcharge = item.Surcharge
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<OrderExpressDetailDto>> Handle(OrderExpressDetailQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new OrderExpressDetailDto()
        {
            Id = item.Id,
            OrderExpressId = item.OrderExpressId,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Origin = item.Origin,
            UnitName = item.UnitName,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            DisplayOrder = item.DisplayOrder,
            Note = item.Note,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            CommodityGroup = item.CommodityGroup,
            SurchargeGroup = item.SurchargeGroup,
            Surcharge = item.Surcharge
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(OrderExpressDetailQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.ProductName,
            Value = x.Id,
            Label = x.ProductName
        });
        return result;
    }
}
