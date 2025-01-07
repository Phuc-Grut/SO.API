using FluentValidation.Results;
using MediatR;

namespace VFi.NetDevPack.Messaging;

public abstract class Command : Message, IRequest<ValidationResult>
{
    public DateTime Timestamp { get; private set; }
    public ValidationResult ValidationResult { get; set; }

    protected Command()
    {
        Timestamp = DateTime.Now;
        ValidationResult = new ValidationResult();
    }

    public virtual bool IsValid()
    {
        return ValidationResult.IsValid;
    }
}

public abstract class CommandResult : Message, IRequest<VFi.NetDevPack.Domain.ValidationResult>
{
    public DateTime Timestamp { get; private set; }
    public VFi.NetDevPack.Domain.ValidationResult ValidationResult { get; set; }

    protected CommandResult()
    {
        Timestamp = DateTime.Now;
        ValidationResult = new VFi.NetDevPack.Domain.ValidationResult();
    }

    protected CommandResult(VFi.NetDevPack.Domain.ValidationResult result)
    {
        Timestamp = DateTime.Now;
        ValidationResult = result;
    }

    public virtual bool IsValid()
    {
        return ValidationResult.IsValid;
    }
}
