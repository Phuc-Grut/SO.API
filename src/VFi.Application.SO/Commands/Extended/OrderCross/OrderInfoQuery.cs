using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Commands.Extended.OrderCross;

public class OrderInfoQuery : IQuery<IList<OrderInfoDto>>
{
    public OrderInfoQuery(Guid accountId, List<string> orderCode)
    {
        AccountId = accountId;
        OrderCode = orderCode;
    }
    public Guid AccountId { get; set; }
    public List<string> OrderCode { get; set; }
}

public class OrderInfoDto
{
    public string Code { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? PaymentExpiryDate { get; set; }
}



public class OrderInfoByCodeQueryHandler :
    IQueryHandler<OrderInfoQuery, IList<OrderInfoDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    public OrderInfoByCodeQueryHandler(IOrderRepository orderRespository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRespository;
        _customerRepository = customerRepository;
    }

    public async Task<IList<OrderInfoDto>> Handle(OrderInfoQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var customerId = await _customerRepository.GetIdByAccountId(request.AccountId);
        var order = await _orderRepository.GetInfoByCodes(customerId, request.OrderCode);
        if (order is null)
        {
            throw new ErrorCodeException("ORDER_NOT_FOUND");
        }
        return order.Select(x => new OrderInfoDto()
        {
            Status = x.Status,
            PaymentExpiryDate = x.PaymentExpiryDate,
            CreatedDate = x.CreatedDate,
            Code = x.Code,
        }).ToList();
    }


}
