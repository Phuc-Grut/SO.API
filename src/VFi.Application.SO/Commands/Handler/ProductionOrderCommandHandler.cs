using System.Collections.Generic;
using System.IO.Packaging;
using System.Xml.Linq;
using Consul;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using FluentValidation.Results;
using Flurl.Util;
using MassTransit.Components;
using MassTransit.Internals;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using static MassTransit.Logging.LogCategoryName;

namespace VFi.Application.SO.Commands;

internal class ProductionOrderCommandHandler : CommandHandler,
    IRequestHandler<ProductionOrderAddCommand, ValidationResult>,
    IRequestHandler<ProductionOrderDeleteCommand, ValidationResult>,
    IRequestHandler<ProductionOrderEditCommand, ValidationResult>,
    IRequestHandler<ProductionOrdersConfirmCommand, ValidationResult>
{
    private readonly IProductionOrderRepository _repository;
    private readonly IProductionOrdersDetailRepository _ProductionOrdersDetailRepository;
    private readonly IContextUser _context;
    private readonly IMESRepository _MESRepository;

    public ProductionOrderCommandHandler(
        IContextUser contextUser,
        IProductionOrderRepository ProductionOrderRepository,
        IProductionOrdersDetailRepository ProductionOrdersDetailRepository,
        IMESRepository MESRepository
        )
    {
        _context = contextUser;
        _repository = ProductionOrderRepository;
        _ProductionOrdersDetailRepository = ProductionOrdersDetailRepository;
        _MESRepository = MESRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ProductionOrderAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var ProductionOrder = new ProductionOrder
        {
            Id = request.Id,
            Code = request.Code,
            Note = request.Note,
            Status = request.Status,
            RequestDate = request.RequestDate,
            CustomerId = request.CustomerId,
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            EmployeeId = request.EmployeeId,
            EmployeeCode = request.EmployeeCode,
            EmployeeName = request.EmployeeName,
            DateNeed = request.DateNeed,
            OrderId = request.OrderId,
            OrderNumber = request.OrderNumber,
            SaleEmployeeId = request.SaleEmployeeId,
            SaleEmployeeCode = request.SaleEmployeeCode,
            SaleEmployeeName = request.SaleEmployeeName,
            Type = request.Type,
            EstimateDate = request.EstimateDate,
            File = request.File,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName,
        };
        _repository.Add(ProductionOrder);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.ListProductionOrdersDetail?.Count > 0)
        {
            List<ProductionOrdersDetail> list = new List<ProductionOrdersDetail>();
            var i = 1;
            foreach (var u in request.ListProductionOrdersDetail)
            {
                var detail = new ProductionOrdersDetail()
                {
                    Id = u.Id,
                    ProductionOrdersId = u.ProductionOrdersId,
                    OrderId = u.OrderId,
                    OrderCode = u.OrderCode,
                    OrderProductId = u.OrderProductId,
                    ProductId = u.ProductId,
                    ProductCode = u.ProductCode,
                    ProductName = u.ProductName,
                    ProductImage = u.ProductImage,
                    Sku = u.Sku,
                    Gtin = u.Gtin,
                    Origin = u.Origin,
                    UnitType = u.UnitType,
                    UnitCode = u.UnitCode,
                    UnitName = u.UnitName,
                    Quantity = u.Quantity,
                    Note = u.Note,
                    EstimatedDeliveryQuantity = u.EstimatedDeliveryQuantity,
                    DeliveryDate = u.DeliveryDate,
                    IsWorkOrdered = u.IsWorkOrdered,
                    ProductionOrdersCode = u.ProductionOrdersCode,
                    IsEstimated = u.IsEstimated,
                    IsBom = u.IsBom,
                    Status = u.Status,
                    EstimatedDate = u.EstimatedDate,
                    EstimateStatus = u.EstimateStatus,
                    Solution = u.Solution,
                    Transport = u.Transport,
                    Height = u.Height,
                    Package = u.Package,
                    Volume = u.Volume,
                    Length = u.Length,
                    Weight = u.Weight,
                    Width = u.Width,
                    CancelReason = u.CancelReason,
                    DisplayOrder = i,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now,
                    CreatedByName = createName
                };
                list.Add(detail);
                i++;
            }
            _ProductionOrdersDetailRepository.Add(list);
            _ = await CommitNoCheck(_ProductionOrdersDetailRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(ProductionOrderDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var ProductionOrder = new ProductionOrder
        {
            Id = request.Id
        };

        //add domain event
        //ProductionOrder.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Remove(ProductionOrder);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ProductionOrderEditCommand request, CancellationToken cancellationToken)
    {
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var data = await _repository.GetById(request.Id);
        data.Code = request.Code;
        data.Note = request.Note;
        data.Status = request.Status;
        data.RequestDate = request.RequestDate;
        data.CustomerId = request.CustomerId;
        data.CustomerCode = request.CustomerCode;
        data.CustomerName = request.CustomerName;
        data.Email = request.Email;
        data.Phone = request.Phone;
        data.Address = request.Address;
        data.EmployeeId = request.EmployeeId;
        data.EmployeeCode = request.EmployeeCode;
        data.EmployeeName = request.EmployeeName;
        data.DateNeed = request.DateNeed;
        data.OrderId = request.OrderId;
        data.OrderNumber = request.OrderNumber;
        data.SaleEmployeeId = request.SaleEmployeeId;
        data.SaleEmployeeCode = request.SaleEmployeeCode;
        data.SaleEmployeeName = request.SaleEmployeeName;
        data.Type = request.Type;
        data.EstimateDate = request.EstimateDate;
        data.File = request.File;
        data.UpdatedBy = updatedBy;
        data.UpdatedDate = updatedDate;
        data.UpdatedByName = updateName;

        _repository.Update(data);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        if (request.DeleteProductionOrdersDetail?.Count > 0)
        {
            List<ProductionOrdersDetail> list = new List<ProductionOrdersDetail>();
            foreach (var d in request.DeleteProductionOrdersDetail)
            {
                var item = await _ProductionOrdersDetailRepository.GetById(d.Id);
                if (item is not null)
                {
                    list.Add(item);
                }
            }
            _ProductionOrdersDetailRepository.Remove(list);
            _ = await CommitNoCheck(_ProductionOrdersDetailRepository.UnitOfWork);
        }
        if (request.ListProductionOrdersDetail?.Count > 0)
        {
            List<ProductionOrdersDetail> listAdd = new List<ProductionOrdersDetail>();
            List<ProductionOrdersDetail> listUpdate = new List<ProductionOrdersDetail>();
            var i = 1;
            foreach (var u in request.ListProductionOrdersDetail)
            {
                var item = await _ProductionOrdersDetailRepository.GetById((Guid)u.Id);
                if (item is not null)
                {
                    item.OrderId = u.OrderId;
                    item.OrderCode = u.OrderCode;
                    item.OrderProductId = u.OrderProductId;
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.ProductImage = u.ProductImage;
                    item.Sku = u.Sku;
                    item.Gtin = u.Gtin;
                    item.Origin = u.Origin;
                    item.UnitType = u.UnitType;
                    item.UnitCode = u.UnitCode;
                    item.UnitName = u.UnitName;
                    item.Quantity = u.Quantity;
                    item.Note = u.Note;
                    item.EstimatedDeliveryQuantity = u.EstimatedDeliveryQuantity;
                    item.DeliveryDate = u.DeliveryDate;
                    item.IsWorkOrdered = u.IsWorkOrdered;
                    item.ProductionOrdersCode = u.ProductionOrdersCode;
                    item.IsEstimated = u.IsEstimated;
                    item.IsBom = u.IsBom;
                    item.Status = u.Status;
                    item.EstimatedDate = u.EstimatedDate;
                    item.EstimateStatus = u.EstimateStatus;
                    item.Solution = u.Solution;
                    item.Transport = u.Transport;
                    item.Height = u.Height;
                    item.Package = u.Package;
                    item.Volume = u.Volume;
                    item.Length = u.Length;
                    item.Weight = u.Weight;
                    item.Width = u.Width;
                    item.CancelReason = u.CancelReason;
                    item.DisplayOrder = i;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedByName = updateName;
                    listUpdate.Add(item);
                }
                else
                {
                    listAdd.Add(new ProductionOrdersDetail()
                    {
                        Id = (Guid)u.Id,
                        ProductionOrdersId = u.ProductionOrdersId,
                        OrderId = u.OrderId,
                        OrderCode = u.OrderCode,
                        OrderProductId = u.OrderProductId,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        ProductImage = u.ProductImage,
                        Sku = u.Sku,
                        Gtin = u.Gtin,
                        Origin = u.Origin,
                        UnitType = u.UnitType,
                        UnitCode = u.UnitCode,
                        UnitName = u.UnitName,
                        Quantity = u.Quantity,
                        Note = u.Note,
                        EstimatedDeliveryQuantity = u.EstimatedDeliveryQuantity,
                        DeliveryDate = u.DeliveryDate,
                        IsWorkOrdered = u.IsWorkOrdered,
                        ProductionOrdersCode = u.ProductionOrdersCode,
                        IsEstimated = u.IsEstimated,
                        IsBom = u.IsBom,
                        Status = u.Status,
                        EstimatedDate = u.EstimatedDate,
                        EstimateStatus = u.EstimateStatus,
                        Solution = u.Solution,
                        Transport = u.Transport,
                        Height = u.Height,
                        Package = u.Package,
                        Volume = u.Volume,
                        Length = u.Length,
                        Weight = u.Weight,
                        Width = u.Width,
                        CancelReason = u.CancelReason,
                        DisplayOrder = i,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.Now,
                        CreatedByName = updateName,
                    });
                }
                i++;
            }
            if (listAdd.Count > 0)
            {
                _ProductionOrdersDetailRepository.Add(listAdd);
            }
            if (listUpdate.Count > 0)
            {
                _ProductionOrdersDetailRepository.Update(listUpdate);
            }
            _ = await CommitNoCheck(_ProductionOrdersDetailRepository.UnitOfWork);
        }

        return result;
    }
    public async Task<ValidationResult> Handle(ProductionOrdersConfirmCommand request, CancellationToken cancellationToken)
    {
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var obj = await _repository.GetById(request.Id);
        obj.UpdatedBy = updatedBy;
        obj.UpdatedDate = updatedDate;
        obj.UpdatedByName = updateName;
        obj.Status = request.Status;
        if (request.Status == 2 && (obj.ProductionOrderCode == null || obj.ProductionOrderCode == ""))
        {
            var model = new MESProductionOrder
            {
                Code = obj.Code,
                Note = obj.Note,
                Status = 1,
                RequestDate = obj.RequestDate,
                CustomerId = obj.CustomerId,
                CustomerCode = obj.CustomerCode,
                CustomerName = obj.CustomerName,
                Email = obj.Email,
                Phone = obj.Phone,
                Address = obj.Address,
                EmployeeId = obj.EmployeeId,
                EmployeeCode = obj.EmployeeCode,
                EmployeeName = obj.EmployeeName,
                DateNeed = obj.DateNeed,
                OrderId = obj.OrderId,
                OrderNumber = obj.OrderNumber,
                SaleEmployeeId = obj.SaleEmployeeId,
                SaleEmployeeCode = obj.SaleEmployeeCode,
                SaleEmployeeName = obj.SaleEmployeeName,
                Type = obj.Type,
                EstimateDate = obj.EstimateDate,
                File = obj.File,
                ProductionOrdersDetail = obj.ProductionOrdersDetail.Select(x => new MESProductionOrdersDetail()
                {
                    ProductId = x.ProductId.Value,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    ProductImage = x.ProductImage,
                    Sku = x.Sku,
                    Gtin = x.Gtin,
                    Origin = x.Origin,
                    UnitType = x.UnitType,
                    UnitCode = x.UnitCode,
                    UnitName = x.UnitName,
                    Quantity = x.Quantity,
                    Note = x.Note,
                    EstimatedDeliveryQuantity = x.EstimatedDeliveryQuantity,
                    DeliveryDate = x.DeliveryDate,
                    IsWorkOrdered = x.IsWorkOrdered,
                    ProductionOrdersCode = x.ProductionOrdersCode,
                    IsEstimated = x.IsEstimated,
                    IsBom = x.IsBom,
                    EstimateStatus = x.EstimateStatus
                }).ToList()
            };
            var result = await _MESRepository.AddExt(model);
            if (result.IsValid)
            {
                obj.ProductionOrderCode = result.RuleSetsExecuted[0];
            }
            _repository.Update(obj);
        }
        return await Commit(_repository.UnitOfWork);

    }

}
