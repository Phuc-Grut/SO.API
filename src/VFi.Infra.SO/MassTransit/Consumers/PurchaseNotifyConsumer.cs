using Flurl.Http;
using Flurl.Http.Configuration;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VFi.Domain.SO.Events;

namespace VFi.Infra.SO.MassTransit.Consumers;

public class PurchaseNotifyConsumer : IConsumer<PurchaseNotifyQueueEvent>
{
    private readonly ILogger<PurchaseNotifyQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;

    public PurchaseNotifyConsumer(ILogger<PurchaseNotifyQueueEvent> logger,
         IServiceProvider serviceFactory
)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;



    }
    public async Task Consume(ConsumeContext<PurchaseNotifyQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));

        var apiClient = _serviceFactory.GetService(typeof(IFlurlClientFactory)) as IFlurlClientFactory;
        if (apiClient != null)
        {
            var t = await apiClient.Get("https://api.telegram.org").Request("/bot7419349902:AAENDs9BfRKFEuG998k3k4P7BTw6Lr4t2Vc/sendMessage")
                    .SetQueryParam("chat_id", "-4278117526")
                    .SetQueryParam("parse_mode", "HTML")
                    .SetQueryParam("text", "Mã đơn hàng: <strong>"
                        + msg.OrderCode + "</strong>\n"
                        + "Khách hàng: <strong> " + msg.CustomerName + "</strong>\n"
                        + "Số tiền: <strong> " + msg.Price + "</strong>\n"
                        + "Link: <strong> " + msg.Link + "</strong>\n"
                        + "Ngày tạo: <strong> " + msg.Date + "</strong>\n"
                        + "Trạng thái: <strong> " + msg.Status + "</strong>")
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


//Mã đơn hàng: <strong>MG0000001XX</strong>
//Khách hàng: <strong>Nguyễn Văn A</strong>
//Tiền hàng: <strong>￥100.000</strong>
//Link: https://auction.megabuy.jp/product/v1141494789
//Ngày tạo: 2024/06/25 23:01
//Trạng thái: Chờ mua hàng

