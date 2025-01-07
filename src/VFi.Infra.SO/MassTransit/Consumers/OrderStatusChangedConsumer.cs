using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.MassTransit.Consumers;

public class OrderStatusChangedConsumer : IConsumer<OrderStatusChangedQueueEvent>
{
    private readonly ILogger<OrderStatusChangedConsumer> _logger;
    private readonly IServiceProvider _serviceFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly ISyntaxCodeRepository _synctaxCodeRepository;
    private readonly CodeSyntaxConfig _codeSyntax;


    public OrderStatusChangedConsumer(
        ILogger<OrderStatusChangedConsumer> logger,
        IServiceProvider serviceFactory,
        IAccountRepository accountRepository,
        ISyntaxCodeRepository synctaxCodeRepository,
        CodeSyntaxConfig codeSyntax)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
        _accountRepository = accountRepository;
        _synctaxCodeRepository = synctaxCodeRepository;
        _codeSyntax = codeSyntax;
    }

    public async Task Consume(ConsumeContext<OrderStatusChangedQueueEvent> context)
    {
        var msg = context.Message;
        var contextUser = (IContextUser)_serviceFactory.GetRequiredService(typeof(IContextUser));
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        var orderRepository = _serviceFactory.GetRequiredService<IOrderRepository>();
        var requestPurchaseRepository = _serviceFactory.GetRequiredService<IRequestPurchaseRepository>();
        // if order status is null, get status here
        var orderStatus = msg.ToStatus;

        if (orderStatus is null)
        {
            return;
        }

        switch (orderStatus)
        {
            case (int)Domain.SO.Enums.OrderStatus.PendingConfirm:
                break;
            case (int)Domain.SO.Enums.OrderStatus.WaitForPurchase:
                await OrderWaitForPurchase(msg, orderRepository);
                await CreateRequestPurchase(msg, orderRepository, requestPurchaseRepository);
                break;
            case (int)Domain.SO.Enums.OrderStatus.DomesticShipping:
                break;
            case (int)Domain.SO.Enums.OrderStatus.InWareHouse:
                break;
            case (int)Domain.SO.Enums.OrderStatus.WaitForSettlement:
                //await OrderArrivedWarehouse(msg, orderRepository);
                break;
            case (int)Domain.SO.Enums.OrderStatus.Delivering:
                break;
            case (int)Domain.SO.Enums.OrderStatus.Delivered:
                break;
            case (int)Domain.SO.Enums.OrderStatus.Returned:
                break;
            case (int)Domain.SO.Enums.OrderStatus.Canceled:
                break;
            default:
                break;
        }
    }

    private async Task OrderArrivedWarehouse(OrderStatusChangedQueueEvent msg, IOrderRepository orderRepository)
    {
        try
        {
            var order = await orderRepository.GetByIdWithCustomerAndProducts(msg.OrderId);
            var customer = order.Customer;
            var product = order.OrderProduct.FirstOrDefault();
            var mailMerge = JsonConvert.SerializeObject(new
            {
                Name = customer.Name,
                OrderCode = order.Code,
                ProductName = product.ProductName,
                PreviewImage = product.ProductImage,
                Link = $"https://my.megabuy.jp/order/detail/{product.SourceCode}"
            });

            var emailNotify = new EmailNotify
            {
                SenderCode = "AWS_SMTP",
                SenderName = "Megabuy Japan",
                Subject = $"🚚 Đơn hàng đã về kho #{order.Code} - {order.OrderProduct.FirstOrDefault()?.ProductName}",
                To = order.Customer.Email,
                CC = "",
                BCC = "",
                Body = mailMerge,
                TemplateCode = "ORDER_RETURNED_WAREHOUSE"
            };

            _ = await _accountRepository.SendEmail(emailNotify);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        _logger.LogInformation($"Received {GetType().Name}: {JsonConvert.SerializeObject(msg)}");
    }

    private async Task OrderWaitForPurchase(OrderStatusChangedQueueEvent msg, IOrderRepository orderRepository)
    {
        try
        {
            var order = await orderRepository.GetByIdWithCustomerAndProducts(msg.OrderId);
            var customer = order.Customer;
            var product = order.OrderProduct.FirstOrDefault();


            {
                var currency = order.Currency switch
                {
                    "JPY" => "¥",
                    "USD" => "$",
                    "VND" => "₫",
                    _ => ""
                };

                var unitPrice = $"{((decimal)product.UnitPrice).ToString("#,##0")} {currency}";
                var quantity = ((decimal)(product.Quantity ?? 1)).ToString("F0");
                var mailMerge = JsonConvert.SerializeObject(new
                {
                    Name = customer.Name,
                    OrderCode = order.Code,
                    ProductName = product.ProductName,
                    PreviewImage = product.ProductImage,
                    Link = $"https://my.megabuy.jp/order/detail/{order.Code}",
                    CreatedDate = ((DateTime)order.CreatedDate).ToString("HH:mm dd/MM/yyyy"),
                    UnitPrice = unitPrice,
                    Quantity = quantity,
                    Tax = product.TaxRate
                });

                var emailNotify = new EmailNotify
                {
                    SenderCode = "AWS_SMTP",
                    SenderName = "Megabuy Japan",
                    Subject = $"✅ Đơn hàng của bạn đang được mua #{order.Code} - {product.ProductName}",
                    To = order.Customer.Email,
                    CC = "",
                    BCC = "",
                    Body = mailMerge,
                    TemplateCode = "ORDER_WAIT_FOR_PURCHASE"
                };

                _ = await _accountRepository.SendEmail(emailNotify);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        _logger.LogInformation($"Received {GetType().Name}: {JsonConvert.SerializeObject(msg)}");
    }
    protected async Task<string> GetCodeQuery(string syntax, int use)
    {
        var result = await _synctaxCodeRepository.GetCode(syntax, use);
        return result;
    }


    private async Task CreateRequestPurchase(OrderStatusChangedQueueEvent msg, IOrderRepository orderRepository, IRequestPurchaseRepository requestPurchaseRepository)
    {
        try
        {
            var order = await orderRepository.GetByIdWithCustomerAndProducts(msg.OrderId);
            var customer = order.Customer;

            var requestPurchase = new RequestPurchase
            {
                Code = await GetCodeQuery(_codeSyntax.SO_Request_Purchase, 1),
                CreatedDate = DateTime.Now,
                RequestBy = msg.RequestBy,
                RequestByName = msg.RequestByName,
                RequestByEmail = msg.RequestByEmail,
                RequestDate = DateTime.Now,
                CurrencyCode = order.Currency,
                CurrencyName = order.CurrencyName,
                QuantityRequest = (double)order.OrderProduct.Sum(x => x.Quantity ?? 0),
                ExchangeRate = order.ExchangeRate,
                Status = 0,
                Note = order.Note,
                Calculation = order.Calculation,
                File = order.File,
                OrderId = order.Id,
                OrderCode = order.Code,
            };

            foreach (var orderProduct in order.OrderProduct)
            {
                var requestPurchaseProduct = new RequestPurchaseProduct
                {
                    RequestPurchaseId = requestPurchase.Id,
                    OrderId = order.Id,
                    OrderCode = order.Code,
                    OrderProductId = orderProduct.Id,
                    ProductId = orderProduct.ProductId,
                    ProductCode = orderProduct.ProductCode,
                    ProductName = orderProduct.ProductName,
                    ProductImage = orderProduct.ProductImage,
                    SourceLink = orderProduct.SourceLink,
                    UnitPrice = orderProduct.UnitPrice,
                    Currency = order.Currency,
                    QuantityRequest = (double)orderProduct.Quantity,
                    Status = 1,
                    PriorityLevel = 0,
                    Note = orderProduct.Note,
                };

                requestPurchase.RequestPurchaseProduct.Add(requestPurchaseProduct);
            }

            requestPurchaseRepository.Add(requestPurchase);

            await requestPurchaseRepository.UnitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

    }
}
