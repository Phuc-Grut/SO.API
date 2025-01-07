using FluentValidation;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Ultilities;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;
using static VFi.Domain.SO.Enums.ExportWarehouseEnum;

namespace VFi.Application.SO.Commands.Validations;

public abstract class ExportWarehouseValidation<T> : AbstractValidator<T> where T : ExportWarehouseCommand

{
    protected readonly IExportWarehouseRepository _exportWarehouseRepository;
    protected readonly IExportWarehouseProductRepository _exportWarehouseProductRepository;
    protected readonly IOrderRepository _orderRepository;
    protected Guid Id;

    public ExportWarehouseValidation(IExportWarehouseRepository exportWarehouseRepository)
    {
        _exportWarehouseRepository = exportWarehouseRepository;
    }
    public ExportWarehouseValidation(IExportWarehouseRepository exportWarehouseRepository, Guid id)
    {
        _exportWarehouseRepository = exportWarehouseRepository;
        Id = id;
    }
    public ExportWarehouseValidation(IExportWarehouseRepository exportWarehouseRepository, IExportWarehouseProductRepository exportWarehouseProductRepository, IOrderRepository orderRepository, Guid id)
    {
        _exportWarehouseRepository = exportWarehouseRepository;
        _exportWarehouseProductRepository = exportWarehouseProductRepository;
        _orderRepository = orderRepository;
        Id = id;
    }

    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    private bool IsAddUnique(string code)
    {
        var model = _exportWarehouseRepository.GetByCode(code).Result;

        if (model == null)
        {
            return true;
        }

        return false;
    }
    private bool IsEditUnique(string? code)
    {
        var model = _exportWarehouseRepository.GetByCode(code).Result;

        if (model == null || model.Id == Id)
        {
            return true;
        }

        return false;
    }
    protected void ValidateEditCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code already exists").WithErrorCode(ErrorCode.TRUNGMA);
    }

    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .NotNull()
            .WithMessage("Code not null")
            .MaximumLength(50)
            .WithMessage("Code must not exceed 50 characters");
    }

    private bool IsValidObj(ExportWarehouseAddCommand exportWh)
    {
        Guid customerId = (Guid)(exportWh.Detail[0].CustomerId);
        foreach (ExportWarehouseProductDto item in exportWh.Detail)
        {
            if (item.CustomerId != customerId
                || (item.ExportWarehouseId != null
                    && item.ExportWarehouseStatus != (decimal)Status.Rejected
                    && item.ExportWarehouseStatus != null)
                || (item.Status != (decimal)OrderStatus.InWareHouse
                    && item.Status != (decimal)OrderStatus.WaitForSettlement)
            )
            {
                return false;
            }
        }
        return true;
    }

    protected void ValidObj(ExportWarehouseAddCommand exportWh)
    {
        RuleFor(x => x).Custom((command, context) =>
        {
            if (!IsValidObj(exportWh))
            {
                context.AddFailure("Unable to create export warehouse.");
            }
        });
    }
}
public class ExportWarehouseAddCommandValidation : ExportWarehouseValidation<ExportWarehouseAddCommand>
{
    public ExportWarehouseAddCommandValidation(IExportWarehouseRepository context, ExportWarehouseAddCommand command) : base(context)
    {
        ValidateId();
        ValidateCode();
        ValidateAddCodeUnique();
        ValidObj(command);
    }
}
public class ExportWarehouseEditCommandValidation : ExportWarehouseValidation<ExportWarehouseEditCommand>
{
    public ExportWarehouseEditCommandValidation(IExportWarehouseRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateCode();
        ValidateEditCodeUnique();
    }
}

public class ExportWarehouseDeleteCommandValidation : ExportWarehouseValidation<ExportWarehouseDeleteCommand>
{
    public ExportWarehouseDeleteCommandValidation(IExportWarehouseRepository context) : base(context)
    {
        ValidateId();
    }
}
public class ExportWarehousetDuplicateCommandValidation : ExportWarehouseValidation<ExportWarehouseDuplicateCommand>
{
    public ExportWarehousetDuplicateCommandValidation(IExportWarehouseRepository context) : base(context)
    {
        ValidateAddCodeUnique();
        ValidateId();
    }
}

