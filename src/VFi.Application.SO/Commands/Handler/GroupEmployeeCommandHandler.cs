using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class GroupEmployeeCommandHandler : CommandHandler, IRequestHandler<GroupEmployeeAddCommand, ValidationResult>, IRequestHandler<GroupEmployeeDeleteCommand, ValidationResult>, IRequestHandler<GroupEmployeeEditCommand, ValidationResult>
{
    private readonly IGroupEmployeeRepository _repository;
    private readonly IContextUser _context;
    private readonly ILeadRepository _leadRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IQuotationRepository _quotationRepository;
    private readonly IContractRepository _contractRepository;
    private readonly IOrderRepository _orderRepository;

    public GroupEmployeeCommandHandler(IGroupEmployeeRepository groupEmployeeRepository,
                                       IContextUser contextUser,
                                       ILeadRepository leadRepository,
                                       ICustomerRepository customerRepository,
                                       IQuotationRepository quotationRepository,
                                       IContractRepository contractRepository,
                                       IOrderRepository orderRepository)
    {
        _repository = groupEmployeeRepository;
        _context = contextUser;
        _leadRepository = leadRepository;
        _customerRepository = customerRepository;
        _quotationRepository = quotationRepository;
        _contractRepository = contractRepository;
        _orderRepository = orderRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(GroupEmployeeAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new GroupEmployee
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(GroupEmployeeDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;

        var filter = new Dictionary<string, object> { { "groupEmployeeId", request.Id } };

        var leads = await _leadRepository.Filter(filter);
        var customers = await _customerRepository.Filter(filter);
        var quotations = await _quotationRepository.Filter(filter);
        var contracts = await _contractRepository.Filter(filter);
        var orders = await _orderRepository.Filter(filter);

        if (leads.Any() || customers.Any() || quotations.Any() || contracts.Any() || orders.Any())
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
        }

        var item = new GroupEmployee
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(GroupEmployeeEditCommand request, CancellationToken cancellationToken)
    {
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = await _repository.GetById(request.Id);

        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
