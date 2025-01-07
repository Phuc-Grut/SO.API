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

internal class OrderFulfillmentCommandHandler : CommandHandler, IRequestHandler<OrderFulfillmentAddCommand, ValidationResult>, IRequestHandler<OrderFulfillmentDeleteCommand, ValidationResult>, IRequestHandler<OrderFulfillmentEditCommand, ValidationResult>
{
    private readonly IOrderFulfillmentRepository _repository;
    private readonly IContextUser _context;

    public OrderFulfillmentCommandHandler(IOrderFulfillmentRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(OrderFulfillmentAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new OrderFulfillment
        {

            Id = request.Id,
            OrderType = request.OrderType,
            Code = request.Code,
            OrderDate = request.OrderDate,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            CustomerCode = request.CustomerCode,
            StoreId = request.StoreId,
            StoreCode = request.StoreCode,
            StoreName = request.StoreName,
            ContractId = request.ContractId,
            ContractName = request.ContractName,
            ChannelId = request.ChannelId,
            ChannelName = request.ChannelName,
            ShipperName = request.ShipperName,
            ShipperPhone = request.ShipperPhone,
            ShipperZipCode = request.ShipperZipCode,
            ShipperAddress = request.ShipperAddress,
            ShipperCountry = request.ShipperCountry,
            ShipperProvince = request.ShipperProvince,
            ShipperDistrict = request.ShipperDistrict,
            ShipperWard = request.ShipperWard,
            ShipperNote = request.ShipperNote,
            PickupStatus = request.PickupStatus,
            PickupName = request.PickupName,
            PickupPhone = request.PickupPhone,
            PickupZipCode = request.PickupZipCode,
            PickupAddress = request.PickupAddress,
            PickupCountry = request.PickupCountry,
            PickupProvince = request.PickupProvince,
            PickupDistrict = request.PickupDistrict,
            PickupWard = request.PickupWard,
            PickupNote = request.PickupNote,
            AccountName = request.AccountName,
            Note = request.Note,
            GroupEmployeeId = request.GroupEmployeeId,
            GroupEmployeeName = request.GroupEmployeeName,
            AccountId = request.AccountId,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderFulfillmentDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new OrderFulfillment
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderFulfillmentEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.OrderType = request.OrderType;
        item.Code = request.Code;
        item.OrderDate = request.OrderDate;
        item.CustomerId = request.CustomerId;
        item.CustomerName = request.CustomerName;
        item.CustomerCode = request.CustomerCode;
        item.StoreId = request.StoreId;
        item.StoreCode = request.StoreCode;
        item.StoreName = request.StoreName;
        item.ContractId = request.ContractId;
        item.ContractName = request.ContractName;
        item.ChannelId = request.ChannelId;
        item.ChannelName = request.ChannelName;
        item.ShipperName = request.ShipperName;
        item.ShipperPhone = request.ShipperPhone;
        item.ShipperZipCode = request.ShipperZipCode;
        item.ShipperAddress = request.ShipperAddress;
        item.ShipperCountry = request.ShipperCountry;
        item.ShipperProvince = request.ShipperProvince;
        item.ShipperDistrict = request.ShipperDistrict;
        item.ShipperWard = request.ShipperWard;
        item.ShipperNote = request.ShipperNote;
        item.PickupStatus = request.PickupStatus;
        item.PickupName = request.PickupName;
        item.PickupPhone = request.PickupPhone;
        item.PickupZipCode = request.PickupZipCode;
        item.PickupAddress = request.PickupAddress;
        item.PickupCountry = request.PickupCountry;
        item.PickupProvince = request.PickupProvince;
        item.PickupDistrict = request.PickupDistrict;
        item.PickupWard = request.PickupWard;
        item.PickupNote = request.PickupNote;
        item.AccountName = request.AccountName;
        item.Note = request.Note;
        item.GroupEmployeeId = request.GroupEmployeeId;
        item.GroupEmployeeName = request.GroupEmployeeName;
        item.AccountId = request.AccountId;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
