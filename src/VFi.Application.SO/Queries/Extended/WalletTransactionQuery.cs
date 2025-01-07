using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries.Extended;

public class WalletTransactionQueryAll : IQuery<IEnumerable<WalletTransactionDto>>
{
    public WalletTransactionQueryAll()
    {
    }
}

public class WalletTransactionQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public WalletTransactionQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class WalletTransactionQueryCheckCode : IQuery<bool>
{

    public WalletTransactionQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class WalletTransactionQueryById : IQuery<WalletTransactionDto>
{
    public WalletTransactionQueryById()
    {
    }

    public WalletTransactionQueryById(Guid walletTransactionId)
    {
        WalletTransactionId = walletTransactionId;
    }

    public Guid WalletTransactionId { get; set; }
}
public class WalletTransactionPagingQuery : FopQuery, IQuery<PagedResult<List<WalletTransactionDto>>>
{
    public WalletTransactionPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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
public class WalletTransactionByAccountQuery : IQuery<PagedResult<List<WalletTransactionDto>>>
{

    public Guid AccountId { get; set; }
    public string Type { get; set; }
    public string Wallet { get; set; }
    public int PageNumber { get; set; }

    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }

}

public class WalletTransactionQueryHandler : IQueryHandler<WalletTransactionQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<WalletTransactionQueryAll, IEnumerable<WalletTransactionDto>>,
                                         IQueryHandler<WalletTransactionQueryCheckCode, bool>,
                                         IQueryHandler<WalletTransactionQueryById, WalletTransactionDto>,
                                         IQueryHandler<WalletTransactionPagingQuery, PagedResult<List<WalletTransactionDto>>>,
                                        IQueryHandler<WalletTransactionByAccountQuery, PagedResult<List<WalletTransactionDto>>>
{
    private readonly IWalletTransactionRepository _repository;
    public WalletTransactionQueryHandler(IWalletTransactionRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(WalletTransactionQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<WalletTransactionDto> Handle(WalletTransactionQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.WalletTransactionId);
        var result = new WalletTransactionDto()
        {
            Id = item.Id,
            Code = item.Code,
            Amount = item.Amount,
            AccountId = item.AccountId,
            WalletId = item.WalletId,
            Type = item.Type,
            Method = item.Method,
            Status = item.Status,
            ApplyDate = item.ApplyDate,
            RawData = item.RawData,
            Balance = item.Balance,
            RefId = item.RefId,
            RefType = item.RefType,
            RefundStatus = item.RefundStatus,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<WalletTransactionDto>>> Handle(WalletTransactionPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<WalletTransactionDto>>();
        var fopRequest = FopExpressionBuilder<WalletTransaction>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new WalletTransactionDto()
        {
            Id = item.Id,
            Code = item.Code,
            Amount = item.Amount,
            AccountId = item.AccountId,
            WalletId = item.WalletId,
            Type = item.Type,
            Method = item.Method,
            Note = item.Note,
            Status = item.Status,
            ApplyDate = item.ApplyDate,
            RefundStatus = item.RefundStatus,
            RawData = item.RawData,
            Balance = item.Balance,
            RefId = item.RefId,
            RefType = item.RefType,
            RefCode = item.RefCode,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<WalletTransactionDto>>> Handle(WalletTransactionByAccountQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<WalletTransactionDto>>();
        var filter = new Dictionary<string, object>();
        if (request.Status.HasValue)
            filter.Add("status", request.Status.Value);
        if (!string.IsNullOrEmpty(request.Type))
            filter.Add("type", request.Type);
        filter.Add("accountId", request.AccountId);
        var items = await _repository.Filter(request.Keyword, filter, request.PageSize, request.PageNumber);
        var count = await _repository.FilterCount(request.Keyword, filter);
        var data = items.Select(item => new WalletTransactionDto()
        {
            Id = item.Id,
            Code = item.Code,
            Amount = item.Amount,
            AccountId = item.AccountId,
            WalletId = item.WalletId,
            Type = item.Type,
            Method = item.Method,
            Note = item.Note,
            Status = item.Status,
            ApplyDate = item.ApplyDate,
            RawData = item.RawData,
            Balance = item.Balance,
            RefId = item.RefId,
            RefType = item.RefType,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<IEnumerable<WalletTransactionDto>> Handle(WalletTransactionQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new WalletTransactionDto()
        {
            Id = item.Id,
            Code = item.Code,
            Amount = item.Amount,
            AccountId = item.AccountId,
            WalletId = item.WalletId,
            Type = item.Type,
            Method = item.Method,
            Note = item.Note,
            Status = item.Status,
            ApplyDate = item.ApplyDate,
            RawData = item.RawData,
            Balance = item.Balance,
            RefId = item.RefId,
            RefType = item.RefType,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(WalletTransactionQueryComboBox request, CancellationToken cancellationToken)
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
            Label = x.RawData
        });
        return result;
    }
}
