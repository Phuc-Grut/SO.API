using System.Data;
using MassTransit;
using MassTransit.Initializers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.MassTransit.Consumers;

public class CustomerRevenueLoadConsumer : IConsumer<CustomerRevenueLoadQueueEvent>
{
    private readonly ILogger<CustomerRevenueLoadQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;

    public CustomerRevenueLoadConsumer(ILogger<CustomerRevenueLoadQueueEvent> logger,
         IServiceProvider serviceFactory
)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;



    }
    public async Task Consume(ConsumeContext<CustomerRevenueLoadQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));
        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        //object? db = _serviceFactory.GetService(typeof(SqlCoreContext));
        var _procedures = _serviceFactory.GetService(typeof(ISOExtProcedures)) as ISOExtProcedures;
        var _repository = _serviceFactory.GetService(typeof(IPaymentInvoiceRepository)) as IPaymentInvoiceRepository;
        if (msg.AccountId.HasValue)
        {
            var result = await _procedures.SP_ACCOUNT_REVENUE_LOADAsync(msg.AccountId, null, null);
        }
        else
        {
            if (!msg.BackHour.HasValue)
                msg.BackHour = 6360;
            var startDate = DateTime.Now.AddHours(-1 * msg.BackHour.Value);
            var filter = new Dictionary<string, object>();
            filter.Add("pay_start_date", startDate);
            var invoices = await _repository.Filter(filter, 1000);
            var listAccountId = invoices.Select(x => x.AccountId).ToList().Distinct();

            foreach (var accountId in listAccountId)
            {
                var result = await _procedures.SP_ACCOUNT_REVENUE_LOADAsync(accountId, null, null);
                Thread.Sleep(500);
            }
        }




    }

}


