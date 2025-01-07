using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Models;

namespace VFi.Infra.SO.Repository;

public class EmailMasterRepository : IEmailMasterRepository
{
    private readonly MasterApiContext _apiContext;
    private const string PATH_GET_LIST_BOX_SEND_CONFIG = "/api/sendconfig/get-listbox";
    private const string PATH_GET_LIST_BOX_SEND_TEMPLATE = "/api/sendtemplate/get-listbox";
    private const string PATH_SEND_TEMPLATE_BUILDER = "/api/sendtemplate/builder";
    private const string PATH_NOTIFY_SEND_EMAIL = "/api/notify/send-email";
    private const string PATH_NOTIFY_EMAIL = "/api/notify/email";
    private const string PATH_GET_LIST_SEND_TRANSACTION = "/api/sendtransaction/get-list";
    private const string PATH_GET_BY_ID_SEND_TRANSACTION = "/api/sendtransaction/get-by-id";
    private readonly IContextUser _context;
    public EmailMasterRepository(MasterApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }


    public async Task<IEnumerable<SendConfigCombobox>> GetListboxSendConfig()
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var t = _apiContext.Client.Request(PATH_GET_LIST_BOX_SEND_CONFIG)
                .GetJsonAsync<IEnumerable<SendConfigCombobox>>().Result;

            return await Task.FromResult(t);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<SendTemplateCombobox>> GetListboxSendTemplate()
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var t = _apiContext.Client.Request(PATH_GET_LIST_BOX_SEND_TEMPLATE)
                .GetJsonAsync<IEnumerable<SendTemplateCombobox>>().Result;

            return await Task.FromResult(t);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<EmailBody> EmailBuilder(string subject, string jBody, string template)
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var t = await _apiContext.Client.Request(PATH_SEND_TEMPLATE_BUILDER)
                 .PostJsonAsync(new { Subject = subject, JBody = jBody, Template = template }).ReceiveJson<EmailBody>();
            return t;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async void SendEmail(SendEmail request)
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var t = await _apiContext.Client.Request(PATH_NOTIFY_SEND_EMAIL)
                .PostJsonAsync(request);

        }
        catch (Exception ex)
        {
        }
    }
    public async void EmailNotify(EmailNotify request)
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var t = await _apiContext.Client.Request(PATH_NOTIFY_SEND_EMAIL)
                .PostJsonAsync(request);

        }
        catch (Exception ex)
        {
        }
    }

    public async void CampaignSendEmail(CampaignSendEmail request)
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var t = await _apiContext.Client.Request(PATH_NOTIFY_SEND_EMAIL)
                .PostJsonAsync(request);

        }
        catch (Exception ex)
        {
        }
    }

    public async Task<List<SendTransactionLog>> GetListSendTransaction(Dictionary<string, string?> filter)
    {
        _apiContext.Token = _context.GetToken();
        var t = new List<SendTransactionLog>();

        var request = _apiContext.Client.Request(PATH_GET_LIST_SEND_TRANSACTION);

        foreach (var param in filter)
        {
            if (param.Key.Equals("keyword"))
            {
                request = request.SetQueryParam(param.Key, param.Value);
            }
            if (param.Key.Equals("to"))
            {
                request = request.SetQueryParam(param.Key, param.Value);
            }
            if (param.Key.Equals("order"))
            {
                request = request.SetQueryParam(param.Key, param.Value);
            }
            if (param.Key.Equals("campaign"))
            {
                request = request.SetQueryParam(param.Key, param.Value);
            }
            if (param.Key.Equals("quotation"))
            {
                request = request.SetQueryParam(param.Key, param.Value);
            }
        }

        var resultList = await request.GetJsonAsync<IEnumerable<SendTransactionLog>>();
        t.AddRange(resultList);

        return t;
    }

    public async Task<SendTransaction> GetSendTransactionById(Guid sendTransactionId)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request($"{PATH_GET_BY_ID_SEND_TRANSACTION}/{sendTransactionId}")
                 .GetJsonAsync<SendTransaction>().Result;
        return await Task.FromResult(t);
    }
}
