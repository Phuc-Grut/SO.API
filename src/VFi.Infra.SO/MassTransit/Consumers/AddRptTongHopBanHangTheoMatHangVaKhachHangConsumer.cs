using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VFi.Domain.SO.Events;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.ACC.MassTransit.Consumers;

public class AddRptTongHopBanHangTheoMatHangVaKhachHangConsumer : IConsumer<AddRptTongHopBanHangTheoMatHangVaKhachHangQueueEvent>
{
    private readonly ILogger<AddRptTongHopBanHangTheoMatHangVaKhachHangQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;
    public AddRptTongHopBanHangTheoMatHangVaKhachHangConsumer(ILogger<AddRptTongHopBanHangTheoMatHangVaKhachHangQueueEvent> logger, IServiceProvider serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }


    public async Task Consume(ConsumeContext<AddRptTongHopBanHangTheoMatHangVaKhachHangQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));

        var repository = _serviceFactory.GetService(typeof(ISOContextProcedures)) as ISOContextProcedures;

        var _ = await repository.SP_Rpt_TongHopBanHangTheoMatHangVaKhachHang_LoadDataAsync(
            msg.ReportId,
            msg.CustomerCode,
            msg.EmployeeId,
            msg.CategoryRootId,
            msg.ProductCode,
            msg.FromDate,
            msg.ToDate
            );
    }
}