public class ExportWarehouseAddOrderIdsCommandValidation : ExportWarehouseValidation<ExportWarehouseAddOrderIdsCommand>
{
    public ExportWarehouseAddOrderIdsCommandValidation(
            IExportWarehouseRepository _exportWarehouseRepository,
            IExportWarehouseProductRepository _exportWarehouseProductRepository,
            IOrderRepository _orderRepository,
            Guid id) :
    base(_exportWarehouseRepository, _exportWarehouseProductRepository, _orderRepository, id)
    {
        ValidateExportWarehouseId();
        ValidateStatus(id);
        ValidateOrderIds();
        ValidateCustomerIds();
    }

    private void ValidateExportWarehouseId()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("Id cannot be empty");
    }
    private void ValidateOrderIds()
    {
        RuleFor(x => x.OrderIds)
            .NotEmpty().WithMessage("Order IDs cannot be empty")
            .MustAsync(async (orderIds, cancellationToken) => await AreOrderIdsUnique(orderIds, Id, cancellationToken))
            .WithMessage("Some order IDs already exist");
    }

    private async Task<bool> AreOrderIdsUnique(List<Guid> orderIds, Guid exportWarehouseId, CancellationToken cancellationToken)
    {
        var existingProducts = await _exportWarehouseProductRepository
            .Filter(new Dictionary<string, object> { { "exportWarehouseId", exportWarehouseId } });

        var existingOrderIds = existingProducts
            .Where(x => x.OrderId.HasValue)
            .Select(x => x.OrderId.Value)
            .ToList();

        return !orderIds.Any(orderId => existingOrderIds.Contains(orderId));
    }

    private void ValidateStatus(Guid id)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) => await IsStatusValid(id))
            .WithMessage("The status of the export warehouse must be Draft or Pending");
    }

    private async Task<bool> IsStatusValid(Guid id)
    {
        var exportWarehouse = await _exportWarehouseRepository.GetById(id);
        return exportWarehouse != null && (exportWarehouse.Status == (int)ExportWarehouseEnum.Status.Draft || exportWarehouse.Status == (int)ExportWarehouseEnum.Status.Pending);
    }

    private void ValidateCustomerIds()
    {
        RuleFor(x => x.OrderIds)
            .MustAsync(async (orderIds, cancellationToken) =>
                await AreOrderIdsValidCustomer(orderIds, Id, cancellationToken))
            .WithMessage("CustomerId of some orders does not match the ExportWarehouse's CustomerId");
    }

    private async Task<bool> AreOrderIdsValidCustomer(List<Guid> orderIds, Guid exportWarehouseId, CancellationToken cancellationToken)
    {
        var exportWarehouse = await _exportWarehouseRepository.GetById(exportWarehouseId);

        var orders = await _orderRepository.GetByIdsWithCustomerId(orderIds);
        if (!orders.Any())
        {
            return false;
        }
        // Kiểm tra tất cả các Order có CustomerId khớp với CustomerId của ExportWarehouse
        return orders.All(order => order.CustomerId == exportWarehouse.CustomerId);
    }



}

public class ExportWarehouseUpdateServiceFeesCommandValidation : AbstractValidator<UpdateServiceFeesCommand>
{
    public ExportWarehouseUpdateServiceFeesCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("Id not null")
            .NotEmpty()
            .WithMessage("Id not empty");
        RuleFor(x => x.ServiceAddId)
            .NotNull()
            .WithMessage("ServiceAddId not null")
            .NotEmpty()
            .WithMessage("ServiceAddId not empty");
        RuleFor(x => x.ServiceAddCurrencyCode)
            .NotNull()
            .WithMessage("ServiceAddCurrencyCode not null")
            .NotEmpty()
            .WithMessage("ServiceAddCurrencyCode not empty");
    }
}
