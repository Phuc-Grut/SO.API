using DocumentFormat.OpenXml.Office.Word;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class LeadQuerySendTransactionByTo : IQuery<IEnumerable<SendTransactionDto>>
{
    public LeadQuerySendTransactionByTo(string keyword, string to)
    {
        Keyword = keyword;
        To = to;
    }

    public string Keyword { get; set; }
    public string To { get; set; }
}

public class LeadEmailBuilderQuery : IQuery<EmailBodyDto>
{
    public LeadEmailBuilderQuery()
    {
    }
    public string Template { get; set; }
    public string Subject { get; set; }
    public string JBody { get; set; }
}
public class LeadQueryAll : IQuery<IEnumerable<LeadDto>>
{
    public LeadQueryAll()
    {
    }
}
public class LeadQuerySendTemplateListbox : IQuery<IEnumerable<SendTemplateComboboxDto>>
{
    public LeadQuerySendTemplateListbox()
    {
    }
}

public class LeadQuerySendConfigListbox : IQuery<IEnumerable<SendConfigComboboxDto>>
{
    public LeadQuerySendConfigListbox()
    {
    }
}
public class LeadQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public LeadQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class LeadQueryCheckCode : IQuery<bool>
{

    public LeadQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class LeadQueryById : IQuery<LeadDto>
{
    public LeadQueryById()
    {
    }

    public LeadQueryById(Guid leadId)
    {
        LeadId = leadId;
    }

    public Guid LeadId { get; set; }
}
public class LeadPagingQuery : FopQuery, IQuery<PagedResult<List<LeadDto>>>
{
    public LeadPagingQuery(string? keyword, int? status, int? convert, string tags, Guid? campaignId, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
        CampaignId = campaignId;
        Convert = convert;
        Tags = tags;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }
    public Guid? CampaignId { get; set; }
    public int? Convert { get; set; }
    public string? Tags { get; set; }
}

public class LeadQueryHandler : IQueryHandler<LeadQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<LeadQueryAll, IEnumerable<LeadDto>>,
                                         IQueryHandler<LeadQueryCheckCode, bool>,
                                         IQueryHandler<LeadQueryById, LeadDto>,
                                         IQueryHandler<LeadPagingQuery, PagedResult<List<LeadDto>>>,
                                         IQueryHandler<LeadQuerySendConfigListbox, IEnumerable<SendConfigComboboxDto>>,
                                         IQueryHandler<LeadQuerySendTemplateListbox, IEnumerable<SendTemplateComboboxDto>>,
                                         IQueryHandler<LeadEmailBuilderQuery, EmailBodyDto>,
                                         IQueryHandler<LeadQuerySendTransactionByTo, IEnumerable<SendTransactionDto>>
{
    private readonly ILeadRepository _repository;
    private readonly ILeadCampaignRepository _leadCampaignRepository;
    private readonly IEmailMasterRepository _emailMasterRepository;
    public LeadQueryHandler(ILeadRepository leadRespository, ILeadCampaignRepository leadCampaignRepository, IEmailMasterRepository emailMasterRepository)
    {
        _repository = leadRespository;
        _leadCampaignRepository = leadCampaignRepository;
        _emailMasterRepository = emailMasterRepository;
    }
    public async Task<bool> Handle(LeadQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<LeadDto> Handle(LeadQueryById request, CancellationToken cancellationToken)
    {
        var obj = await _repository.GetById(request.LeadId);
        var result = new LeadDto()
        {
            Id = obj.Id,
            Source = obj.Source,
            Code = obj.Code,
            Image = obj.Image,
            Name = obj.Name,
            Email = obj.Email,
            Phone = obj.Phone,
            Country = obj.Country,
            Province = obj.Province,
            District = obj.District,
            Ward = obj.Ward,
            ZipCode = obj.ZipCode,
            Address = obj.Address,
            Website = obj.Website,
            TaxCode = obj.TaxCode,
            BusinessSector = obj.BusinessSector,
            Company = obj.Company,
            CompanyPhone = obj.CompanyPhone,
            CompanyName = obj.CompanyName,
            CompanySize = obj.CompanySize,
            Capital = obj.Capital,
            EstablishedDate = obj.EstablishedDate,
            Tags = obj.Tags,
            Note = obj.Note,
            Status = obj.Status,
            GroupId = obj.GroupId,
            Group = obj.Group,
            EmployeeId = obj.EmployeeId,
            Employee = obj.Employee,
            GroupEmployeeId = obj.GroupEmployeeId,
            GroupEmployee = obj.GroupEmployee,
            Gender = obj.Gender,
            Year = obj.Year,
            Month = obj.Month,
            Day = obj.Day,
            Facebook = obj.Facebook,
            Zalo = obj.Zalo,
            RevenueTarget = obj.RevenueTarget,
            Revenue = obj.Revenue,
            Scale = obj.Scale,
            Difficult = obj.Difficult,
            Point = obj.Point,
            Priority = obj.Priority,
            Demand = obj.Demand,
            DynamicData = obj.DynamicData,
            Converted = obj.Converted,
            CustomerCode = obj.CustomerCode,
            CreatedDate = obj.CreatedDate,
            CreatedBy = obj.CreatedBy,
            UpdatedDate = obj.UpdatedDate,
            UpdatedBy = obj.UpdatedBy,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<LeadDto>>> Handle(LeadPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<LeadDto>>();

        var fopRequest = FopExpressionBuilder<Lead>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        string id = null;
        if (request.CampaignId is not null)
        {
            var filter = new Dictionary<string, object>();
            filter.Add("campaignId", request.CampaignId);
            var leadCampaign = await _leadCampaignRepository.Filter(filter);
            if (leadCampaign.Count() > 0)
            {
                id = string.Join(",", leadCampaign.Select(x => x.LeadId).ToList());
            }
        }

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, request.Convert, request.Tags, id, fopRequest);
        var data = datas.Select(item => new LeadDto()
        {
            Id = item.Id,
            Source = item.Source,
            Code = item.Code,
            Image = item.Image,
            Name = item.Name,
            Email = item.Email,
            Phone = item.Phone,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            ZipCode = item.ZipCode,
            Address = item.Address,
            Website = item.Website,
            TaxCode = item.TaxCode,
            BusinessSector = item.BusinessSector,
            Company = item.Company,
            CompanyPhone = item.CompanyPhone,
            CompanyName = item.CompanyName,
            CompanySize = item.CompanySize,
            Capital = item.Capital,
            EstablishedDate = item.EstablishedDate,
            Tags = item.Tags,
            Note = item.Note,
            Status = item.Status,
            GroupId = item.GroupId,
            Group = item.Group,
            EmployeeId = item.EmployeeId,
            Employee = item.Employee,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployee = item.GroupEmployee,
            Gender = item.Gender,
            Year = item.Year,
            Month = item.Month,
            Day = item.Day,
            Facebook = item.Facebook,
            Zalo = item.Zalo,
            RevenueTarget = item.RevenueTarget,
            Revenue = item.Revenue,
            Scale = item.Scale,
            Difficult = item.Difficult,
            Point = item.Point,
            Priority = item.Priority,
            //Demand = item.Demand,
            DynamicData = item.DynamicData,
            Converted = item.Converted,
            CustomerCode = item.CustomerCode,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<LeadDto>> Handle(LeadQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new LeadDto()
        {
            Id = item.Id,
            Source = item.Source,
            Code = item.Code,
            Image = item.Image,
            Name = item.Name,
            Email = item.Email,
            Phone = item.Phone,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            ZipCode = item.ZipCode,
            Address = item.Address,
            Website = item.Website,
            TaxCode = item.TaxCode,
            BusinessSector = item.BusinessSector,
            Company = item.Company,
            CompanyPhone = item.CompanyPhone,
            CompanyName = item.CompanyName,
            CompanySize = item.CompanySize,
            Capital = item.Capital,
            EstablishedDate = item.EstablishedDate,
            Tags = item.Tags,
            Note = item.Note,
            Status = item.Status,
            GroupId = item.GroupId,
            Group = item.Group,
            EmployeeId = item.EmployeeId,
            Employee = item.Employee,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployee = item.GroupEmployee,
            Gender = item.Gender,
            Year = item.Year,
            Month = item.Month,
            Day = item.Day,
            Facebook = item.Facebook,
            Zalo = item.Zalo,
            RevenueTarget = item.RevenueTarget,
            Revenue = item.Revenue,
            Scale = item.Scale,
            Difficult = item.Difficult,
            Point = item.Point,
            Priority = item.Priority,
            Demand = item.Demand,
            DynamicData = item.DynamicData,
            Converted = item.Converted,
            CustomerCode = item.CustomerCode,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(LeadQueryComboBox request, CancellationToken cancellationToken)
    {

        var items = await _repository.GetListCbx(request.Status);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }

    public async Task<IEnumerable<SendConfigComboboxDto>> Handle(LeadQuerySendConfigListbox request, CancellationToken cancellationToken)
    {
        var datas = await _emailMasterRepository.GetListboxSendConfig();
        var result = datas.Select(x => new SendConfigComboboxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            From = x.From,
            FromName = x.FromName,
        });
        return result;
    }

    public async Task<IEnumerable<SendTemplateComboboxDto>> Handle(LeadQuerySendTemplateListbox request, CancellationToken cancellationToken)
    {
        var datas = await _emailMasterRepository.GetListboxSendTemplate();
        var result = datas.Select(x => new SendTemplateComboboxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            Description = x.Description,
            Status = x.Status,
            Type = x.Type,
        });
        return result;
    }

    public async Task<EmailBodyDto> Handle(LeadEmailBuilderQuery request, CancellationToken cancellationToken)
    {
        var data = await _emailMasterRepository.EmailBuilder(request.Subject, request.JBody, request.Template);

        var result = new EmailBodyDto
        {
            Body = data.Body,
            BodyText = data.BodyText,
            Subject = data.Subject,
        };

        return result;
    }

    public async Task<IEnumerable<SendTransactionDto>> Handle(LeadQuerySendTransactionByTo request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, string?> {
            { "keyword", request.Keyword },
            { "to", request.To }
        };
        var data = await _emailMasterRepository.GetListSendTransaction(filter);
        var result = data.Select(x => new SendTransactionDto
        {
            Id = x.Id,
            Subject = x.Subject,
            SendDate = x.SendDate,
            Open = x.Open,
            Click = x.Click,
        });

        return result;
    }
}
