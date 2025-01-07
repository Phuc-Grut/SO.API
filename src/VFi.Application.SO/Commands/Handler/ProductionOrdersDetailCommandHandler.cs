using System.Data;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class ProductionOrdersDetailCommandHandler : CommandHandler, IRequestHandler<ProductionOrdersDetailEditPackageCommand, ValidationResult>,
                                                                     IRequestHandler<ProductionOrdersDetailCancelCommand, ValidationResult>,
                                                                     IRequestHandler<ProductionOrdersDetailCompleteCommand, ValidationResult>

{
    private readonly IProductionOrdersDetailRepository _repository;
    private readonly IContextUser _context;

    public ProductionOrdersDetailCommandHandler(
        IContextUser contextUser,
        IProductionOrdersDetailRepository ProductionOrdersDetailRepository
    )
    {
        _context = contextUser;
        _repository = ProductionOrdersDetailRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ProductionOrdersDetailEditPackageCommand request, CancellationToken cancellationToken)
    {
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var data = await _repository.GetById(request.Id);
        data.Solution = request.Solution;
        data.Transport = request.Transport;
        data.Height = request.Height;
        data.Package = request.Package;
        data.Volume = request.Volume;
        data.Length = request.Length;
        data.Weight = request.Weight;
        data.Width = request.Width;
        data.UpdatedBy = updatedBy;
        data.UpdatedDate = updatedDate;
        data.UpdatedByName = updateName;
        _repository.Update(data);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ProductionOrdersDetailCancelCommand request, CancellationToken cancellationToken)
    {
        int CancelStatus = 4;
        if (!request.IsValidPro(_repository))
            return request.ValidationResult;
        var dataPO = await _repository.GetById(request.Id);
        dataPO.Status = CancelStatus;
        dataPO.CancelReason = request.CancelReason;
        _repository.Update(dataPO);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ProductionOrdersDetailCompleteCommand request, CancellationToken cancellationToken)
    {
        int CompleteStatus = 3;
        if (!request.IsValidPro(_repository))
            return request.ValidationResult;
        var dataPO = await _repository.GetById(request.Id);
        dataPO.Status = CompleteStatus;
        _repository.Update(dataPO);

        return await Commit(_repository.UnitOfWork);
    }
}
