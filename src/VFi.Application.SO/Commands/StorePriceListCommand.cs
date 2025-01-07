using Consul;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

public class StorePriceListCommand : Command
{
    public Guid Id { get; set; }
    public Guid? StoreId { get; set; }
    public Guid? PriceListId { get; set; }
    public string? PriceListName { get; set; }
    public bool? Default { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public int? DisplayOrder { get; set; }
    public string? UpdatedByName { get; set; }
    public string? CreatedByName { get; set; }
}
public class StorePriceListAddCommand : StorePriceListCommand
{
    public StorePriceListAddCommand(
        Guid id,
        Guid? storeId,
        Guid? priceListId,
        string? priceListName,
        bool? defaults,
        DateTime createdDate,
        DateTime? updatedDate,
        Guid? createdBy,
        Guid? updatedBy,
        int? displayOrder,
        string? createdByName,
        string? updatedByName

        )
    {
        Id = id;
        StoreId = storeId;
        PriceListId = priceListId;
        PriceListName = priceListName;
        Default = defaults;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
        DisplayOrder = displayOrder;
        CreatedByName = createdByName;
        UpdatedByName = updatedByName;


    }
    public bool IsValid(IStorePriceListRepository _context)
    {
        ValidationResult = new StorePriceListAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class StorePriceListEditCommand : StorePriceListCommand
{
    public StorePriceListEditCommand(
        Guid id,
       Guid? storeId,
        Guid? priceListId,
        string? priceListName,
        bool? defaults,
        DateTime createdDate,
        DateTime? updatedDate,
        Guid? createdBy,
        Guid? updatedBy,
        int? displayOrder,
        string? createdByName,
        string? updatedByName
        )
    {
        Id = id;
        StoreId = storeId;
        PriceListId = priceListId;
        PriceListName = priceListName;
        Default = defaults;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
        DisplayOrder = displayOrder;
        CreatedByName = createdByName;
        UpdatedByName = updatedByName;

    }
    public bool IsValid(IStorePriceListRepository _context)
    {
        ValidationResult = new StorePriceListEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class StorePriceListDeleteCommand : StorePriceListCommand
{
    public StorePriceListDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IStorePriceListRepository _context)
    {
        ValidationResult = new StorePriceListDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
