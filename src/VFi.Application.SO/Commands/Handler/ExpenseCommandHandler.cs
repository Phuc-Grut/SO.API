using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class ExpenseCommandHandler : CommandHandler, IRequestHandler<ExpenseAddCommand, ValidationResult>,
                                                        IRequestHandler<ExpenseDeleteCommand, ValidationResult>,
                                                        IRequestHandler<ExpenseEditCommand, ValidationResult>,
                                                        IRequestHandler<ExpenseSortCommand, ValidationResult>
{
    private readonly IExpenseRepository _repository;
    private readonly IContextUser _context;

    public ExpenseCommandHandler(IExpenseRepository ExpenseRepository, IContextUser contextUser)
    {
        _repository = ExpenseRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ExpenseAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Expense
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExpenseDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new Expense
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExpenseEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.DisplayOrder = request.DisplayOrder;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ExpenseSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();

        List<Expense> list = new List<Expense>();

        foreach (var sort in request.SortList)
        {
            Expense obj = data.FirstOrDefault(c => c.Id == sort.Id);
            if (obj != null)
            {
                obj.DisplayOrder = sort.SortOrder;
                list.Add(obj);
            }
        }
        _repository.Update(list);
        return await Commit(_repository.UnitOfWork);
    }
}
