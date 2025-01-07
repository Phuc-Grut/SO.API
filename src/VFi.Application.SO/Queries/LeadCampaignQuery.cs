using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class LeadCampaignSendTransactionQueryById : IQuery<SendTransactionDto>
{
    public LeadCampaignSendTransactionQueryById(Guid sendTransactionId)
    {
        SendTransactionId = sendTransactionId;
    }

    public Guid SendTransactionId { get; set; }
}

public class SendTransactionQueryByCampaign : IQuery<IEnumerable<SendTransactionLogDto>>
{
    public SendTransactionQueryByCampaign(string keyword, string campaign, string to)
    {
        Campaign = campaign;
        Keyword = keyword;
        To = to;
    }

    public string Keyword { get; set; }
    public string Campaign { get; set; }
    public string To { get; set; }
}

public class LeadCampaignQuerySendConfigCombobox : IQuery<IEnumerable<SendConfigComboboxDto>>
{
    public LeadCampaignQuerySendConfigCombobox()
    {
    }
}

public class LeadCampaignQuerySendTemplateCombobox : IQuery<IEnumerable<SendTemplateComboboxDto>>
{

    public LeadCampaignQuerySendTemplateCombobox()
    {
    }
}

public class LeadCampaignEmailBuilderQuery : IQuery<EmailBodyDto>
{
    public LeadCampaignEmailBuilderQuery()
    {
    }
    public string Template { get; set; }
    public string Subject { get; set; }
    public string JBody { get; set; }
}
public class LeadCampaignQueryAll : IQuery<IEnumerable<LeadCampaignDto>>
{
    public LeadCampaignQueryAll()
    {
    }
}

public class LeadCampaignQueryCheckCode : IQuery<bool>
{

    public LeadCampaignQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class LeadCampaignQueryById : IQuery<LeadCampaignDto>
{
    public LeadCampaignQueryById()
    {
    }

    public LeadCampaignQueryById(Guid leadCampaignId)
    {
        LeadCampaignId = leadCampaignId;
    }

    public Guid LeadCampaignId { get; set; }
}
public class LeadCampaignPagingQuery : FopQuery, IQuery<PagedResult<List<LeadCampaignDto>>>
{
    public LeadCampaignPagingQuery(string? keyword, Guid? campaignId, Guid? leader, int? status, int? isState, string? employeeId, string filter, string leadCampaign, int pageNumber, int pageSize)
    {
        Filter = filter;
        LeadCampaign = leadCampaign;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
        IsState = isState;
        Leader = leader;
        CampaignId = campaignId;
        EmployeeId = employeeId;
    }
    public string LeadCampaign { get; set; }
    public int? IsState { get; set; }
    public Guid? Leader { get; set; }
    public Guid? CampaignId { get; set; }
    public string? EmployeeId { get; set; }
}

public class LeadCampaignQueryHandler : IQueryHandler<LeadCampaignQueryAll, IEnumerable<LeadCampaignDto>>,
                                         IQueryHandler<LeadCampaignQueryById, LeadCampaignDto>,
                                         IQueryHandler<LeadCampaignPagingQuery, PagedResult<List<LeadCampaignDto>>>,
                                         IQueryHandler<LeadCampaignQuerySendConfigCombobox, IEnumerable<SendConfigComboboxDto>>,
                                         IQueryHandler<LeadCampaignQuerySendTemplateCombobox, IEnumerable<SendTemplateComboboxDto>>,
                                         IQueryHandler<LeadCampaignEmailBuilderQuery, EmailBodyDto>,
                                         IQueryHandler<SendTransactionQueryByCampaign, IEnumerable<SendTransactionLogDto>>,
                                         IQueryHandler<LeadCampaignSendTransactionQueryById, SendTransactionDto>
{
    private readonly ILeadCampaignRepository _LeadCampaignRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmailMasterRepository _emailMasterRepository;
    public LeadCampaignQueryHandler(ILeadCampaignRepository LeadCampaignRespository, IEmployeeRepository employeeRespository, IEmailMasterRepository emailMasterRepository)
    {
        _LeadCampaignRepository = LeadCampaignRespository;
        _employeeRepository = employeeRespository;
        _emailMasterRepository = emailMasterRepository;
    }

    public async Task<LeadCampaignDto> Handle(LeadCampaignQueryById request, CancellationToken cancellationToken)
    {
        var item = await _LeadCampaignRepository.GetById(request.LeadCampaignId);
        var filter = new Dictionary<string, object>();
        var em = await _employeeRepository.Filter(filter);
        var result = new LeadCampaignDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            Email = item.Email,
            Phone = item.Phone,
            Name = item.Name,
            CampaignId = item.CampaignId,
            Campaign = item.Campaign,
            StateId = item.StateId,
            State = item.State,
            Leader = item.Leader,
            LeaderName = item.LeaderName,
            Member = !string.IsNullOrEmpty(item.Member) ? item.Member.Split(',').ToList() : new List<string>(),
            ListMember = em.ToList(),
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
        };
        return result;
    }

