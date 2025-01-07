using System.ComponentModel;
using System.Data;
using System.Runtime;
using Consul.Filtering;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using static MassTransit.Logging.LogCategoryName;

namespace VFi.Application.SO.Queries;


public class ProductionOrdersDetailQueryAll : IQuery<IEnumerable<ProductionOrdersDetailDto>>
{
    public ProductionOrdersDetailQueryAll()
    {
    }
}

public class ProductionOrdersDetailQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public ProductionOrdersDetailQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class ProductionOrdersDetailQueryCheckCode : IQuery<bool>
{

    public ProductionOrdersDetailQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class ProductionOrdersDetailQueryById : IQuery<ProductionOrdersDetailDto>
{
    public ProductionOrdersDetailQueryById()
    {
    }

    public ProductionOrdersDetailQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class ProductionOrdersDetailQueryByCode : IQuery<ProductionOrdersDetailDto>
{
    public ProductionOrdersDetailQueryByCode()
    {
    }

    public ProductionOrdersDetailQueryByCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class ProductionOrdersDetailPagingFilterQuery : FopQuery, IQuery<PagedResult<List<ProductionOrdersDetailDto>>>
{
    public ProductionOrdersDetailPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize, int? type, int? productOrderStatus)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Type = type;
        ProductOrderStatus = productOrderStatus;
    }
    public string Keyword { get; set; }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Type { get; set; }
    public int? ProductOrderStatus { get; set; }
}
public class ProductionOrdersDetailPagingFilterQueryCountTotal : FopQuery, IQuery<PagedResult<List<ProductionOrdersDetailCountTotalDto>>>
{
    public ProductionOrdersDetailPagingFilterQueryCountTotal(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public string Keyword { get; set; }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

public class ProductionOrdersDetailQueryHandler : IQueryHandler<ProductionOrdersDetailQueryById, ProductionOrdersDetailDto>,
                                                  IQueryHandler<ProductionOrdersDetailPagingFilterQuery, PagedResult<List<ProductionOrdersDetailDto>>>,
                                                  IQueryHandler<ProductionOrdersDetailPagingFilterQueryCountTotal, PagedResult<List<ProductionOrdersDetailCountTotalDto>>>

{
    private readonly IProductionOrdersDetailRepository _ProductionOrdersDetailRepository;
    private readonly ISOContextProcedures _SOContextProcedures;

    public ProductionOrdersDetailQueryHandler(
        IProductionOrdersDetailRepository ProductionOrdersDetailRepository,
        ISOContextProcedures mesContextProcedures
     )
    {
        _ProductionOrdersDetailRepository = ProductionOrdersDetailRepository;
        _SOContextProcedures = mesContextProcedures;
    }

    public async Task<ProductionOrdersDetailDto> Handle(ProductionOrdersDetailQueryById request, CancellationToken cancellationToken)
    {
        var data = await _ProductionOrdersDetailRepository.GetById(request.Id);

        var result = new ProductionOrdersDetailDto()
        {
            Id = data.Id,
            OrderId = data.OrderId,
            OrderCode = data.OrderCode,
            ProductionOrdersId = data.ProductionOrdersId,
            ProductId = data.ProductId,
            ProductCode = data.ProductCode,
            ProductName = data.ProductName,
            ProductImage = data.ProductImage,
            Sku = data.Sku,
            Gtin = data.Gtin,
            Origin = data.Origin,
            UnitType = data.UnitType,
            UnitCode = data.UnitCode,
            UnitName = data.UnitName,
            Quantity = data.Quantity,
            Note = data.Note,
            EstimatedDeliveryQuantity = data.EstimatedDeliveryQuantity,
            DeliveryDate = data.DeliveryDate,
            IsWorkOrdered = data.IsWorkOrdered,
            ProductionOrdersCode = data.ProductionOrdersCode,
            IsEstimated = data.IsEstimated,
            IsBom = data.IsBom,
            Status = data.Status,
            EstimatedDate = data.EstimatedDate,
            EstimateStatus = data.EstimateStatus,
            Solution = data.Solution,
            Transport = data.Transport,
            Height = data.Height,
            Package = data.Package,
            Volume = data.Volume,
            Length = data.Length,
            Weight = data.Weight,
            Width = data.Width,
            CancelReason = data.CancelReason,
            DisplayOrder = data.DisplayOrder,
            CreatedDate = data.CreatedDate,
            UpdatedDate = data.UpdatedDate,
            CreatedBy = data.CreatedBy,
            UpdatedBy = data.UpdatedBy,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<ProductionOrdersDetailDto>>> Handle(ProductionOrdersDetailPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ProductionOrdersDetailDto>>();
        var fopRequest = FopExpressionBuilder<ProductionOrdersDetail>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var (datas, count) = await _ProductionOrdersDetailRepository.Filter(request.Keyword, fopRequest, request.Type, request.ProductOrderStatus);
        var result = datas.Select(item =>
        {
            return new ProductionOrdersDetailDto()
            {
                Id = item.Id,
                OrderId = item.OrderId,
                OrderCode = item.OrderCode,
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                ProductImage = item.ProductImage,
                Sku = item.Sku,
                Gtin = item.Gtin,
                Origin = item.Origin,
                UnitType = item.UnitType,
                UnitCode = item.UnitCode,
                UnitName = item.UnitName,
                Quantity = item.Quantity,
                Note = item.Note,
                EstimatedDeliveryQuantity = item.EstimatedDeliveryQuantity,
                DeliveryDate = item.DeliveryDate,
                IsWorkOrdered = item.IsWorkOrdered,
                ProductionOrdersCode = item.ProductionOrdersCode,
                IsEstimated = item.IsEstimated,
                IsBom = item.IsBom,
                Status = item.Status,
                EstimatedDate = item.EstimatedDate,
                EstimateStatus = item.EstimateStatus,
                Solution = item.Solution,
                Transport = item.Transport,
                Height = item.Height,
                Package = item.Package,
                Volume = item.Volume,
                Length = item.Length,
                Weight = item.Weight,
                Width = item.Width,
                CancelReason = item.CancelReason,
                DisplayOrder = item.DisplayOrder,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                CreatedBy = item.CreatedBy,
                UpdatedBy = item.UpdatedBy,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName,
                CustomerName = item.ProductionOrder.CustomerName,
                CustomerId = item.ProductionOrder.CustomerId,
                CustomerCode = item.ProductionOrder.CustomerCode,
                EstimateDate = item.ProductionOrder.EstimateDate,
            };
        }
        ).OrderBy(x => x.DisplayOrder).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<ProductionOrdersDetailCountTotalDto>>> Handle(ProductionOrdersDetailPagingFilterQueryCountTotal request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ProductionOrdersDetailCountTotalDto>>();
        var fopRequest = FopExpressionBuilder<ProductionOrdersDetail>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var (datas, count) = await _ProductionOrdersDetailRepository.Filter(request.Keyword, fopRequest);
        string p = string.Join(',', datas.Select(x => x.Id.ToString()).ToList());
        var log = await _SOContextProcedures.SP_GET_TOTAL_COSTESTIMATEAsync(p);
        var result = datas.Select(item =>
        {
            var find = log.Where(x => x.Id == item.Id).FirstOrDefault();
            return new ProductionOrdersDetailCountTotalDto()
            {
                Id = item.Id,
                OrderId = item.OrderId,
                OrderCode = item.OrderCode,
                ProductionOrdersId = item.ProductionOrdersId,
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                ProductImage = item.ProductImage,
                Sku = item.Sku,
                Gtin = item.Gtin,
                Origin = item.Origin,
                UnitType = item.UnitType,
                UnitCode = item.UnitCode,
                UnitName = item.UnitName,
                Quantity = item.Quantity,
                Note = item.Note,
                EstimatedDeliveryQuantity = item.EstimatedDeliveryQuantity,
                DeliveryDate = item.DeliveryDate,
                IsWorkOrdered = item.IsWorkOrdered,
                IsEstimated = item.IsEstimated,
                EstimatedDate = item.EstimatedDate,
                EstimateStatus = item.EstimateStatus,
                Solution = item.Solution,
                Transport = item.Transport,
                Height = item.Height,
                Package = item.Package,
                Volume = item.Volume,
                Length = item.Length,
                Weight = item.Weight,
                Width = item.Width,
                CancelReason = item.CancelReason,
                DisplayOrder = item.DisplayOrder,
                CreatedDate = item.CreatedDate,
                UpdatedDate = item.UpdatedDate,
                CreatedBy = item.CreatedBy,
                UpdatedBy = item.UpdatedBy,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName,
                ProductionOrdersCode = item.ProductionOrder.Code,
                CustomerName = item.ProductionOrder.CustomerName,
                CustomerId = item.ProductionOrder.CustomerId,
                CustomerCode = item.ProductionOrder.CustomerCode,
                EstimateDate = item.ProductionOrder.EstimateDate,
                Total = find != null ? find.Total : 0,
            };
        }).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

}
