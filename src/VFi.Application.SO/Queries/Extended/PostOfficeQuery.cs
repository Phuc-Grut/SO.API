using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PostOfficeQueryAll : IQuery<IEnumerable<PostOfficeDto>>
{
    public PostOfficeQueryAll()
    {
    }
}

public class PostOfficeQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PostOfficeQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class PostOfficeQueryCheckCode : IQuery<bool>
{

    public PostOfficeQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PostOfficeQueryById : IQuery<PostOfficeDto>
{
    public PostOfficeQueryById(Guid postOfficeId)
    {
        PostOfficeId = postOfficeId;
    }
    public Guid PostOfficeId { get; set; }
}
public class PostOfficeQueryByCode : IQuery<PostOfficeDto>
{
    public PostOfficeQueryByCode(string code)
    {
        Code = code;
    }
    public string Code { get; set; }
}
public class MyPostOfficeQueryByCode : IQuery<PostOfficeDto>
{
    public MyPostOfficeQueryByCode(string code)
    {
        Code = code;
    }
    public string Code { get; set; }
    public Guid AccountId { get; set; }
}
public class PostOfficePagingQuery : FopQuery, IQuery<PagedResult<List<PostOfficeDto>>>
{
    public PostOfficePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
    public string Filter { get; set; }
    public string Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }

}


public class PostOfficeQueryHandler : IQueryHandler<PostOfficeQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<PostOfficeQueryAll, IEnumerable<PostOfficeDto>>,
                                         IQueryHandler<PostOfficeQueryCheckCode, bool>,
                                         IQueryHandler<PostOfficeQueryById, PostOfficeDto>, IQueryHandler<MyPostOfficeQueryByCode, PostOfficeDto>,
                                         IQueryHandler<PostOfficeQueryByCode, PostOfficeDto>,
                                         IQueryHandler<PostOfficePagingQuery, PagedResult<List<PostOfficeDto>>>
{
    private readonly IPostOfficeRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    public PostOfficeQueryHandler(IPostOfficeRepository repository, ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
        _repository = repository;
    }
    public async Task<bool> Handle(PostOfficeQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<PostOfficeDto> Handle(PostOfficeQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PostOfficeId);
        var result = new PostOfficeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            ShortName = item.ShortName,
            Country = item.Country,
            Address = item.Address,
            Address1 = item.Address1,
            PostCode = item.PostCode,
            Phone = item.Phone,
            SyntaxSender = item.SyntaxSender,
            Note = item.Note,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }
    public async Task<PostOfficeDto> Handle(PostOfficeQueryByCode request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByCode(request.Code);
        var result = new PostOfficeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            ShortName = item.ShortName,
            Country = item.Country,
            Address = item.Address,
            Address1 = item.Address1,
            PostCode = item.PostCode,
            Phone = item.Phone,
            SyntaxSender = item.SyntaxSender,
            Note = item.Note,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }
    public async Task<PostOfficeDto> Handle(MyPostOfficeQueryByCode request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByCode(request.Code);

        var emp = await _customerRepository.GetByAccountId(request.AccountId);

        var result = new PostOfficeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            ShortName = item.ShortName,
            Country = item.Country,
            Address = item.Address,
            Address1 = item.Address1,
            PostCode = item.PostCode,
            Phone = item.Phone,
            SyntaxSender = item.SyntaxSender + " " + emp.Code,
            Note = item.Note,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<PostOfficeDto>>> Handle(PostOfficePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PostOfficeDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<PostOffice>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new PostOfficeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            ShortName = item.ShortName,
            Country = item.Country,
            Address = item.Address,
            Address1 = item.Address1,
            PostCode = item.PostCode,
            Phone = item.Phone,
            SyntaxSender = item.SyntaxSender,
            Note = item.Note,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<PostOfficeDto>> Handle(PostOfficeQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PostOfficeDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            ShortName = item.ShortName,
            Country = item.Country,
            Address = item.Address,
            Address1 = item.Address1,
            PostCode = item.PostCode,
            Phone = item.Phone,
            SyntaxSender = item.SyntaxSender,
            Note = item.Note,
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(PostOfficeQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.Status != null)
        {
            filter.Add("status", request.Status);
        }
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
