using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using FluentValidation.Results;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.Contracts;
using VFi.Application.SO.Events;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Handler;

internal class ExportWarehouseCommandHandler : CommandHandler,
    IRequestHandler<ExportWarehouseAddCommand, ValidationResult>,
    IRequestHandler<ExportWarehouseEditCommand, ValidationResult>,
    IRequestHandler<ExportWarehouseDeleteCommand, ValidationResult>,
    IRequestHandler<ApprovalExportWarehouseCommand, ValidationResult>,
    IRequestHandler<ExportWarehouseDuplicateCommand, ValidationResult>,
    IRequestHandler<ExportWarehouseAddOrderIdsCommand, ValidationResult>,
    IRequestHandler<UpdateServiceFeesCommand, ValidationResult>
{
    private readonly IExportWarehouseRepository _exportWarehouseRepository;
    private readonly IExportWarehouseProductRepository _exportWarehouseProductRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IExportRepository _exportRepository;
    private readonly IContextUser _context;
    private readonly IWMSRepository _WMSRepository;
    private readonly IOrderServiceAddRepository _orderServiceAddRepository;
    private readonly IServiceAddRepository _serviceAddRepository;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExportWarehouseCommandHandler(
        IExportWarehouseRepository exportWarehouseRepository,
        IExportWarehouseProductRepository exportWarehouseProductRepository,
        IOrderRepository orderRepository,
        IOrderProductRepository orderProductRepository,
        IExportRepository exportRepository,
        IContextUser context,
        IWMSRepository wmsRepository,
        IMediatorHandler mediatorHandler,
        IOrderServiceAddRepository orderServiceAddRepository,
        IServiceAddRepository serviceAddRepository,
        IExchangeRateRepository exchangeRateRepository)
    {
        _exportWarehouseRepository = exportWarehouseRepository;
        _exportWarehouseProductRepository = exportWarehouseProductRepository;
        _orderRepository = orderRepository;
        _orderProductRepository = orderProductRepository;
        _exportRepository = exportRepository;
        _context = context;
        _WMSRepository = wmsRepository;
        _mediatorHandler = mediatorHandler;
        _orderServiceAddRepository = orderServiceAddRepository;
        _serviceAddRepository = serviceAddRepository;
        _exchangeRateRepository = exchangeRateRepository;
    }

    public void Dispose()
    {
        _exportWarehouseRepository.Dispose();
    }


    public async Task<ValidationResult> Handle(ExportWarehouseAddOrderIdsCommand request, CancellationToken cancellationToken)
    {
        if (!await request.IsValidAsync(_exportWarehouseRepository, _exportWarehouseProductRepository, _orderRepository))
        {
            return request.ValidationResult;
        }

        var newProducts = new List<ExportWarehouseProduct>();

        var existingProducts = await _exportWarehouseProductRepository
            .Filter(new Dictionary<string, object> { { "exportWarehouseId", request.Id } });

        var existingOrderIds = existingProducts.Select(x => x.OrderId).ToList();


        var orderIdsToFetch = request.OrderIds.ToList();
        var orderProducts = await _orderProductRepository.GetByOrderIds(orderIdsToFetch);
        //var orderProducts = await _orderRepository.GetByIds(orderIdsToFetch);


        foreach (var orderProduct in orderProducts)
        {
            var newProduct = new ExportWarehouseProduct
            {
                Id = Guid.NewGuid(),
                OrderId = orderProduct.OrderId,
                ExportWarehouseId = request.Id,
                QuantityRequest = (double)(orderProduct.Quantity ?? 0),
                QuantityExported = 0,
                ProductId = orderProduct.ProductId,
                ProductCode = orderProduct.ProductCode,
                ProductName = orderProduct.ProductName,
                OrderCode = orderProduct.Order.Code,
                OrderProductId = orderProduct.Id,
                UnitCode = orderProduct.UnitCode,
                UnitName = orderProduct.UnitName,
                WarehouseCode = orderProduct.WarehouseCode,
                WarehouseName = orderProduct.WarehouseName,
                Note = orderProduct.Order.Description ?? string.Empty
            };
            newProducts.Add(newProduct);
        }

        if (newProducts.Any())
        {
            _exportWarehouseProductRepository.Add(newProducts);
            return await Commit(_exportWarehouseProductRepository.UnitOfWork);
        }
        else
        {
            return new ValidationResult();
        }
    }


    public async Task<ValidationResult> Handle(ExportWarehouseAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_exportWarehouseRepository))
            return request.ValidationResult;
        //var filter = new Dictionary<string, object>();
        //var orderProductId = String.Join(",", request.Detail?.Where(x => x.OrderProductId != null).Select(g => g.OrderProductId.ToString()).ToArray());
        //filter.Add("orderProductId", orderProductId);
        //var detailExport = await _ExportWarehouseProductRepository.Filter(filter);
        //filter.Add("id", orderProductId); 
        //var detailOrder = await _OrderProductRepository.Filter(filter);
        var ExportWarehouse = new ExportWarehouse
        {
            Id = request.Id,
            Code = request.Code,
            OrderId = request.OrderId,
            OrderCode = request.OrderCode,
            CustomerId = request.CustomerId,
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            Description = request.Description,
            WarehouseId = request.WarehouseId,
            WarehouseCode = request.WarehouseCode,
            WarehouseName = request.WarehouseName,
            DeliveryStatus = request.DeliveryStatus,
            DeliveryName = request.DeliveryName,
            DeliveryAddress = request.DeliveryAddress,
            DeliveryCountry = request.DeliveryCountry,
            DeliveryProvince = request.DeliveryProvince,
            DeliveryDistrict = request.DeliveryDistrict,
            DeliveryWard = request.DeliveryWard,
            DeliveryNote = request.DeliveryNote,
            EstimatedDeliveryDate = request.EstimatedDeliveryDate,
            DeliveryMethodId = request.DeliveryMethodId,
            DeliveryMethodName = request.DeliveryMethodName,
            ShippingMethodId = request.ShippingMethodId,
            ShippingMethodName = request.ShippingMethodName,
            Status = request.Status,
            Note = request.Note,
            RequestBy = request.RequestBy,
            RequestByName = request.RequestByName,
            RequestDate = request.RequestDate,
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.Now,
            CreatedByName = request.CreatedByName,
            File = request.File
        };

        _exportWarehouseRepository.Add(ExportWarehouse);
        var result = await Commit(_exportWarehouseRepository.UnitOfWork);
        if (!result.IsValid)
            return result;

        var list = new List<ExportWarehouseProduct>();
        if (request.Detail?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.Detail)
            {
                //if (u.OrderProductId != null )
                //{
                //    var QuantityRequest = 0;
                //    if (detailExport.Count() > 0)
                //    {
                //        QuantityRequest = detailExport.Where(x => x.OrderProductId == u.OrderProductId).Sum(x => x.QuantityRequest.Value);
                //    }
                //    var Quantity = 0;
                //    if (detailOrder.Count() > 0)
                //    {
                //        Quantity = detailOrder.Where(x => x.Id == u.OrderProductId).Select(x => x.Quantity ?? 0).Take(1).SingleOrDefault();
                //    }
                //    if ((QuantityRequest + u.QuantityRequest) > Quantity)
                //    {
                //        var attemptedValue = new List<string>();
                //        attemptedValue.Add("[");
                //        attemptedValue.Add("Second line");
                //        attemptedValue.Add(i.ToString());
                //        attemptedValue.Add("]");
                //        attemptedValue.Add("QuantityRequestDes");
                //        attemptedValue.Add("must<=");
                //        attemptedValue.Add((Quantity - QuantityRequest).ToString());
                //        return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("quantityRequest", "Error Detail", attemptedValue) } };
                //    }
                //}
                list.Add(new ExportWarehouseProduct()
                {
                    Id = Guid.NewGuid(),
                    OrderId = u.OrderId,
                    OrderCode = u.OrderCode,
                    OrderProductId = u.OrderProductId,
                    ExportWarehouseId = request.Id,
                    ProductId = u.ProductId,
                    ProductCode = u.ProductCode,
                    ProductName = u.ProductName,
                    ProductImage = u.ProductImage,
                    WarehouseCode = u.WarehouseCode,
                    WarehouseName = u.WarehouseName,
                    UnitCode = u.UnitCode,
                    UnitName = u.UnitName,
                    QuantityRequest = u.QuantityRequest,
                    QuantityExported = 0,
                    Note = u.Note,
                    DisplayOrder = i
                });
                i++;
            }

            _exportWarehouseProductRepository.Add(list);
            _ = await CommitNoCheck(_exportWarehouseProductRepository.UnitOfWork);
        }

        return result;
    }

    public async Task<ValidationResult> Handle(ExportWarehouseDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_exportWarehouseRepository))
            return request.ValidationResult;
        var ExportWarehouse = new ExportWarehouse
        {
            Id = request.Id
        };
        _exportWarehouseRepository.Remove(ExportWarehouse);
        return await Commit(_exportWarehouseRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExportWarehouseEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;

        var obj = await _exportWarehouseRepository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "FulfillmentRequest is not exist") } };
        }

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "FulfillmentRequest is not exist") } };
        }

        if (!request.IsValid(_exportWarehouseRepository))
            return request.ValidationResult;

        //var filter = new Dictionary<string, object>();
        //var orderProductId = String.Join(",", request.Detail?.Where(x => x.OrderProductId != null).Select(g => g.OrderProductId.ToString()).ToArray());
        //filter.Add("orderProductId", orderProductId);
        //var detailExport = await _ExportWarehouseProductRepository.Filter(filter);
        //filter.Add("id", orderProductId);
        //var detailOrder = await _OrderProductRepository.Filter(filter);

        obj.Id = request.Id;
        obj.Code = request.Code;
        obj.OrderId = request.OrderId;
        obj.OrderCode = request.OrderCode;
        obj.CustomerId = request.CustomerId;
        obj.CustomerCode = request.CustomerCode;
        obj.CustomerName = request.CustomerName;
        obj.Description = request.Description;
        obj.WarehouseId = request.WarehouseId;
        obj.WarehouseCode = request.WarehouseCode;
        obj.WarehouseName = request.WarehouseName;
        obj.DeliveryStatus = request.DeliveryStatus;
        obj.DeliveryName = request.DeliveryName;
        obj.DeliveryAddress = request.DeliveryAddress;
        obj.DeliveryCountry = request.DeliveryCountry;
        obj.DeliveryProvince = request.DeliveryProvince;
        obj.DeliveryDistrict = request.DeliveryDistrict;
        obj.DeliveryWard = request.DeliveryWard;
        obj.DeliveryNote = request.DeliveryNote;
        obj.EstimatedDeliveryDate = request.EstimatedDeliveryDate;
        obj.DeliveryMethodId = request.DeliveryMethodId;
        obj.DeliveryMethodName = request.DeliveryMethodName;
        obj.ShippingMethodId = request.ShippingMethodId;
        obj.ShippingMethodName = request.ShippingMethodName;
        obj.Status = request.Status;
        obj.Note = request.Note;
        obj.RequestBy = request.RequestBy;
        obj.RequestByName = request.RequestByName;
        obj.RequestDate = request.RequestDate;
        obj.UpdatedBy = request.UpdatedBy;
        obj.UpdatedDate = DateTime.Now;
        obj.UpdatedByName = request.UpdatedByName;
        obj.File = request.File;
        //add domain event
        //ExportWarehouse.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _exportWarehouseRepository.Update(obj);
        var result = await Commit(_exportWarehouseRepository.UnitOfWork);
        if (!result.IsValid)
            return result;

        //Chi tiết
        var listAdd = new List<ExportWarehouseProduct>();
        var listUpdate = new List<ExportWarehouseProduct>();
        var listDelete = new List<ExportWarehouseProduct>();
        if (request.Detail?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.Detail)
            {
                var item = obj.ExportWarehouseProduct.Where(x => x.Id == u.Id).FirstOrDefault();
                if (item != null)
                {
                    //if (u.OrderProductId != null)
                    //{
                    //    var QuantityRequest = 0;
                    //    if (detailExport.Count() > 0)
                    //    {
                    //        QuantityRequest = detailExport.Where(x => x.OrderProductId == u.OrderProductId && x.Id != u.Id).Sum(x => x.QuantityRequest.Value);
                    //    }
                    //    var Quantity = 0;
                    //    if (detailOrder.Count() > 0) {
                    //        Quantity= detailOrder.Where(x => x.Id == u.OrderProductId).Select(x => x.Quantity ?? 0).Take(1).SingleOrDefault();
                    //    }
                    //    if ((QuantityRequest+u.QuantityRequest) > Quantity)
                    //    {
                    //        var attemptedValue = new List<string>();
                    //        attemptedValue.Add("[");
                    //        attemptedValue.Add("Second line");
                    //        attemptedValue.Add(i.ToString());
                    //        attemptedValue.Add("]");
                    //        attemptedValue.Add("QuantityRequestDes");
                    //        attemptedValue.Add("must<=");
                    //        attemptedValue.Add((Quantity - QuantityRequest).ToString());
                    //        return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("quantityRequest", "Error Detail", attemptedValue) } };
                    //    }
                    //}
                    item.Id = (Guid)u.Id;
                    item.OrderProductId = u.OrderProductId;
                    item.ExportWarehouseId = request.Id;
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.ProductImage = u.ProductImage;
                    item.WarehouseCode = u.WarehouseCode;
                    item.WarehouseName = u.WarehouseName;
                    item.UnitCode = u.UnitCode;
                    item.UnitName = u.UnitName;
                    item.QuantityRequest = u.QuantityRequest;
                    item.Note = u.Note;
                    item.DisplayOrder = i;
                    listUpdate.Add(item);
                }
                else
                {
                    //if (u.OrderProductId != null)
                    //{
                    //    var QuantityRequest = 0;
                    //    if (detailExport.Count() > 0)
                    //    {
                    //        QuantityRequest = detailExport.Where(x => x.OrderProductId == u.OrderProductId).Sum(x => x.QuantityRequest.Value);
                    //    }
                    //    var Quantity = 0;
                    //    if (detailOrder.Count() > 0)
                    //    {
                    //        Quantity = detailOrder.Where(x => x.Id == u.OrderProductId).Select(x => x.Quantity ?? 0).Take(1).SingleOrDefault();
                    //    }
                    //    if ((QuantityRequest + u.QuantityRequest) > Quantity)
                    //    {
                    //        var attemptedValue = new List<string>();
                    //        attemptedValue.Add("[");
                    //        attemptedValue.Add("Second line");
                    //        attemptedValue.Add(i.ToString());
                    //        attemptedValue.Add("]");
                    //        attemptedValue.Add("QuantityRequestDes");
                    //        attemptedValue.Add("must<=");
                    //        attemptedValue.Add((Quantity - QuantityRequest).ToString());
                    //        return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("quantityRequest", "Error Detail", attemptedValue) } };
                    //    }
                    //}
                    listAdd.Add(new ExportWarehouseProduct()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = u.OrderId,
                        OrderCode = u.OrderCode,
                        OrderProductId = u.OrderProductId,
                        ExportWarehouseId = request.Id,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        ProductImage = u.ProductImage,
                        WarehouseCode = u.WarehouseCode,
                        WarehouseName = u.WarehouseName,
                        UnitCode = u.UnitCode,
                        UnitName = u.UnitName,
                        QuantityRequest = u.QuantityRequest,
                        QuantityExported = 0,
                        Note = u.Note,
                        DisplayOrder = i
                    });
                }

                i++;
            }

            if (listAdd.Count > 0)
            {
                _exportWarehouseProductRepository.Add(listAdd);
            }

            if (listUpdate.Count > 0)
            {
                _exportWarehouseProductRepository.Update(listUpdate);
            }

            if (request.Delete?.Count > 0)
            {
                foreach (var d in request.Delete)
                {
                    var item = obj.ExportWarehouseProduct.Where(x => x.Id == d.Id).SingleOrDefault();
                    if (item is not null)
                    {
                        listDelete.Add(item);
                    }
                }

                _exportWarehouseProductRepository.Remove(listDelete);

                _exportWarehouseProductRepository.Remove(listDelete);
            }

            _ = await CommitNoCheck(_exportWarehouseProductRepository.UnitOfWork);
        }

        return result;
    }

    public async Task<ValidationResult> Handle(ApprovalExportWarehouseCommand request, CancellationToken cancellationToken)
    {
        var item = await _exportWarehouseRepository.GetById(request.Id);
        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "FulfillmentRequest is not exist") } };
        }

        item.Id = request.Id;
        item.Status = request.Status;
        item.ApproveDate = DateTime.Now;
        item.ApproveBy = _context.GetUserId();
        item.ApproveByName = _context.UserClaims.FullName;
        item.ApproveComment = request.ApproveComment;
        if (request.Status == (int)ExportWarehouseEnum.Status.Approved)
        {
            foreach (var _item in request.Details)
            {
                _item.CustomerId = item?.CustomerId;
                _item.CustomerCode = item?.CustomerCode;
                _item.CustomerName = item?.CustomerName;
                _item.IsQC = 0;
                _item.SortOrder = _item.DisplayOrder;
            }

            var obj = new FulfillmentRequestAddExt
            {
                Code = item?.Code,
                IsAuto = 0,
                Type = request.Type,
                WarehouseCode = request.WarehouseCode,
                Sonumber = item.OrderCode,
                CustomerCode = item?.CustomerCode,
                CustomerName = item?.CustomerName,
                CustomerId = item?.CustomerId,
                VendorCode = item?.CustomerCode,
                VendorName = item?.CustomerName,
                VendorId = item?.CustomerId,
                DeliveryName = request.DeliveryName,
                DeliveryAddress = request.DeliveryAddress,
                DeliveryCountry = request.DeliveryCountry,
                DeliveryProvince = request.DeliveryProvince,
                DeliveryDistrict = request.DeliveryDistrict,
                DeliveryWard = request.DeliveryWard,
                DeliveryNote = request.DeliveryNote,
                EstimatedDeliveryDate = request.EstimatedDeliveryDate,
                ShippingMethodCode = request.ShippingMethodCode,
                DeliveryMethodCode = request.DeliveryMethodCode,
                Status = request.WMSStatus,
                Note = request.Note,
                RequestBy = request.RequestBy,
                RequestByName = request.RequestByName,
                Details = request.Details,
                Files = new List<FileRequest>()
            };

            var ob = new FulfillmentRequestAddExtEvent
            {
                Id = request.Id,
                ItemData = obj,
                Tenant = _context.Tenant,
                Data = _context.Data,
                Data_Zone = _context.Data_Zone,
            };
            _ = _mediatorHandler.PublishEvent(ob, PublishStrategy.ParallelNoWait, cancellationToken);
            //if (result.IsValid)
            //{
            //    item.FulfillmentRequestCode = result.RuleSetsExecuted[0];
            //}
        }

        _exportWarehouseRepository.Approve(item);
        return await Commit(_exportWarehouseRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExportWarehouseDuplicateCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_exportWarehouseRepository))
            return request.ValidationResult;
        var actionBy = _context.GetUserId();
        var actionByName = _context.UserClaims.FullName;
        var actionDate = DateTime.Now;
        var item = await _exportWarehouseRepository.GetById(request.Id);
        item.Id = Guid.NewGuid();
        item.Code = request.Code;
        item.Status = 0;
        item.RequestBy = actionBy;
        item.RequestByName = actionByName;
        item.RequestDate = actionDate;
        item.CreatedBy = actionBy;
        item.CreatedByName = actionByName;
        item.CreatedDate = actionDate;

        var list = new List<ExportWarehouseProduct>();
        if (item.ExportWarehouseProduct.Any())
        {
            foreach (var x in item.ExportWarehouseProduct)
            {
                x.Id = Guid.NewGuid();
                x.ExportWarehouseId = item.Id;
                x.QuantityExported = 0;
                list.Add(x);
            }

            item.ExportWarehouseProduct = list;
        }

        _exportWarehouseRepository.Add(item);
        return await Commit(_exportWarehouseRepository.UnitOfWork);
        //var export = await _exportRepository.GetByExportWarehouseId(request.Id);
        //export.Id = Guid.NewGuid();
        //export.Code = request.Code;
        //export.Status = 0;
        //export.CreatedBy = actionBy;
        //export.CreatedByName = actionByName;
        //export.CreatedDate = actionDate;
        //List<ExportProduct> listExport = new List<ExportProduct>();
        //if (export.ExportProducts.Any())
        //{
        //    foreach (var x in export.ExportProducts)
        //    {
        //        x.Id = Guid.NewGuid();
        //        x.ExportId = export.Id;
        //        listExport.Add(x);
        //    }
        //    export.ExportProducts = listExport;
        //}
        //_exportRepository.Add(export);
        //return await Commit(_exportRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(UpdateServiceFeesCommand command, CancellationToken cancellationToken)
    {
        if (!command.IsValid(_exportWarehouseRepository))
            return command.ValidationResult;

        var exportWarehouseProduct = await _exportWarehouseProductRepository.GetByExportWarehouseId(command.Id);
        var orderIds = exportWarehouseProduct.Where(x => x.OrderId is not null).Select(x => (Guid)x.OrderId!).Distinct().ToList();
        var orders = (await _orderRepository.GetByIds(orderIds)).ToList();
        if (!orders.Any())
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("Orders", "Orders is not exist") } };
        }

        var orderHasWeight = orders.Where(x => x.Weight is not null).ToList();
        var orderHasNotWeight = orders.Where(x => x.Weight is null).ToList();

        if (!orderHasNotWeight.Any())
        {
            var totalWeight = orders.Sum(x => x.Weight);
            foreach (var order in orders)
            {
                var priceOrderService = Math.Ceiling((decimal)(order.Weight.Value / totalWeight) * command.PriceTotal);
                await UpdateOrAddOrderServiceAdd(order, command, priceOrderService);
            }
        }
        else
        {
            var totalAmountHasWeight = Math.Ceiling((decimal)orderHasWeight.Count / orders.Count * command.PriceTotal);
            var totalAmountHasNotWeight = command.PriceTotal - totalAmountHasWeight;

            var totalWeight = orderHasWeight.Sum(x => x.Weight);
            foreach (var order in orderHasWeight)
            {
                var priceOrderService = Math.Ceiling((decimal)(order.Weight.Value / totalWeight) * totalAmountHasWeight);
                await UpdateOrAddOrderServiceAdd(order, command, priceOrderService);
            }

            foreach (var order in orderHasNotWeight)
            {
                var priceOrderService = Math.Ceiling(totalAmountHasNotWeight / orderHasNotWeight.Count);
                await UpdateOrAddOrderServiceAdd(order, command, priceOrderService);
            }
        }

        return await Commit(_orderServiceAddRepository.UnitOfWork);
    }

    private async Task UpdateOrAddOrderServiceAdd(Domain.SO.Models.Order order, UpdateServiceFeesCommand command, decimal priceOrderService)
    {
        var orderServiceAdd = order.OrderServiceAdd.FirstOrDefault(x => x.ServiceAddId == command.ServiceAddId);
        decimal? exchangeRate;
        switch (command.ServiceAddCurrencyCode)
        {
            case "VND":
                exchangeRate = 1;
                break;
            case "JPY":
                exchangeRate = order.ExchangeRate;
                break;
            default:
                exchangeRate = (decimal)await _exchangeRateRepository.GetRate(command.ServiceAddCurrencyCode);
                break;
        }

        if (orderServiceAdd is not null)
        {
            orderServiceAdd.Price = priceOrderService;
            orderServiceAdd.UpdatedBy = _context.GetUserId();
            orderServiceAdd.UpdatedDate = DateTime.Now;
            orderServiceAdd.UpdatedByName = _context.UserClaims.FullName;
            orderServiceAdd.ExchangeRate = exchangeRate;
            orderServiceAdd.Currency = command.ServiceAddCurrencyCode;
            _orderServiceAddRepository.Update(orderServiceAdd);
        }
        else
        {
            var serviceAdd = await _serviceAddRepository.GetById(command.ServiceAddId);
            orderServiceAdd = new OrderServiceAdd
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ServiceAddId = command.ServiceAddId,
                ServiceAddName = serviceAdd.Name,
                Currency = command.ServiceAddCurrencyCode,
                Price = priceOrderService,
                CreatedBy = _context.GetUserId(),
                CreatedDate = DateTime.Now,
                CreatedByName = _context.UserClaims.FullName,
                ExchangeRate = exchangeRate
            };
            _orderServiceAddRepository.Add(orderServiceAdd);
        }
    }
}