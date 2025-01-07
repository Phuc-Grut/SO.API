using FluentValidation.Results;
using MediatR;
using VFi.Application.SO.Commands;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CRM.Commands;

internal class OrderCostCommandHandler : CommandHandler,
                                    IRequestHandler<OrderCostAddCommand, ValidationResult>,
                                    IRequestHandler<OrderCostEditCommand, ValidationResult>,
                                    IRequestHandler<OrderCostDeleteCommand, ValidationResult>
{
    private readonly IOrderCostRepository _repository;

    public OrderCostCommandHandler(IOrderCostRepository OrderCostRepository)
    {
        _repository = OrderCostRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(OrderCostAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var OrderCost = new OrderCost
        {
            Id = request.Id,
            QuotationId = request.QuotationId,
            ExpenseId = request.ExpenseId,
            Type = request.Type,
            Rate = request.Rate,
            Amount = request.Amount,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            UpdatedBy = request.UpdatedBy,
            UpdatedDate = request.UpdatedDate

        };

        //add domain event
        //OrderCost.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Add(OrderCost);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderCostDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var OrderCost = new OrderCost
        {
            Id = request.Id,
        };

        //add domain event
        //OrderCost.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Remove(OrderCost);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(OrderCostEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var OrderCost = new OrderCost
        {
            Id = request.Id,
            QuotationId = request.QuotationId,
            ExpenseId = request.ExpenseId,
            Type = request.Type,
            Rate = request.Rate,
            Amount = request.Amount,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            UpdatedBy = request.UpdatedBy,
            UpdatedDate = request.UpdatedDate


        };

        //add domain event
        //OrderCost.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _repository.Update(OrderCost);
        return await Commit(_repository.UnitOfWork);
    }
}
