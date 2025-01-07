using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using Consul;
using MassTransit;
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

public class FulfillmentRequestAddExtConsumer : IConsumer<FulfillmentRequestAddExtQueueEvent>
{
    private readonly ILogger<FulfillmentRequestAddExtQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;

    public FulfillmentRequestAddExtConsumer(ILogger<FulfillmentRequestAddExtQueueEvent> logger,
         IServiceProvider serviceFactory
)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;



    }
    public async Task Consume(ConsumeContext<FulfillmentRequestAddExtQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));
        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        var repository = _serviceFactory.GetService(typeof(IWMSRepository)) as IWMSRepository;
        var _exportWarehouseRepository = _serviceFactory.GetService(typeof(IExportWarehouseRepository)) as IExportWarehouseRepository;

        var rs = await repository.FulfillmentRequestAddExtEndpoint(msg.ItemData);
        var item = await _exportWarehouseRepository.GetById(msg.Id);

        if (rs.IsValid)
        {
            item.FulfillmentRequestCode = rs.RuleSetsExecuted[0];
            _exportWarehouseRepository.Approve(item);
            await _exportWarehouseRepository.UnitOfWork.Commit();


        }

    }

}


