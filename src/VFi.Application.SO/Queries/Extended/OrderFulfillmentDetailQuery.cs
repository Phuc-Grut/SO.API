using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class OrderFulfillmentDetailQueryAll : IQuery<IEnumerable<OrderFulfillmentDetailDto>>
{
    public OrderFulfillmentDetailQueryAll()
    {
    }
}

public class OrderFulfillmentDetailQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public OrderFulfillmentDetailQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class OrderFulfillmentDetailQueryCheckCode : IQuery<bool>
{

    public OrderFulfillmentDetailQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class OrderFulfillmentDetailQueryById : IQuery<OrderFulfillmentDetailDto>
{
    public OrderFulfillmentDetailQueryById()
    {
    }

    public OrderFulfillmentDetailQueryById(Guid OrderFulfillmentDetailId)
    {
        OrderFulfillmentDetailId = OrderFulfillmentDetailId;
    }

    public Guid OrderFulfillmentDetailId { get; set; }
}
public class OrderFulfillmentDetailPagingQuery : FopQuery, IQuery<PagedResult<List<OrderFulfillmentDetailDto>>>
{
    public OrderFulfillmentDetailPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class OrderFulfillmentDetailQueryHandler : IQueryHandler<OrderFulfillmentDetailQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<OrderFulfillmentDetailQueryAll, IEnumerable<OrderFulfillmentDetailDto>>,
                                         IQueryHandler<OrderFulfillmentDetailQueryCheckCode, bool>,
                                         IQueryHandler<OrderFulfillmentDetailQueryById, OrderFulfillmentDetailDto>,
                                         IQueryHandler<OrderFulfillmentDetailPagingQuery, PagedResult<List<OrderFulfillmentDetailDto>>>
{
    private readonly IOrderFulfillmentDetailRepository _repository;
    public OrderFulfillmentDetailQueryHandler(IOrderFulfillmentDetailRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(OrderFulfillmentDetailQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<OrderFulfillmentDetailDto> Handle(OrderFulfillmentDetailQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.OrderFulfillmentDetailId);
        var result = new OrderFulfillmentDetailDto()
        {
            Id = item.Id,
            OrderFulfillmentId = item.OrderFulfillmentId,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Origin = item.Origin,
            WarehouseId = item.WarehouseId,
            WarehouseCode = item.WarehouseCode,
            WarehouseName = item.WarehouseName,
            UnitPrice = item.UnitPrice,
            UnitName = item.UnitName,
            Quantity = item.Quantity,
            DisplayOrder = item.DisplayOrder,
            Note = item.Note,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<OrderFulfillmentDetailDto>>> Handle(OrderFulfillmentDetailPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<OrderFulfillmentDetailDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<OrderFulfillmentDetail>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new OrderFulfillmentDetailDto()
        {
            Id = item.Id,
            OrderFulfillmentId = item.OrderFulfillmentId,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Origin = item.Origin,
            WarehouseId = item.WarehouseId,
            WarehouseCode = item.WarehouseCode,
            WarehouseName = item.WarehouseName,
            UnitPrice = item.UnitPrice,
            UnitName = item.UnitName,
            Quantity = item.Quantity,
            DisplayOrder = item.DisplayOrder,
            Note = item.Note,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<OrderFulfillmentDetailDto>> Handle(OrderFulfillmentDetailQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new OrderFulfillmentDetailDto()
        {
            Id = item.Id,
            OrderFulfillmentId = item.OrderFulfillmentId,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            ProductImage = item.ProductImage,
            Origin = item.Origin,
            WarehouseId = item.WarehouseId,
            WarehouseCode = item.WarehouseCode,
            WarehouseName = item.WarehouseName,
            UnitPrice = item.UnitPrice,
            UnitName = item.UnitName,
            Quantity = item.Quantity,
            DisplayOrder = item.DisplayOrder,
            Note = item.Note,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(OrderFulfillmentDetailQueryComboBox request, CancellationToken cancellationToken)
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
