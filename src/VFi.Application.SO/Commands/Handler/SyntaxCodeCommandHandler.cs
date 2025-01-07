using FluentValidation.Results;
using MediatR;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Handler;

internal class SyntaxCodeCommandHandler : CommandHandler, IRequestHandler<UseCodeCommand, ValidationResult>
{
    private readonly ISyntaxCodeRepository _repository;

    public SyntaxCodeCommandHandler(ISyntaxCodeRepository syntaxCodeRepository)
    {
        _repository = syntaxCodeRepository;
    }
    //public void Dispose()
    //{
    //    _repository.Dispose();
    //}

    public async Task<ValidationResult> Handle(UseCodeCommand request, CancellationToken cancellationToken)
    {
        _repository.UseCode(request.Syntax, request.Code);
        return new ValidationResult();
    }

}
