using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeadCampaignController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<LeadCampaignController> _logger;

    public LeadCampaignController(IMediatorHandler mediator, IContextUser context, ILogger<LeadCampaignController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-by-id-send-transaction/{id}")]
    public async Task<IActionResult> GetByIdSendTransaction(Guid id)
    {
        var rs = await _mediator.Send(new LeadCampaignSendTransactionQueryById(id));
        return Ok(rs);
    }

    [HttpGet("get-list-send-transaction")]
    public async Task<IActionResult> GetListSendTransactionByLeadCampaign([FromQuery] LeadCampaignSendTransactionRequest request)
    {
        var rs = await _mediator.Send(new SendTransactionQueryByCampaign(request.Keyword, request.Campaign, request.To));
        return Ok(rs);
    }

    [HttpGet("get-listbox-sendconfig")]
    public async Task<IActionResult> GetListboxSendConfig()
    {
        var rs = await _mediator.Send(new LeadCampaignQuerySendConfigCombobox());
        return Ok(rs);
    }

    [HttpGet("get-listbox-sendtemplate")]
    public async Task<IActionResult> GetListboxSendTemplate()
    {
        var rs = await _mediator.Send(new LeadCampaignQuerySendTemplateCombobox());
        return Ok(rs);
    }

    [HttpPost("builder")]
    public async Task<IActionResult> Builder([FromBody] EmailBuilderRequest request)
    {
        var query = new LeadCampaignEmailBuilderQuery()
        {
            Subject = request.Subject,
            Template = request.Template,
            JBody = request.JBody
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] LeadCampaignSendEmailRequest request)
    {
        var data = new LeadCampaignSendEmailCommand(
            request.Campaign,
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

    /// <summary>
    /// Lấy thông tin
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new LeadCampaignQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] LeadCampaignRequest request)
    {
        if (request.PageNumber == 0)
        {
            request.PageNumber = 1;
        }
        if (request.PageSize == 0)
        {
            request.PageSize = int.MaxValue;
        }
        if (_context.QueryMyData())
        {
            request.EmployeeId = _context.GetUserId().ToString();
        }
        var query = new LeadCampaignPagingQuery(request.Keyword, request.CampaignId, request.Leader, request.Status, request.IsState, request.EmployeeId, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddLeadCampaignRequest request)
    {
        var LeadCampaignAddCommand = new LeadCampaignAddCommand(
        Guid.NewGuid(),
        request.LeadId,
        request.Email,
        request.Phone,
        request.Name,
        request.CampaignId,
        request.Campaign,
        request.StateId,
        request.State,
        request.Leader,
        request.LeaderName,
        request.Member.Count() > 0 ? string.Join(",", request.Member) : null,
        request.Status
      );
        var result = await _mediator.SendCommand(LeadCampaignAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditLeadCampaignRequest request)
    {
        LeadCampaignDto data = await _mediator.Send(new LeadCampaignQueryById(request.Id));
        if (data == null)
            return BadRequest(new ValidationResult("Code not exists"));
        var LeadCampaignEditCommand = new LeadCampaignEditCommand(
        request.Id,
        request.LeadId,
        request.Email,
        request.Phone,
        request.Name,
        request.CampaignId,
        request.Campaign,
        request.StateId,
        request.State,
        request.Leader,
        request.LeaderName,
        request.Member.Count() > 0 ? string.Join(",", request.Member) : null,
        request.Status
      );
        var result = await _mediator.SendCommand(LeadCampaignEditCommand);
        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var sortCommand = new LeadCampaignSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(sortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var LeadCampaignId = new Guid(id);
        if (await _mediator.Send(new LeadCampaignQueryById(LeadCampaignId)) == null)
            return BadRequest(new ValidationResult("Code not exists"));

        var result = await _mediator.SendCommand(new LeadCampaignDeleteCommand(LeadCampaignId));

        return Ok(result);
    }

    [HttpPost("addlead")]
    public async Task<IActionResult> AddLead([FromBody] AddLead request)
    {
        var AddLeadCampaignCommand = new AddLeadCampaignCommand(
            request.Data.Select(x => new LeadCampaignCommand()
            {
                Id = Guid.NewGuid(),
                LeadId = x.LeadId,
                Email = x.Email,
                Phone = x.Phone,
                Name = x.Name,
                CampaignId = x.CampaignId,
                Campaign = x.Campaign,
                StateId = x.StateId,
                State = x.State,
                Leader = x.Leader,
                LeaderName = x.LeaderName,
                Status = x.Status,
                Member = x.Member.Count() > 0 ? string.Join(",", x.Member) : null,
            }
            ).ToList());

        var result = await _mediator.SendCommand(AddLeadCampaignCommand);
        return Ok(result);
    }

    [HttpPut("editlead")]
    public async Task<IActionResult> EditLead([FromBody] EditLead request)
    {
        var EditLeadCampaignCommand = new EditLeadCampaignCommand(
            request.LeadId,
            request.Email,
            request.Phone,
            request.Name,
            request.CampaignId,
            request.Campaign,
            request.StateId,
            request.State,
            request.Leader,
            request.LeaderName,
            request.Member.Count() > 0 ? string.Join(",", request.Member) : null,
            request.Status,
            request.Data.ToList()
            );

        var result = await _mediator.SendCommand(EditLeadCampaignCommand);
        return Ok(result);
    }

    [HttpPut("editstatus")]
    public async Task<IActionResult> EditLead([FromBody] EditStatus request)
    {
        var EditStatusCommand = new EditStatusCommand(
            request.Data.Select(x => new UpdateStatusCommand { Id = x.Id, Status = x.Status }).ToList()
            );

        var result = await _mediator.SendCommand(EditStatusCommand);
        return Ok(result);
    }

    [HttpPut("editstate")]
    public async Task<IActionResult> EditState([FromBody] EditState request)
    {
        var EditStateCommand = new EditStateCommand(
            request.Id,
            request.StateId,
            request.State
            );

        var result = await _mediator.SendCommand(EditStateCommand);
        return Ok(result);
    }

    [HttpPost("deletelead")]
    public async Task<IActionResult> DeleteLead([FromBody] DeleteLead request)
    {
        var DeleteLeadCampaignCommand = new DeleteLeadCampaignCommand(
            request.Data.ToList()
            );

        var result = await _mediator.SendCommand(DeleteLeadCampaignCommand);
        return Ok(result);
    }
}
