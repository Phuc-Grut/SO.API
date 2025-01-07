using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class BankQueryAll : IQuery<IEnumerable<BankDto>>
{
    public BankQueryAll()
    {
    }
}

public class BankQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public BankQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class BankQueryCheckCode : IQuery<bool>
{

    public BankQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class BankQueryById : IQuery<BankDto>
{
    public BankQueryById()
    {
    }

    public BankQueryById(Guid bankId)
    {
        BankId = bankId;
    }

    public Guid BankId { get; set; }
}
public class BankPagingQuery : FopQuery, IQuery<PagedResult<List<BankDto>>>
{
    public BankPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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

public class BankQueryHandler : IQueryHandler<BankQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<BankQueryAll, IEnumerable<BankDto>>,
                                         IQueryHandler<BankQueryCheckCode, bool>,
                                         IQueryHandler<BankQueryById, BankDto>,
                                         IQueryHandler<BankPagingQuery, PagedResult<List<BankDto>>>
{
    private readonly IBankRepository _bankRepository;
    public BankQueryHandler(IBankRepository bankRespository)
    {
        _bankRepository = bankRespository;
    }
    public async Task<bool> Handle(BankQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _bankRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<BankDto> Handle(BankQueryById request, CancellationToken cancellationToken)
    {
        var item = await _bankRepository.GetById(request.BankId);
        var result = new BankDto()
        {
            Id = item.Id,
            Code = item.Code,
            Qrbin = item.Qrbin,
            ShortName = item.ShortName,
            Name = item.Name,
            EnglishName = item.EnglishName,
            Address = item.Address,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            Note = item.Note,
            Image = item.Image,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<BankDto>>> Handle(BankPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<BankDto>>();

        var fopRequest = FopExpressionBuilder<Bank>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _bankRepository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new BankDto()
        {
            Id = item.Id,
            Code = item.Code,
            Qrbin = item.Qrbin,
            ShortName = item.ShortName,
            Name = item.Name,
            EnglishName = item.EnglishName,
            Address = item.Address,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            Note = item.Note,
            Image = item.Image,
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

    public async Task<IEnumerable<BankDto>> Handle(BankQueryAll request, CancellationToken cancellationToken)
    {
        var datas = await _bankRepository.GetAll();
        var result = datas.Select(item => new BankDto()
        {
            Id = item.Id,
            Code = item.Code,
            Qrbin = item.Qrbin,
            ShortName = item.ShortName,
            Name = item.Name,
            EnglishName = item.EnglishName,
            Address = item.Address,
            DisplayOrder = item.DisplayOrder,
            Status = item.Status,
            Note = item.Note,
            Image = item.Image,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(BankQueryComboBox request, CancellationToken cancellationToken)
    {

        var datas = await _bankRepository.GetListCbx(request.Status);
        var result = datas.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
