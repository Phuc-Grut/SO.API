using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Consul;
using FluentValidation.Results;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using static System.Net.Mime.MediaTypeNames;

namespace VFi.Application.SO.Commands;

internal class BankCommandHandler : CommandHandler, IRequestHandler<BankAddCommand, ValidationResult>,
                                                       IRequestHandler<BankDeleteCommand, ValidationResult>,
                                                       IRequestHandler<BankEditCommand, ValidationResult>,
                                                       IRequestHandler<BankSortCommand, ValidationResult>
{
    private readonly IBankRepository _bankRepository;
    private readonly IContextUser _context;

    public BankCommandHandler(IBankRepository bankRepository, IContextUser contextUser)
    {
        _bankRepository = bankRepository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _bankRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(BankAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_bankRepository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new Bank
        {
            Id = request.Id,
            Code = request.Code,
            Qrbin = request.Qrbin,
            ShortName = request.ShortName,
            Name = request.Name,
            EnglishName = request.EnglishName,
            Address = request.Address,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            Note = request.Note,
            Image = request.Image,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _bankRepository.Add(item);
        return await Commit(_bankRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(BankDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_bankRepository))
            return request.ValidationResult;
        var item = new Bank
        {
            Id = request.Id
        };

        _bankRepository.Remove(item);
        return await Commit(_bankRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(BankEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_bankRepository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _bankRepository.GetById(request.Id);

        if (item is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Bank is not exist") } };
        }
        item.Code = request.Code;
        item.Qrbin = request.Qrbin;
        item.ShortName = request.ShortName;
        item.Name = request.Name;
        item.EnglishName = request.EnglishName;
        item.Address = request.Address;
        item.DisplayOrder = request.DisplayOrder;
        item.Status = request.Status;
        item.Note = request.Note;
        item.Image = request.Image;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;
        item.UpdatedDate = updatedDate;

        _bankRepository.Update(item);
        return await Commit(_bankRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(BankSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _bankRepository.GetAll();

        List<Bank> list = new List<Bank>();

        foreach (var sort in request.SortList)
        {
            Bank obj = data.FirstOrDefault(c => c.Id == sort.Id);
            if (obj != null)
            {
                obj.DisplayOrder = sort.SortOrder;
                list.Add(obj);
            }
        }
        _bankRepository.Update(list);
        return await Commit(_bankRepository.UnitOfWork);
    }
}
