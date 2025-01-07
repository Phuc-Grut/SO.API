using FluentValidation.Results;

namespace VFi.NetDevPack.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {

        var detailErrors = new List<string>();
        foreach (var validationFailure in failures)
            detailErrors.Add(validationFailure.ErrorCode);
        ErrorCode = detailErrors;
    }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public IList<string> ErrorCode { get; }
}
