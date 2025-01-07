using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeadController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<LeadController> _logger;

    public LeadController(IMediatorHandler mediator, IContextUser context, ILogger<LeadController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy thông tin
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new LeadQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-send-transaction")]
    public async Task<IActionResult> GetListSendTransactionByTo([FromQuery] LeadSendTransactionRequest request)
    {
        var result = await _mediator.Send(new LeadQuerySendTransactionByTo(request.Keyword, request.To));

        return Ok(result);
    }

    [HttpGet("get-listbox-send-template")]
    public async Task<IActionResult> GetListboxSendTemplate()
    {
        var result = await _mediator.Send(new LeadQuerySendTemplateListbox());

        return Ok(result);
    }

    [HttpGet("get-listbox-send-config")]
    public async Task<IActionResult> GetListboxSendConfig()
    {
        var result = await _mediator.Send(new LeadQuerySendConfigListbox());

        return Ok(result);
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new LeadQueryComboBox(request.Status));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] LeadRequest request)
    {
        var query = new LeadPagingQuery(request.Keyword, request.Status, request.Convert, request.Tags, request.campaignId, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddLeadRequest request)
    {
        var Code = request.Code;
        if (request.IsAuto == true)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var LeadAddCommand = new LeadAddCommand(
          Guid.NewGuid(),
          request.Source,
          Code,
          request.Image,
          request.Name,
          request.Email,
          request.Phone,
          request.Country,
          request.Province,
          request.District,
          request.Ward,
          request.ZipCode,
          request.Address,
          request.Website,
          request.TaxCode,
          request.BusinessSector,
          request.Company,
          request.CompanyPhone,
          request.CompanyName,
          request.CompanySize,
          request.Capital,
          request.EstablishedDate,
          request.Tags,
          request.Note,
          request.Status,
          request.GroupId,
          request.Group,
          request.EmployeeId,
          request.Employee,
          request.GroupEmployeeId,
          request.GroupEmployee,
          request.Gender,
          request.Year,
          request.Month,
          request.Day,
          request.Facebook,
          request.Zalo,
          request.RevenueTarget,
          request.Revenue,
          request.Scale,
          request.Difficult,
          request.Point,
          request.Priority,
          request.Demand,
          request.DynamicData,
          request.Converted
      );
        var result = await _mediator.SendCommand(LeadAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditLeadRequest request)
    {
        var LeadId = new Guid(request.Id);
        var LeadEditCommand = new LeadEditCommand(
           LeadId,
           request.Source,
           request.Code,
           request.Image,
           request.Name,
           request.Email,
           request.Phone,
           request.Country,
           request.Province,
           request.District,
           request.Ward,
           request.ZipCode,
           request.Address,
           request.Website,
           request.TaxCode,
           request.BusinessSector,
           request.Company,
           request.CompanyPhone,
           request.CompanyName,
           request.CompanySize,
           request.Capital,
           request.EstablishedDate,
           request.Tags,
           request.Note,
           request.Status,
           request.GroupId,
           request.Group,
           request.EmployeeId,
           request.Employee,
           request.GroupEmployeeId,
           request.GroupEmployee,
           request.Gender,
           request.Year,
           request.Month,
           request.Day,
           request.Facebook,
           request.Zalo,
           request.RevenueTarget,
           request.Revenue,
           request.Scale,
           request.Difficult,
           request.Point,
           request.Priority,
           request.Demand,
           request.DynamicData,
           request.Converted,
           request.CustomerCode
       );
        var result = await _mediator.SendCommand(LeadEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var LeadId = new Guid(id);
        if (await _mediator.Send(new LeadQueryById(LeadId)) == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var result = await _mediator.SendCommand(new LeadDeleteCommand(LeadId));

        return Ok(result);
    }

    [HttpPut("convertlead")]
    public async Task<IActionResult> ConvertLead([FromBody] EditLeadRequest request)
    {
        var LeadId = new Guid(request.Id);
        var ConvertLeadCommand = new ConvertLeadCommand(
           LeadId,
           request.Source,
           request.Code,
           request.Image,
           request.Name,
           request.Email,
           request.Phone,
           request.Country,
           request.Province,
           request.District,
           request.Ward,
           request.ZipCode,
           request.Address,
           request.Website,
           request.TaxCode,
           request.BusinessSector,
           request.Company,
           request.CompanyPhone,
           request.CompanyName,
           request.CompanySize,
           request.Capital,
           request.EstablishedDate,
           request.Tags,
           request.Note,
           request.Status,
           request.GroupId,
           request.Group,
           request.EmployeeId,
           request.Employee,
           request.GroupEmployeeId,
           request.GroupEmployee,
           request.Gender,
           request.Year,
           request.Month,
           request.Day,
           request.Facebook,
           request.Zalo,
           request.RevenueTarget,
           request.Revenue,
           request.Scale,
           request.Difficult,
           request.Point,
           request.Priority,
           request.Demand,
           request.DynamicData,
           request.Converted,
           request.CustomerCode
       );
        var result = await _mediator.SendCommand(ConvertLeadCommand);

        return Ok(result);
    }

    //[HttpPost("builder")]
    //public async Task<IActionResult> Builder([FromBody] EmailBuilderRequest request)
    //{
    //    var query = new LeadEmailBuilderQuery()
    //    {
    //        Subject = request.Subject,
    //        Template = request.Template,
    //        JBody = request.JBody
    //    };
    //    var result = await _mediator.Send(query);
    //    return Ok(result);
    //}

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailNotifyRequest request)
    {
        var data = new LeadEmailNotifyCommand(
            request.SenderCode,
            request.SenderName,
            request.Subject,
            request.From,
            request.To,
            request.CC,
            request.BCC,
            request.Body,
            request.TemplateCode
            );
        var result = await _mediator.SendCommand(data);
        return Ok(result);
    }

    [HttpPost("builder")]
    public async Task<IActionResult> Builder([FromBody] EmailBuilderRequest request)
    {
        var query = new LeadEmailBuilderQuery()
        {
            Subject = request.Subject,
            Template = request.Template,
            JBody = request.JBody
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
