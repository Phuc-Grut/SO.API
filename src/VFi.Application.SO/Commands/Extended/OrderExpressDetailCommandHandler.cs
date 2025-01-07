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

internal class OrderExpressDetailCommandHandler : CommandHandler, IRequestHandler<OrderExpressDetailAddCommand, ValidationResult>, IRequestHandler<OrderExpressDetailDeleteCommand, ValidationResult>, IRequestHandler<OrderExpressDetailEditCommand, ValidationResult>
{
    private readonly IOrderExpressDetailRepository _repository;
    private readonly IContextUser _context;

    public OrderExpressDetailCommandHandler(IOrderExpressDetailRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(OrderExpressDetailAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new OrderExpressDetail
        {

            Id = request.Id,
            OrderExpressId = request.OrderExpressId,
            ProductName = request.ProductName,
            ProductImage = request.ProductImage,
            Origin = request.Origin,
            UnitName = request.UnitName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            DisplayOrder = request.DisplayOrder,
            Note = request.Note,
            CommodityGroup = request.CommodityGroup,
            SurchargeGroup = request.SurchargeGroup,
            Surcharge = request.Surcharge,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderExpressDetailDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new OrderExpressDetail
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderExpressDetailEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.OrderExpressId = request.OrderExpressId;
        item.ProductName = request.ProductName;
        item.ProductImage = request.ProductImage;
        item.Origin = request.Origin;
        item.UnitName = request.UnitName;
        item.Quantity = request.Quantity;
        item.UnitPrice = request.UnitPrice;
        item.DisplayOrder = request.DisplayOrder;
        item.Note = request.Note;
        item.CommodityGroup = request.CommodityGroup;
        item.SurchargeGroup = request.SurchargeGroup;
        item.Surcharge = request.Surcharge;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
