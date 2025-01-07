using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class QuotationTermCommandHandler : CommandHandler, IRequestHandler<QuotationTermAddCommand, ValidationResult>,
                                                            IRequestHandler<QuotationTermDeleteCommand, ValidationResult>,
                                                            IRequestHandler<QuotationTermEditCommand, ValidationResult>,
                                                            IRequestHandler<QuotationTermSortCommand, ValidationResult>
{
    private readonly IQuotationTermRepository _repository;
    private readonly IQuotationRepository _quotationRepository;
    private readonly IContextUser _context;

    public QuotationTermCommandHandler(IQuotationTermRepository quotationTermRepository, IContextUser contextUser, IQuotationRepository quotationRepository)
    {
        _repository = quotationTermRepository;
        _context = contextUser;
        _quotationRepository = quotationRepository;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(QuotationTermAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new QuotationTerm
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName,
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(QuotationTermDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var filter = new Dictionary<string, object> { { "quotationTermId", request.Id } };

        var quotations = await _quotationRepository.Filter(filter);

        if (quotations.Any())
        {
            return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
        }

        var item = new QuotationTerm
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(QuotationTermEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var quotationTerm = await _repository.GetById(request.Id);
        quotationTerm.Code = request.Code;
        quotationTerm.Name = request.Name;
        quotationTerm.Description = request.Description;
        quotationTerm.DisplayOrder = request.DisplayOrder;
        quotationTerm.Status = request.Status;
        quotationTerm.UpdatedDate = updatedDate;
        quotationTerm.UpdatedBy = updatedBy;
        quotationTerm.UpdatedByName = updateName;

        _repository.Update(quotationTerm);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(QuotationTermSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();

        List<QuotationTerm> list = new List<QuotationTerm>();

        foreach (var sort in request.SortList)
        {
            QuotationTerm obj = data.FirstOrDefault(c => c.Id == sort.Id);
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
