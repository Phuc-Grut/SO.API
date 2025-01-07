using HtmlAgilityPack;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Interfaces.Extented;
using VFi.NetDevPack.Context;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace VFi.Infra.SO.MassTransit.Consumers
{
    public class OrderItemFetchDomesticDeliveryConsumer: IConsumer<OrderItemFetchDomesticDeliveryQueueEvent>
    {
        private readonly ILogger<OrderItemFetchDomesticDeliveryQueueEvent> _logger;
        private readonly IServiceProvider _serviceFactory;
        private readonly ICarrierJapanRepository _carrierJapanRepository;


        public OrderItemFetchDomesticDeliveryConsumer(ILogger<OrderItemFetchDomesticDeliveryQueueEvent> logger,
         IServiceProvider serviceFactory,
         ICarrierJapanRepository carrierJapanRepository)
        {
            _logger = logger;
            _serviceFactory = serviceFactory;
            _carrierJapanRepository = carrierJapanRepository;

        }
        public async Task Consume(ConsumeContext<OrderItemFetchDomesticDeliveryQueueEvent> context)
        {
            var msg = context.Message;
            _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));
            var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
            contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
            //var _shippingCarrierRepository = _serviceFactory.GetService(typeof(IShippingCarrierRepository)) as IShippingCarrierRepository;
            var _procedures = _serviceFactory.GetService(typeof(ISOExtProcedures)) as ISOExtProcedures;

            //var allShippingCarrier = await _shippingCarrierRepository!.GetAll();

            _logger.LogInformation("OrderItemFetchTrackingQueueEvent: msg {@msg}", msg);

            if (msg == null
                 || !msg.Id.HasValue
                 )
            {
                return;
            }
            await FetchOrder(_procedures!, msg);

        }
        private async Task FetchOrder(
        ISOExtProcedures procedures,
        OrderItemFetchDomesticDeliveryQueueEvent message
        )
        {
            switch (message.DomesticCarrier)
            {
                case "Yamato":
                    var responseYamato = await _carrierJapanRepository.GetYamato(message.DomesticTracking.Replace("-", ""));
                    DateTime? deliveryDateYamato = ExtractDeliveryDateYamato(responseYamato);
                    if (deliveryDateYamato.HasValue && deliveryDateYamato < message.CreatedDate)
                    {
                        deliveryDateYamato = deliveryDateYamato.Value.AddYears(1);
                    }
                    if (responseYamato.Contains("配達完了"))
                    {
                        //await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSAsync(order.Id, 10, message.CreatedBy, message.CreatedName);
                        await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERYAsync(message.Id, deliveryDateYamato, 10, message.CreatedBy, message.CreatedName);
                    }
                    else
                    {
                        //await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSAsync(order.Id, 0, message.CreatedBy, message.CreatedName);
                        await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERYAsync(message.Id, deliveryDateYamato, 0, message.CreatedBy, message.CreatedName);

                    }
                    break;
                case "JapanPost":
                    var responseJapanPost = await _carrierJapanRepository.GetJapanPost(message.DomesticTracking.Replace("-", ""));
                    DateTime? deliveryDateJapanPost = ExtractDeliveryDateJapanPost(responseJapanPost);
                    if (deliveryDateJapanPost.HasValue && deliveryDateJapanPost < message.CreatedDate)
                    {
                        deliveryDateJapanPost = deliveryDateJapanPost.Value.AddYears(1);
                    }
                    if (responseJapanPost.Contains("お届け先にお届け済み"))
                    {
                        //await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSAsync(order.Id, 10, message.CreatedBy, message.CreatedName);
                        await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERYAsync(message.Id, deliveryDateJapanPost, 10, message.CreatedBy, message.CreatedName);

                    }
                    else
                    {
                        //await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSAsync(order.Id, 0, message.CreatedBy, message.CreatedName);
                        await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERYAsync(message.Id, deliveryDateJapanPost, 0, message.CreatedBy, message.CreatedName);


                    }
                    break;
                case "Sagawa":
                    var responseSagawa = await _carrierJapanRepository.GetSagawa(message.DomesticTracking.Replace("-", ""));
                    DateTime? deliveryDateSagawa = ExtractDeliveryDateSagawa(responseSagawa);
                    if (deliveryDateSagawa.HasValue && deliveryDateSagawa < message.CreatedDate)
                    {
                        deliveryDateSagawa = deliveryDateSagawa.Value.AddYears(1);
                    }
                    if (responseSagawa.Contains("お荷物のお届けが完了致しました") || responseSagawa.Contains("&#12362;&#33655;&#29289;&#12398;&#12362;&#23626;&#12369;&#12364;&#23436;&#20102;&#33268;&#12375;&#12414;&#12375;&#12383;&#12290;"))
                    {
                        //await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSAsync(order.Id, 10, message.CreatedBy, message.CreatedName);
                        await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERYAsync(message.Id, deliveryDateSagawa, 10, message.CreatedBy, message.CreatedName);

                    }
                    else
                    {
                        //await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSAsync(order.Id, 0, message.CreatedBy, message.CreatedName);
                        await procedures.SP_ORDER_UPDATE_DOMESTIC_DELIVERYAsync(message.Id, deliveryDateSagawa, 0, message.CreatedBy, message.CreatedName);

                    }
                    break;
                default:
                    break;
            }
        }

        public DateTime? ExtractDeliveryDateJapanPost(string responseHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseHtml);
            var rows = htmlDoc.QuerySelectorAll("table[summary='履歴情報'] tr");
            foreach (var row in rows)
            {

                var statusColumn = row.QuerySelector("td:nth-child(2)");
                if (statusColumn != null && statusColumn.InnerText.Contains("お届け先にお届け済み"))
                {

                    var dateColumn = row.QuerySelector("td:nth-child(1)");
                    if (dateColumn != null)
                    {

                        string datePart = dateColumn.InnerText.Trim();
                        if (DateTime.TryParseExact(datePart, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deliveryDate))
                        {
                            return deliveryDate;
                        }

                    }
                }
            }

            return null;
        }
        public DateTime? ExtractDeliveryDateYamato(string responseHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseHtml);
            var deliveryStatus = htmlDoc.QuerySelector(".tracking-invoice-block-detail");
            if (deliveryStatus != null)
            {
                var listItems = deliveryStatus.QuerySelectorAll("li");
                foreach (var listItem in listItems)
                {
                    var itemNode = listItem.QuerySelector(".item");
                    if (itemNode != null && itemNode.InnerText.Trim() == "配達完了")
                    {
                        var parsedDeliveryDate = listItem.QuerySelector(".date");
                        if (parsedDeliveryDate != null)
                        {
                            var dateText = parsedDeliveryDate.InnerText.Trim();
                            var parts = dateText.Split(new[] { '月', '日', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length == 3)
                            {
                                string month = parts[0];
                                string day = parts[1];
                                string time = parts[2];
                                var datePart = $"{month}/{day} {time}";
                                if (DateTime.TryParseExact(datePart, "MM/dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deliveryDate))
                                {
                                    return deliveryDate;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        public DateTime? ExtractDeliveryDateSagawa(string responseHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseHtml);
            var deliveryCompletedRow = htmlDoc.QuerySelector(".table_okurijo_index .ttl02");
            var deliveryDateNode = deliveryCompletedRow.QuerySelector("dl.okurijo_info dd");

            if (deliveryDateNode != null)
            {
                string dateText = HttpUtility.HtmlDecode(deliveryDateNode.InnerText.Trim());
                var parts = dateText.Split(new[] { ' ', '　', '日', '月', '時', '分' }, StringSplitOptions.RemoveEmptyEntries);


                if (parts.Length >= 4)
                {
                    string day = parts[1];
                    string month = parts[0];
                    string hour = parts[2];
                    string minute = parts[3];
                    var datePart = $"{month}/{day} {hour}:{minute}";
                    if (DateTime.TryParseExact(datePart, "MM/dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deliveryDate))
                    {
                        return deliveryDate;
                    }
                }
            }
            return null;
        }


    }


    
}
