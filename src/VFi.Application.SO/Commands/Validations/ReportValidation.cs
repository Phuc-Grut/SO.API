using FluentValidation;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ReportValidation<T> : AbstractValidator<T> where T : ReportCommand

{
    protected readonly IReportRepository _context;
    private Guid Id;

    public ReportValidation(IReportRepository context)
    {
        _context = context;
    }
    public ReportValidation(IReportRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

}
public class ReportAddCommandValidation : ReportValidation<ReportAddCommand>
{
    public ReportAddCommandValidation(IReportRepository context) : base(context)
    {
        ValidateId();
    }
}
public class ReportEditCommandValidation : ReportValidation<ReportEditCommand>
{
    public ReportEditCommandValidation(IReportRepository context, Guid id) : base(context, id)
    {
        ValidateId();
    }
}

public class ReportDeleteCommandValidation : ReportValidation<ReportDeleteCommand>
{
    public ReportDeleteCommandValidation(IReportRepository context) : base(context)
    {
        ValidateId();
    }
}