    public async Task<PagedResult<List<LeadCampaignDto>>> Handle(LeadCampaignPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<LeadCampaignDto>>();

        var fopRequest = FopExpressionBuilder<LeadCampaign>.Build(request.Filter, request.LeadCampaign, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
            filter.Add("status", request.Status.ToString());
        if (request.CampaignId != null)
            filter.Add("campaignId", request.CampaignId);
        if (request.Leader != null)
            filter.Add("leader", request.Leader);
        if (request.IsState != null)
            filter.Add("isState", request.IsState.ToString());
        if (!string.IsNullOrEmpty(request.EmployeeId))
            filter.Add("employeeId", request.EmployeeId);
        var (datas, count) = await _LeadCampaignRepository.Filter(request.Keyword, filter, fopRequest);
        var data = datas.Select(item => new LeadCampaignDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            Email = item.Lead.Email,
            Phone = item.Lead.Phone,
            Name = item.Name,
            CampaignId = item.CampaignId,
            Campaign = item.Campaign,
            StateId = item.StateId,
            State = item.State,
            StateDate = item.StateDate,
            Leader = item.Leader,
            LeaderName = item.LeaderName,
            Member = !string.IsNullOrEmpty(item.Member) ? item.Member.Split(',').ToList() : new List<string>(),
            Status = item.Status,
            CustomerCode = item.Lead.CustomerCode,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).OrderByDescending(x => x.StateDate).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<LeadCampaignDto>> Handle(LeadCampaignQueryAll request, CancellationToken cancellationToken)
    {
        var datas = await _LeadCampaignRepository.GetAll();
        var result = datas.Select(item => new LeadCampaignDto()
        {
            Id = item.Id,
            LeadId = item.LeadId,
            Email = item.Email,
            Phone = item.Phone,
            Name = item.Name,
            CampaignId = item.CampaignId,
            Campaign = item.Campaign,
            StateId = item.StateId,
            State = item.State,
            Leader = item.Leader,
            LeaderName = item.LeaderName,
            Member = !string.IsNullOrEmpty(item.Member) ? item.Member.Split(',').ToList() : new List<string>(),
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<SendConfigComboboxDto>> Handle(LeadCampaignQuerySendConfigCombobox request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.GetListboxSendConfig();

        var result = data.Select(x => new SendConfigComboboxDto
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            From = x.From,
            FromName = x.FromName
        });

        return result;
    }

    public async Task<IEnumerable<SendTemplateComboboxDto>> Handle(LeadCampaignQuerySendTemplateCombobox request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.GetListboxSendTemplate();

        var result = data.Select(x => new SendTemplateComboboxDto
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            Description = x.Description,
            Status = x.Status,
            Type = x.Type
        });

        return result;
    }

    public async Task<EmailBodyDto> Handle(LeadCampaignEmailBuilderQuery request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.EmailBuilder(request.Subject, request.JBody, request.Template);

        var result = new EmailBodyDto
        {
            Subject = data.Subject,
            Body = data.Body,
            BodyText = data.BodyText
        };

        return result;
    }

    public async Task<IEnumerable<SendTransactionLogDto>> Handle(SendTransactionQueryByCampaign request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, string?> {
            { "keyword", request.Keyword },
            { "campaign", request.Campaign },
            { "to", request.To }
        };

        var data = await _emailMasterRepository.GetListSendTransaction(filter);
        var result = data.Select(x => new SendTransactionLogDto
        {
            Id = x.Id,
            Campaign = x.Campaign,
            Subject = x.Subject,
            SendDate = x.SendDate,
            Open = x.Open,
            Click = x.Click,
        });

        return result;
    }

    public async Task<SendTransactionDto> Handle(LeadCampaignSendTransactionQueryById request, CancellationToken cancellationToken)
    {
        var item = await _emailMasterRepository.GetSendTransactionById(request.SendTransactionId);

        var result = new SendTransactionDto()
        {
            Id = item.Id,
            SenderCode = item.SenderCode,
            Subject = item.Subject,
            From = item.From,
            To = item.To,
            Cc = item.Cc,
            Bcc = item.Bcc,
            Content = item.Content,
            Plaintext = item.Plaintext,
            Description = item.Description,
            Status = item.Status,
            Product = item.Product,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            SendDate = item.SendDate,
            Priority = item.Priority,
            Group = item.Group,
            Campaign = item.Campaign,
            Open = item.Open,
            SendPriority = item.SendPriority,
            Click = item.Click,
            ClickUrl = item.ClickUrl,
            OpenDate = item.OpenDate,
            Order = item.Order,
            Bounce = item.Bounce,
        };
        return result;
    }
}
