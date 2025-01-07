using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;


namespace VFi.Application.SO.Queries;

public class OrderCostQueryById : IQuery<OrderCostDto>
{
    public OrderCostQueryById()
    {
    }

    public OrderCostQueryById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class OrderCostQueryAll : IQuery<IEnumerable<OrderCostDto>>
{
    public OrderCostQueryAll()
    {
    }
}

public class OrderCostQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public OrderCostQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class OrderCostQueryCheckCode : IQuery<bool>
{

    public OrderCostQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class OrderCostPagingQuery : FopQuery, IQuery<PagedResult<List<OrderCostDto>>>
{
    public OrderCostPagingQuery(string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
// lấy dữ liệu ,khai báo
public class OrderCostQueryHandler : IQueryHandler<OrderCostQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<OrderCostQueryAll, IEnumerable<OrderCostDto>>,
                                         IQueryHandler<OrderCostQueryCheckCode, bool>,
                                         IQueryHandler<OrderCostQueryById, OrderCostDto>,
                                         IQueryHandler<OrderCostPagingQuery, PagedResult<List<OrderCostDto>>>
{
    private readonly IOrderCostRepository _OrderCostRepository;
    private readonly IEmployeeRepository _EmployeeRepository;
    private readonly ICustomerGroupRepository _CustomerGroupRepository;
    public OrderCostQueryHandler(IOrderCostRepository OrderCostRespository, IEmployeeRepository employeeRepository, ICustomerGroupRepository customerGroupRepository)
    {
        _OrderCostRepository = OrderCostRespository;
        _EmployeeRepository = employeeRepository;
        _CustomerGroupRepository = customerGroupRepository;
    }
    public async Task<bool> Handle(OrderCostQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _OrderCostRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<OrderCostDto> Handle(OrderCostQueryById request, CancellationToken cancellationToken)
    {
        var OrderCost = await _OrderCostRepository.GetById(request.Id);
        var result = new OrderCostDto()
        {
            Id = OrderCost.Id,
            QuotationId = OrderCost.Id,
            ExpenseId = OrderCost.ExpenseId,
            Type = OrderCost.Type,
            Rate = OrderCost.Rate,
            Amount = OrderCost.Amount,
            Status = OrderCost.Status,
            DisplayOrder = OrderCost.DisplayOrder,
            CreatedBy = OrderCost.CreatedBy,
            CreatedDate = OrderCost.CreatedDate,
            UpdatedBy = OrderCost.UpdatedBy,
            UpdatedDate = OrderCost.UpdatedDate
        };
        return result;
    }

    public async Task<PagedResult<List<OrderCostDto>>> Handle(OrderCostPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<OrderCostDto>>();

        var fopRequest = FopExpressionBuilder<OrderCost>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (OrderCosts, count) = await _OrderCostRepository.Filter(fopRequest);

        var data = OrderCosts.Select(OrderCost => new OrderCostDto()
        {
            Id = OrderCost.Id,
            QuotationId = OrderCost.Id,
            ExpenseId = OrderCost.ExpenseId,
            Type = OrderCost.Type,
            Rate = OrderCost.Rate,
            Amount = OrderCost.Amount,
            Status = OrderCost.Status,
            DisplayOrder = OrderCost.DisplayOrder,
            CreatedBy = OrderCost.CreatedBy,
            CreatedDate = OrderCost.CreatedDate,
            UpdatedBy = OrderCost.UpdatedBy,
            UpdatedDate = OrderCost.UpdatedDate

        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<OrderCostDto>> Handle(OrderCostQueryAll request, CancellationToken cancellationToken)
    {
        var OrderCosts = await _OrderCostRepository.GetAll();

        var result = OrderCosts.Select(OrderCost => new OrderCostDto()
        {
            Id = OrderCost.Id,
            QuotationId = OrderCost.Id,
            ExpenseId = OrderCost.ExpenseId,
            Type = OrderCost.Type,
            Rate = OrderCost.Rate,
            Amount = OrderCost.Amount,
            Status = OrderCost.Status,
            DisplayOrder = OrderCost.DisplayOrder,
            CreatedBy = OrderCost.CreatedBy,
            CreatedDate = OrderCost.CreatedDate,
            UpdatedBy = OrderCost.UpdatedBy,
            UpdatedDate = OrderCost.UpdatedDate

        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(OrderCostQueryComboBox request, CancellationToken cancellationToken)
    {

        var OrderCosts = await _OrderCostRepository.GetListCbx(request.Status);
        //var Contacts = await _ContactsRepository.GetListCbx(request.Status);
        var result = OrderCosts.Select(x => new ComboBoxDto()
        {
            Key = "",
            Value = x.Id,
            Label = ""
        });
        return result;
    }
}
