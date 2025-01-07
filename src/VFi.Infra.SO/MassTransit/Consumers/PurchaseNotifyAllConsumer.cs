using System;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using Consul;
using Flurl.Http;
using Flurl.Http.Configuration;
using MassTransit;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.PIM.Context;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Infra.SO.MassTransit.Consumers;

public class PurchaseNotifyAllConsumer : IConsumer<PurchaseNotifyAllQueueEvent>
{
    private readonly ILogger<PurchaseNotifyAllQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;

    public PurchaseNotifyAllConsumer(ILogger<PurchaseNotifyAllQueueEvent> logger, IServiceProvider serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;

    }
    public async Task Consume(ConsumeContext<PurchaseNotifyAllQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));

        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        var repository = _serviceFactory.GetService(typeof(IOrderRepository)) as IOrderRepository;
        var filter = new Dictionary<string, object>();
        filter.Add("status", 10);
        filter.Add("includeProduct", "1");
        //filter.Add("orderType", 10);
        var items = await repository.GetListListBox("", filter, 100, 1);
        var apiClient = _serviceFactory.GetService(typeof(IFlurlClientFactory)) as IFlurlClientFactory;
        foreach (var item in items.OrderBy(x => x.CustomerCode))
        {
            var products = item.OrderProduct;
            var productName = item.Description;
            var link = products.First().SourceLink;
            var t = apiClient.Get("https://api.telegram.org").Request("/bot7419349902:AAENDs9BfRKFEuG998k3k4P7BTw6Lr4t2Vc/sendMessage")
                                      .SetQueryParam("chat_id", "-4278117526")
                                      .SetQueryParam("parse_mode", "HTML")
                                      .SetQueryParam("text",
                                          "Mã đơn hàng: <strong>" + item.Code + "</strong>\n"
                                        + "Khách hàng: <strong> " + item.CustomerCode + " / " + item.CustomerName + "</strong>\n"
                                        + "Số tiền: <strong> " + item.TotalAmountTax.Value.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("ja-JP")) + "</strong>\n"
                                        + "Link: <strong> " + link + "</strong>\n"
                                        + "Ngày tạo: <strong> " + item.OrderDate.Value.ToString("yyyy/MM/dd HH:mm") + "</strong>\n"
                                        + "Trạng thái: <strong> " + "Chờ mua hàng" + "</strong>").GetAsync().Result;
        }

    }

}

;
