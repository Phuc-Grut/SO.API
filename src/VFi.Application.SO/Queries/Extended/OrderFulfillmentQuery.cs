using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class OrderFulfillmentQueryAll : IQuery<IEnumerable<OrderFulfillmentDto>>
{
    public OrderFulfillmentQueryAll()
    {
    }
}

public class OrderFulfillmentQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public OrderFulfillmentQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class OrderFulfillmentQueryCheckCode : IQuery<bool>
{

    public OrderFulfillmentQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class OrderFulfillmentQueryById : IQuery<OrderFulfillmentDto>
{
    public OrderFulfillmentQueryById()
    {
    }

    public OrderFulfillmentQueryById(Guid OrderFulfillmentId)
    {
        OrderFulfillmentId = OrderFulfillmentId;
    }

    public Guid OrderFulfillmentId { get; set; }
}
public class OrderFulfillmentPagingQuery : FopQuery, IQuery<PagedResult<List<OrderFulfillmentDto>>>
{
    public OrderFulfillmentPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }

}


public class OrderFulfillmentQueryHandler : IQueryHandler<OrderFulfillmentQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<OrderFulfillmentQueryAll, IEnumerable<OrderFulfillmentDto>>,
                                         IQueryHandler<OrderFulfillmentQueryCheckCode, bool>,
                                         IQueryHandler<OrderFulfillmentQueryById, OrderFulfillmentDto>,
                                         IQueryHandler<OrderFulfillmentPagingQuery, PagedResult<List<OrderFulfillmentDto>>>
{
    private readonly IOrderFulfillmentRepository _repository;
    public OrderFulfillmentQueryHandler(IOrderFulfillmentRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(OrderFulfillmentQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<OrderFulfillmentDto> Handle(OrderFulfillmentQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.OrderFulfillmentId);
        var result = new OrderFulfillmentDto()
        {
            Id = item.Id,
            OrderType = item.OrderType,
            Code = item.Code,
            OrderDate = item.OrderDate,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CustomerCode = item.CustomerCode,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            ChannelId = item.ChannelId,
            ChannelName = item.ChannelName,
            ShipperName = item.ShipperName,
            ShipperPhone = item.ShipperPhone,
            ShipperZipCode = item.ShipperZipCode,
            ShipperAddress = item.ShipperAddress,
            ShipperCountry = item.ShipperCountry,
            ShipperProvince = item.ShipperProvince,
            ShipperDistrict = item.ShipperDistrict,
            ShipperWard = item.ShipperWard,
            ShipperNote = item.ShipperNote,
            PickupStatus = item.PickupStatus,
            PickupName = item.PickupName,
            PickupPhone = item.PickupPhone,
            PickupZipCode = item.PickupZipCode,
            PickupAddress = item.PickupAddress,
            PickupCountry = item.PickupCountry,
            PickupProvince = item.PickupProvince,
            PickupDistrict = item.PickupDistrict,
            PickupWard = item.PickupWard,
            PickupNote = item.PickupNote,
            AccountName = item.AccountName,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<OrderFulfillmentDto>>> Handle(OrderFulfillmentPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<OrderFulfillmentDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<OrderFulfillment>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new OrderFulfillmentDto()
        {
            Id = item.Id,
            OrderType = item.OrderType,
            Code = item.Code,
            OrderDate = item.OrderDate,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CustomerCode = item.CustomerCode,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            ChannelId = item.ChannelId,
            ChannelName = item.ChannelName,
            ShipperName = item.ShipperName,
            ShipperPhone = item.ShipperPhone,
            ShipperZipCode = item.ShipperZipCode,
            ShipperAddress = item.ShipperAddress,
            ShipperCountry = item.ShipperCountry,
            ShipperProvince = item.ShipperProvince,
            ShipperDistrict = item.ShipperDistrict,
            ShipperWard = item.ShipperWard,
            ShipperNote = item.ShipperNote,
            PickupStatus = item.PickupStatus,
            PickupName = item.PickupName,
            PickupPhone = item.PickupPhone,
            PickupZipCode = item.PickupZipCode,
            PickupAddress = item.PickupAddress,
            PickupCountry = item.PickupCountry,
            PickupProvince = item.PickupProvince,
            PickupDistrict = item.PickupDistrict,
            PickupWard = item.PickupWard,
            PickupNote = item.PickupNote,
            AccountName = item.AccountName,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<OrderFulfillmentDto>> Handle(OrderFulfillmentQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new OrderFulfillmentDto()
        {
            Id = item.Id,
            OrderType = item.OrderType,
            Code = item.Code,
            OrderDate = item.OrderDate,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CustomerCode = item.CustomerCode,
            StoreId = item.StoreId,
            StoreCode = item.StoreCode,
            StoreName = item.StoreName,
            ContractId = item.ContractId,
            ContractName = item.ContractName,
            ChannelId = item.ChannelId,
            ChannelName = item.ChannelName,
            ShipperName = item.ShipperName,
            ShipperPhone = item.ShipperPhone,
            ShipperZipCode = item.ShipperZipCode,
            ShipperAddress = item.ShipperAddress,
            ShipperCountry = item.ShipperCountry,
            ShipperProvince = item.ShipperProvince,
            ShipperDistrict = item.ShipperDistrict,
            ShipperWard = item.ShipperWard,
            ShipperNote = item.ShipperNote,
            PickupStatus = item.PickupStatus,
            PickupName = item.PickupName,
            PickupPhone = item.PickupPhone,
            PickupZipCode = item.PickupZipCode,
            PickupAddress = item.PickupAddress,
            PickupCountry = item.PickupCountry,
            PickupProvince = item.PickupProvince,
            PickupDistrict = item.PickupDistrict,
            PickupWard = item.PickupWard,
            PickupNote = item.PickupNote,
            AccountName = item.AccountName,
            Note = item.Note,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeName,
            AccountId = item.AccountId,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(OrderFulfillmentQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Code
        });
        return result;
    }
}
