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

internal class PostOfficeCommandHandler : CommandHandler, IRequestHandler<PostOfficeAddCommand, ValidationResult>, IRequestHandler<PostOfficeDeleteCommand, ValidationResult>, IRequestHandler<PostOfficeEditCommand, ValidationResult>
{
    private readonly IPostOfficeRepository _repository;
    private readonly IContextUser _context;

    public PostOfficeCommandHandler(IPostOfficeRepository repository, IContextUser contextUser)
    {
        _repository = repository;
        _context = contextUser;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(PostOfficeAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var item = new PostOffice
        {

            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            ShortName = request.ShortName,
            Country = request.Country,
            Address = request.Address,
            Address1 = request.Address1,
            PostCode = request.PostCode,
            Phone = request.Phone,
            SyntaxSender = request.SyntaxSender,
            Note = request.Note,
            Status = request.Status,
            CreatedBy = createdBy,
            CreatedDate = createdDate,
            CreatedByName = createName
        };

        _repository.Add(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PostOfficeDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var item = new PostOffice
        {
            Id = request.Id
        };

        _repository.Remove(item);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(PostOfficeEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.Code = request.Code;
        item.Name = request.Name;
        item.ShortName = request.ShortName;
        item.Country = request.Country;
        item.Address = request.Address;
        item.Address1 = request.Address1;
        item.PostCode = request.PostCode;
        item.Phone = request.Phone;
        item.SyntaxSender = request.SyntaxSender;
        item.Note = request.Note;
        item.Status = request.Status;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
}
