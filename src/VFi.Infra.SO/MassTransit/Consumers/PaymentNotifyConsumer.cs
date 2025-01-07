using Flurl.Http;
using Flurl.Http.Configuration;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VFi.Domain.SO.Events;

namespace VFi.Infra.SO.MassTransit.Consumers;

public class PaymentNotifyConsumer : IConsumer<PaymentNotifyQueueEvent>
{
    private readonly ILogger<PaymentNotifyQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;
    public PaymentNotifyConsumer(ILogger<PaymentNotifyQueueEvent> logger,
         IServiceProvider serviceFactory, IFlurlClientFactory flurlClientFac
)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;


    }
    public async Task Consume(ConsumeContext<PaymentNotifyQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));

        var apiClient = _serviceFactory.GetService(typeof(IFlurlClientFactory)) as IFlurlClientFactory;
        if (apiClient != null)
        {
            var t = await apiClient.Get("https://api.telegram.org").Request("/bot7419349902:AAENDs9BfRKFEuG998k3k4P7BTw6Lr4t2Vc/sendMessage")
                .SetQueryParam("chat_id", "-4205545373")
                .SetQueryParam("parse_mode", "HTML")
                .SetQueryParam("text", "Loại giao dịch: <strong>" + msg.PaymentType + "</strong>\n"
                    + "Khách hàng: <strong> " + msg.CustomerName + "</strong>\n"
                    + "Số tiền: <strong> " + msg.Amount + "</strong>\n"
                    + "Nội dung: <strong> " + msg.Body + "</strong>\n"
                    + "Ngày tạo: <strong> " + msg.Date + "</strong>")
                .GetAsync()
                .ReceiveJson();
            if (t != null && t?.ok != null && t?.ok == true)
            {
                _logger.LogDebug("Send tele ok!");
            }
            else
            {
                throw new Exception(t?.description);
            }
        }

    }

}


