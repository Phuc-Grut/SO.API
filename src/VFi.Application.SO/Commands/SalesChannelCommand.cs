using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class SalesChannelCommand : Command
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public bool? IsDefault { get; set; }
    public int? DisplayOrder { get; set; }
}
public class SalesChannelAddCommand : SalesChannelCommand
{
    public SalesChannelAddCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int status,
        bool? isDefault,
        int? displayOrder
        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        IsDefault = isDefault;
        DisplayOrder = displayOrder;

    }
    public bool IsValid(ISalesChannelRepository _context)
    {
        ValidationResult = new SalesChannelAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class SalesChannelEditCommand : SalesChannelCommand
{
    public SalesChannelEditCommand(
        Guid id,
        string? code,
        string? name,
        string? description,
        int status,
        bool? isDefault,
        int? displayOrder
        )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        IsDefault = isDefault;
        DisplayOrder = displayOrder;

    }
    public bool IsValid(ISalesChannelRepository _context)
    {
        ValidationResult = new SalesChannelEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class SalesChannelDeleteCommand : SalesChannelCommand
{
    public SalesChannelDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ISalesChannelRepository _context)
    {
        ValidationResult = new SalesChannelDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
