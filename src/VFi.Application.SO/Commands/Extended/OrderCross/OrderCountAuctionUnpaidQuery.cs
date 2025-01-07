using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Commands.Extended.OrderCross;

public class OrderCountAuctionUnpaidQuery : IQuery<OrderCountAuctionUnpaidDto>
{
    public OrderCountAuctionUnpaidQuery(Guid accountId)
    {
        AccountId = accountId;
    }
    public Guid AccountId { get; set; }
}

public class OrderCountAuctionUnpaidDto
{
    public int CountUnpaid { get; set; }
    public int CountUnpaidExpired { get; set; }
    public int Total => CountUnpaid + CountUnpaidExpired;
}



public class OrderCountAuctionUnpaidByCodeQueryHandler :
    IQueryHandler<OrderCountAuctionUnpaidQuery, OrderCountAuctionUnpaidDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    public OrderCountAuctionUnpaidByCodeQueryHandler(IOrderRepository orderRespository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRespository;
        _customerRepository = customerRepository;
    }

    public async Task<OrderCountAuctionUnpaidDto> Handle(OrderCountAuctionUnpaidQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var customerId = await _customerRepository.GetIdByAccountId(request.AccountId);
        var order = await _orderRepository.GetAuctionUnpaid(customerId);
        if (order is null)
        {
            return new OrderCountAuctionUnpaidDto
            {
                CountUnpaid = 0,
                CountUnpaidExpired = 0,
            };
        }
        return new OrderCountAuctionUnpaidDto
        {
            CountUnpaid = order.Where(x => !x.PaymentExpiryDate.HasValue || x.PaymentExpiryDate.Value > DateTime.Now).Count(),
            CountUnpaidExpired = order.Where(x => x.PaymentExpiryDate.HasValue && x.PaymentExpiryDate.Value < DateTime.Now).Count(),
        };
    }


}
