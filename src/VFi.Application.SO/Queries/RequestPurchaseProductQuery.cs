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


public class RequestPurchaseProductPagingQuery : FopQuery, IQuery<PagedResult<List<RequestPurchaseProductDto>>>
{
    public RequestPurchaseProductPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Status = status;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RequestPurchaseProductQueryHandler : IQueryHandler<RequestPurchaseProductPagingQuery, PagedResult<List<RequestPurchaseProductDto>>>
{
    private readonly IRequestPurchaseProductRepository _RequestPurchaseProductRepository;
    private readonly IPIMRepository _pimRepository;

    public RequestPurchaseProductQueryHandler(IRequestPurchaseProductRepository requestPurchaseProductRepository, IPIMRepository PimRepository)
    {
        _RequestPurchaseProductRepository = requestPurchaseProductRepository;
        _pimRepository = PimRepository;
    }

    public async Task<PagedResult<List<RequestPurchaseProductDto>>> Handle(RequestPurchaseProductPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RequestPurchaseProductDto>>();
        var fopRequest = FopExpressionBuilder<RequestPurchaseProduct>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        var (RequestPurchaseProduct, count) = await _RequestPurchaseProductRepository.Filter(request.Keyword, filter, fopRequest);
        var dataInventory = new List<INVENTORY_DETAIL_BY_LISTID>();
        var listId = new List<string>();
        var listProductId = new List<Guid?>();
        var i = 1;
        var source = RequestPurchaseProduct.Select(x => x.ProductId).Distinct();
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
        var data = RequestPurchaseProduct.Select(x => new RequestPurchaseProductDto()
        {
            Id = x.Id,
            RequestPurchaseId = x.RequestPurchaseId,
            OrderId = x.OrderId,
            OrderCode = x.OrderCode,
            OrderProductId = x.OrderProductId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            ProductImage = x.ProductImage,
            Origin = x.Origin,
            UnitType = x.UnitType,
            UnitCode = x.UnitCode,
            UnitName = x.UnitName,
            StockQuantity = dataInventory.Where(y => y.ProductId == x.ProductId).Sum(y => y.StockQuantity),
            QuantityRequest = x.QuantityRequest,
            QuantityApproved = x.QuantityApproved,
            UnitPrice = x.UnitPrice,
            Currency = x.Currency,
            DeliveryDate = x.DeliveryDate,
            PriorityLevel = x.PriorityLevel,
            Note = x.Note,
            VendorCode = x.VendorCode,
            VendorName = x.VendorName,
            Status = x.Status,
            StatusPurchase = x.StatusPurchase,
            QuantityPurchased = x.QuantityPurchased,
            DisplayOrder = x.DisplayOrder

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
}
