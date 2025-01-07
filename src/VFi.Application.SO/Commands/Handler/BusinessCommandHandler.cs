using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class BusinessCommandHandler : CommandHandler, IRequestHandler<BusinessAddCommand, ValidationResult>,
                                                       IRequestHandler<BusinessDeleteCommand, ValidationResult>,
                                                       IRequestHandler<BusinessEditCommand, ValidationResult>,
                                                       IRequestHandler<BusinessSortCommand, ValidationResult>
{
    private readonly IBusinessRepository _businessRepository;
    private readonly IContextUser _context;
    private readonly ICustomerBusinessMappingRepository _customerBusinessMappingRepository;

    public BusinessCommandHandler(IBusinessRepository businessRepository, IContextUser contextUser, ICustomerBusinessMappingRepository customerBusinessMappingRepository)
    {
        _businessRepository = businessRepository;
        _context = contextUser;
        _customerBusinessMappingRepository = customerBusinessMappingRepository;
    }
    public void Dispose()
    {
        _businessRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(BusinessAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_businessRepository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Business
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

        _businessRepository.Add(item);
        return await Commit(_businessRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(BusinessDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_businessRepository))
            return request.ValidationResult;
        var filter = new Dictionary<string, object> { { "businessId", request.Id } };

        var customers = await _customerBusinessMappingRepository.GetListListBox(filter);

        if (customers.Any())
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
        }
        var Business = new Business
        {
            Id = request.Id
        };

        _businessRepository.Remove(Business);
        return await Commit(_businessRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(BusinessEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_businessRepository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _businessRepository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Business is not exist") } };
        }

        item.Code = request.Code;
        item.Name = request.Name;
        item.Description = request.Description;
        item.Status = request.Status;
        item.DisplayOrder = request.DisplayOrder;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;

        _businessRepository.Update(item);
        return await Commit(_businessRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(BusinessSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _businessRepository.GetAll();

        List<Business> list = new List<Business>();

        foreach (var sort in request.SortList)
        {
            Business obj = data.FirstOrDefault(c => c.Id == sort.Id);
            if (obj != null)
            {
                obj.DisplayOrder = sort.SortOrder;
                list.Add(obj);
            }
        }
        _businessRepository.Update(list);
        return await Commit(_businessRepository.UnitOfWork);
    }
}
