using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class ContractTypeCommand : Command
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Status { get; set; }
}
public class ContractTypeAddCommand : ContractTypeCommand
{
    public ContractTypeAddCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int? status
        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }
    public bool IsValid(IContractTypeRepository _context)
    {
        ValidationResult = new ContractTypeAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ContractTypeEditCommand : ContractTypeCommand
{
    public ContractTypeEditCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int? status

        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
    }
    public bool IsValid(IContractTypeRepository _context)
    {
        ValidationResult = new ContractTypeEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ContractTypeDeleteCommand : ContractTypeCommand
{
    public ContractTypeDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContractTypeRepository _context)
    {
        ValidationResult = new ContractTypeDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
