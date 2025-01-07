using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class DeliveryProductCommandHandler : CommandHandler,
    IRequestHandler<DeliveryProductAddCommand, ValidationResult>,
    IRequestHandler<DeliveryProductDeleteCommand, ValidationResult>,
    IRequestHandler<DeliveryProductAddRangeCommand, ValidationResult>,
    IRequestHandler<DeliveryProductEditCommand, ValidationResult>
{
    private readonly IDeliveryProductRepository _repository;
    private readonly IContextUser _context;

    public DeliveryProductCommandHandler(IDeliveryProductRepository DeliveryProductRepository, IContextUser contextUser)
    {
        _repository = DeliveryProductRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(DeliveryProductAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new DeliveryProduct
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            DisplayOrder = request.DisplayOrder,
            Description = request.Description,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(DeliveryProductDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new DeliveryProduct
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(DeliveryProductEditCommand request, CancellationToken cancellationToken)
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
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;


        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(DeliveryProductAddRangeCommand request, CancellationToken cancellationToken)
    {
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var listGuid = request.ListGuidDeliveryProduct.Split(",").ToList();
        var list = await _repository.GetByDeliveryProductId(listGuid.ConvertAll(Guid.Parse));
        List<DeliveryProduct> listAdd = new List<DeliveryProduct>();
        List<DeliveryProduct> listEdit = new List<DeliveryProduct>();
        List<DeliveryProduct> listExist = new List<DeliveryProduct>();

        if (request?.List?.Count > 0)
        {
            foreach (var item in request.List)
            {
                var record = list?.Where(x => x.Id == item.Id).FirstOrDefault();
                if (record is null)
                {
                    listAdd.Add(new DeliveryProduct
                    {
                        Id = Guid.NewGuid(),
                        OrderProductId = item.OrderProductId,
                        DeliveryDate = item.DeliveryDate,
                        QuantityExpected = item.QuantityExpected,
                        Description = item.Description,
                        DisplayOrder = item.DisplayOrder,
                        CreatedBy = createdBy,
                        CreatedDate = createdDate,
                        CreatedByName = createName
                    });
                }
                else
                {
                    record.QuantityExpected = item.QuantityExpected;
                    record.Description = item.Description;
                    record.DeliveryDate = item.DeliveryDate;
                    record.DisplayOrder = item.DisplayOrder;
                    record.UpdatedBy = createdBy;
                    record.UpdatedDate = createdDate;
                    record.UpdatedByName = createName;
                    listEdit.Add(record);
                    if (item.IsChange == true)
                    {
                        listEdit.Add(record);
                    }
                    listExist.Add(record);
                }
            }
        }

        if (listAdd.Count > 0)
        {
            _repository.Add(listAdd);
        }
        if (listEdit.Count > 0)
        {
            _repository.Update(listEdit);
        }
        var listDel = list.Where(x => !listExist.Select(l => l.Id).ToList().Contains(x.Id)).ToList();
        if (listDel.Count > 0)
        {
            _repository.Remove(listDel);
        }

        return await Commit(_repository.UnitOfWork);
    }
}
