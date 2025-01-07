using System;
using System.Composition;
using Consul;
using FluentValidation.Results;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CRM.Commands;

internal class ExportCommandHandler : CommandHandler,
                                    IRequestHandler<ExportAddCommand, ValidationResult>,
                                    IRequestHandler<ExportEditCommand, ValidationResult>,
                                    IRequestHandler<ExportDeleteCommand, ValidationResult>,
                                    IRequestHandler<ApprovalExportCommand, ValidationResult>
{
    private readonly IExportRepository _repository;
    private readonly IExportProductRepository _ExportProductRepository;
    private readonly IExportWarehouseRepository _exportWarehouseRepository;
    private readonly IExportWarehouseProductRepository _exportWarehouseProductRepository;
    private readonly IOrderProductRepository _OrderProductRepository;
    private readonly IContextUser _context;
    private readonly IWMSRepository _WMSRepository;

    public ExportCommandHandler(
        IExportRepository ExportRepository,
        IExportProductRepository ExportProductRepository,
        IOrderProductRepository orderProductRepository,
        IContextUser context,
        IWMSRepository WMSRepository,
        IExportWarehouseRepository exportWarehouseRepository,
        IExportWarehouseProductRepository exportWarehouseProductRepository
        )
    {
        _repository = ExportRepository;
        _ExportProductRepository = ExportProductRepository;
        _OrderProductRepository = orderProductRepository;
        _context = context;
        _WMSRepository = WMSRepository;
        _exportWarehouseRepository = exportWarehouseRepository;
        _exportWarehouseProductRepository = exportWarehouseProductRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ExportAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;

        var item = new Export
        {
            Id = request.Id,
            ExportWarehouseId = request.ExportWarehouseId,
            Code = request.Code,
            ExportDate = request.ExportDate,
            EmployeeId = request.EmployeeId,
            EmployeeCode = request.EmployeeCode,
            EmployeeName = request.EmployeeName,
            Status = request.Status,
            TotalQuantity = request.TotalQuantity,
            Note = request.Note,
            File = request.File,
            ApproveBy = request.ApproveBy,
            ApproveDate = request.ApproveDate,
            ApproveByName = request.ApproveByName,
            ApproveComment = request.ApproveComment,
            CreatedBy = request.CreatedBy,
            CreatedDate = DateTime.Now,
            CreatedByName = request.CreatedByName,
        };

        _repository.Add(item);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        List<ExportProduct> list = new List<ExportProduct>();
        if (request.Detail?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.Detail)
            {
                list.Add(new ExportProduct()
                {
                    Id = Guid.NewGuid(),
                    ExportId = request.Id,
                    ExportWarehouseProductId = u.ExportWarehouseProductId,
                    ProductId = u.ProductId,
                    ProductCode = u.ProductCode,
                    ProductName = u.ProductName,
                    ProductImage = u.ProductImage,
                    Origin = u.Origin,
                    UnitType = u.UnitType,
                    UnitCode = u.UnitCode,
                    UnitName = u.UnitName,
                    Quantity = u.Quantity,
                    Note = u.Note,
                    DisplayOrder = i
                });
                i++;
            }
            _ExportProductRepository.Add(list);
            /*if(request.Detail.All(x => x.ExportWarehouseProductId != null))
            {
                List<ExportWarehouseProduct> listUpdate = new List<ExportWarehouseProduct>();
                foreach(var x in request.Detail)
                {
                    var data = await _exportWarehouseProductRepository.GetById((Guid)x.ExportWarehouseProductId);
                    if(data is not null)
                    {
                        if(request.Status == 1)
                        {
                            data.QuantityExported = (double)(data.QuantityExported + x.Quantity);
                        }
                        data.QuantityRequest = (double)(data.QuantityExported - x.Quantity);
                        listUpdate.Add(data);
                    }
                }

                if (request.Status == 1)
                {
                    await UpdateReceiptStatus((Guid)request.ExportWarehouseId, listUpdate);

                }
                _exportWarehouseProductRepository.Update(listUpdate);
            }*/
            _ = await CommitNoCheck(_ExportProductRepository.UnitOfWork);
        }
        return result;
    }

    public async Task<ValidationResult> Handle(ExportDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new Export
        {
            Id = request.Id
        };
        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExportEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;

        var obj = await _repository.GetById(request.Id);

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "FulfillmentRequest is not exist") } };
        }

        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "FulfillmentRequest is not exist") } };
        }
        if (!request.IsValid(_repository))
            return request.ValidationResult;

        obj.Id = request.Id;
        obj.ExportWarehouseId = request.ExportWarehouseId;
        obj.Code = request.Code;
        obj.ExportDate = request.ExportDate;
        obj.EmployeeId = request.EmployeeId;
        obj.EmployeeCode = request.EmployeeCode;
        obj.EmployeeName = request.EmployeeName;
        obj.Status = request.Status;
        obj.TotalQuantity = request.TotalQuantity;
        obj.Note = request.Note;
        obj.File = request.File;
        obj.ApproveBy = request.ApproveBy;
        obj.ApproveDate = request.ApproveDate;
        obj.ApproveByName = request.ApproveByName;
        obj.ApproveComment = request.ApproveComment;
        obj.UpdatedBy = request.UpdatedBy;
        obj.UpdatedDate = DateTime.Now;
        obj.UpdatedByName = request.UpdatedByName;

        _repository.Update(obj);
        var result = await Commit(_repository.UnitOfWork);
        if (!result.IsValid)
            return result;

        //Chi tiết
        List<ExportProduct> listAdd = new List<ExportProduct>();
        List<ExportProduct> listUpdate = new List<ExportProduct>();
        List<ExportProduct> listDelete = new List<ExportProduct>();
        if (request.Detail?.Count > 0)
        {
            var i = 1;
            foreach (var u in request.Detail)
            {
                var item = obj.ExportProducts.Where(x => x.Id == u.Id).FirstOrDefault();
                if (item != null)
                {
                    item.Id = (Guid)u.Id;
                    item.ExportId = obj.Id;
                    item.ExportWarehouseProductId = u.ExportWarehouseProductId;
                    item.ProductId = u.ProductId;
                    item.ProductCode = u.ProductCode;
                    item.ProductName = u.ProductName;
                    item.ProductImage = u.ProductImage;
                    item.Origin = u.Origin;
                    item.UnitType = u.UnitType;
                    item.UnitCode = u.UnitCode;
                    item.UnitName = u.UnitName;
                    item.Quantity = u.Quantity;
                    item.Note = u.Note;
                    item.DisplayOrder = i;
                    listUpdate.Add(item);
                }
                else
                {
                    listAdd.Add(new ExportProduct()
                    {
                        Id = Guid.NewGuid(),
                        ExportId = obj.Id,
                        ExportWarehouseProductId = u.ExportWarehouseProductId,
                        ProductId = u.ProductId,
                        ProductCode = u.ProductCode,
                        ProductName = u.ProductName,
                        ProductImage = u.ProductImage,
                        Origin = u.Origin,
                        UnitType = u.UnitType,
                        UnitCode = u.UnitCode,
                        UnitName = u.UnitName,
                        Quantity = u.Quantity,
                        Note = u.Note,
                        DisplayOrder = i
                    });
                }
                i++;
            }
            if (listAdd.Count > 0)
            {
                _ExportProductRepository.Add(listAdd);
            }
            if (listUpdate.Count > 0)
            {
                _ExportProductRepository.Update(listUpdate);
            }
            if (request.Delete?.Count > 0)
            {
                foreach (var d in request.Delete)
                {
                    var item = obj.ExportProducts.Where(x => x.Id == d.Id).SingleOrDefault();
                    if (item is not null)
                    {
                        listDelete.Add(item);
                    }
                }
                _ExportProductRepository.Remove(listDelete);

            }
            _ = await CommitNoCheck(_ExportProductRepository.UnitOfWork);
        }
        return result;
    }
    public async Task<ValidationResult> Handle(ApprovalExportCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Export is not exist") } };
        }
        item.Id = request.Id;
        item.Status = request.Status;
        item.ApproveDate = DateTime.Now;
        item.ApproveBy = _context.GetUserId();
        item.ApproveByName = _context.UserClaims.FullName;
        item.ApproveComment = request.ApproveComment;
        _repository.Approve(item);
        return await Commit(_repository.UnitOfWork);
    }
    // cập nhật tráng thái của tab xuất kho
    /*private async Task<bool> UpdateReceiptStatus(Guid exportWarehouseId, List<ExportWarehouseProduct> detail)
    {
        try
        {
            if (exportWarehouseId != null && detail.Count > 0)
            {
                var totalQuantityImported = detail.Sum(x => x.QuantityExported);
                var totalQuantityRequest = detail.Sum(x => x.QuantityExported);

                var data = await _exportWarehouseRepository.GetById(exportWarehouseId);
                if (data is not null)
                {
                    if (totalQuantityImported == 0)
                    {
                        data.StatusExport = 0;
                    }
                    else if (totalQuantityImported < totalQuantityRequest)
                    {
                        data.StatusExport = 1;
                    }
                    else if (totalQuantityImported == totalQuantityRequest)
                    {
                        data.StatusExport = 2;
                    }
                    _exportWarehouseRepository.Update(data);
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }*/
}
