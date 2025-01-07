using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class OrderFulfillmentDetailCommandHandler : CommandHandler, IRequestHandler<OrderFulfillmentDetailAddCommand, ValidationResult>, IRequestHandler<OrderFulfillmentDetailDeleteCommand, ValidationResult>, IRequestHandler<OrderFulfillmentDetailEditCommand, ValidationResult>
{
    private readonly IOrderFulfillmentDetailRepository _repository;
    private readonly IContextUser _context;

    public OrderFulfillmentDetailCommandHandler(IOrderFulfillmentDetailRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(OrderFulfillmentDetailAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new OrderFulfillmentDetail
        {

            Id = request.Id,
            OrderFulfillmentId = request.OrderFulfillmentId,
            ProductId = request.ProductId,
            ProductCode = request.ProductCode,
            ProductName = request.ProductName,
            ProductImage = request.ProductImage,
            Origin = request.Origin,
            WarehouseId = request.WarehouseId,
            WarehouseCode = request.WarehouseCode,
            WarehouseName = request.WarehouseName,
            UnitPrice = request.UnitPrice,
            UnitName = request.UnitName,
            Quantity = request.Quantity,
            DisplayOrder = request.DisplayOrder,
            Note = request.Note,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderFulfillmentDetailDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new OrderFulfillmentDetail
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderFulfillmentDetailEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.OrderFulfillmentId = request.OrderFulfillmentId;
        item.ProductId = request.ProductId;
        item.ProductCode = request.ProductCode;
        item.ProductName = request.ProductName;
        item.ProductImage = request.ProductImage;
        item.Origin = request.Origin;
        item.WarehouseId = request.WarehouseId;
        item.WarehouseCode = request.WarehouseCode;
        item.WarehouseName = request.WarehouseName;
        item.UnitPrice = request.UnitPrice;
        item.UnitName = request.UnitName;
        item.Quantity = request.Quantity;
        item.DisplayOrder = request.DisplayOrder;
        item.Note = request.Note;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
