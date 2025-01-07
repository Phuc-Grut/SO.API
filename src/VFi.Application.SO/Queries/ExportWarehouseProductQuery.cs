using Consul;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace VFi.Application.SO.Queries;

public class ExportWarehouseProductPagingQuery : FopQuery, IQuery<PagedResult<List<ExportWarehouseProductDto>>>
{
    public ExportWarehouseProductPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class ExportWarehouseProductQueryById : IQuery<ExportWarehouseProductDto>
{
    public ExportWarehouseProductQueryById()
    {
    }

    public ExportWarehouseProductQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class ExportWarehouseProductQueryHandler : IQueryHandler<ExportWarehouseProductPagingQuery, PagedResult<List<ExportWarehouseProductDto>>>
{
    private readonly IExportWarehouseProductRepository _ExportWarehouseProductRepository;
    private readonly IPIMRepository _pimRepository;

    public ExportWarehouseProductQueryHandler(IExportWarehouseProductRepository exportWarehouseProductRepository, IPIMRepository PimRepository)
    {
        _ExportWarehouseProductRepository = exportWarehouseProductRepository;
        _pimRepository = PimRepository;
    }

    public async Task<PagedResult<List<ExportWarehouseProductDto>>> Handle(ExportWarehouseProductPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ExportWarehouseProductDto>>();
        var fopRequest = FopExpressionBuilder<ExportWarehouseProduct>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        var (ExportWarehousesProduct, count) = await _ExportWarehouseProductRepository.Filter(request.Keyword, filter, fopRequest);
        var dataInventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = ExportWarehousesProduct.Select(x => x.ProductId).Distinct();
        foreach (var x in source)
        {
            listProductId.Add(x);
            if (listProductId.Count() == 50)
            {
                listId.Add(string.Join(",", listProductId));
                listProductId.RemoveRange(0, 50);
            }
            if (i == source.Count() && listProductId.Count() > 0)
            {
                listId.Add(string.Join(",", listProductId));
            }
            i++;
        }
        foreach (var o in listId)
        {
            var inventory = await _pimRepository.GetInventoryDetail(o);
            foreach (var item in inventory)
            {
                var rs = new INVENTORY_DETAIL_BY_LISTID()
                {
                    Id = item.Id,
                    WarehouseId = item.WarehouseId,
                    Code = item.Code,
                    Name = item.Name,
                    ProductId = item.ProductId,
                    StockQuantity = item.StockQuantity,
                    ReservedQuantity = item.ReservedQuantity,
                    PlannedQuantity = item.PlannedQuantity
                };
                dataInventory.Add(rs);
            }
        }
        var data = ExportWarehousesProduct.Select(x => new ExportWarehouseProductDto()
        {
            Id = x.Id,
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            OrderProductId = x.OrderProductId,
            ExportWarehouseId = x.ExportWarehouseId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            WarehouseCode = x.WarehouseCode,
            WarehouseName = x.WarehouseName,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            StockQuantity = !String.IsNullOrEmpty(x.WarehouseCode) ? dataInventory.Where(y => y.ProductId == x.ProductId && y.Code == x.WarehouseCode).FirstOrDefault()?.StockQuantity : dataInventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
            QuantityRequest = x.QuantityRequest,
            QuantityExported = x.QuantityExported,
            Note = x.Note,
            DisplayOrder = x.DisplayOrder

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
}
