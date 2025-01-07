using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class TransactionQueryAll : IQuery<IEnumerable<TransactionDto>>
{
    public TransactionQueryAll()
    {
    }
}

public class TransactionQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public TransactionQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class TransactionQueryCheckCode : IQuery<bool>
{

    public TransactionQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class TransactionQueryById : IQuery<TransactionDto>
{
    public TransactionQueryById()
    {
    }

    public TransactionQueryById(Guid TransactionId)
    {
        TransactionId = TransactionId;
    }

    public Guid TransactionId { get; set; }
}
public class TransactionPagingQuery : FopQuery, IQuery<PagedResult<List<TransactionDto>>>
{
    public TransactionPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class TransactionQueryHandler : IQueryHandler<TransactionQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<TransactionQueryAll, IEnumerable<TransactionDto>>,
                                         IQueryHandler<TransactionQueryCheckCode, bool>,
                                         IQueryHandler<TransactionQueryById, TransactionDto>,
                                         IQueryHandler<TransactionPagingQuery, PagedResult<List<TransactionDto>>>
{
    private readonly ITransactionRepository _repository;
    public TransactionQueryHandler(ITransactionRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(TransactionQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<TransactionDto> Handle(TransactionQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.TransactionId);
        var result = new TransactionDto()
        {
            Id = item.Id,
            Code = item.Code,
            Amount = item.Amount,
            AccountId = item.AccountId,
            WalletId = item.WalletId,
            Type = item.Type,
            ObjectRef = item.ObjectRef,
            AuthorizeRef = item.AuthorizeRef,
            TransactionRef = item.TransactionRef,
            Source = item.Source,
            MetaData = item.MetaData,
            Status = item.Status,
            ParentId = item.ParentId,
            //ApplyDate = item.ApplyDate,
            RawData = item.RawData,
            Currency = item.Currency,
            AuthorizationTransactionId = item.AuthorizationTransactionId,
            AuthorizationTransactionCode = item.AuthorizationTransactionCode,
            AuthorizationTransactionResult = item.AuthorizationTransactionResult,
            CaptureTransactionId = item.CaptureTransactionId,
            CaptureTransactionResult = item.CaptureTransactionResult,
            RefundTransactionId = item.RefundTransactionId,
            RefundTransactionResult = item.RefundTransactionResult,
            TransactionDate = item.TransactionDate,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<TransactionDto>>> Handle(TransactionPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<TransactionDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<Transaction>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new TransactionDto()
        {
            Id = item.Id,
            Code = item.Code,
            Amount = item.Amount,
            AccountId = item.AccountId,
            WalletId = item.WalletId,
            Type = item.Type,
            ObjectRef = item.ObjectRef,
            AuthorizeRef = item.AuthorizeRef,
            TransactionRef = item.TransactionRef,
            Source = item.Source,
            MetaData = item.MetaData,
            Status = item.Status,
            ParentId = item.ParentId,
            //ApplyDate = item.ApplyDate,
            RawData = item.RawData,
            Currency = item.Currency,
            AuthorizationTransactionId = item.AuthorizationTransactionId,
            AuthorizationTransactionCode = item.AuthorizationTransactionCode,
            AuthorizationTransactionResult = item.AuthorizationTransactionResult,
            CaptureTransactionId = item.CaptureTransactionId,
            CaptureTransactionResult = item.CaptureTransactionResult,
            RefundTransactionId = item.RefundTransactionId,
            RefundTransactionResult = item.RefundTransactionResult,
            TransactionDate = item.TransactionDate,
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

    public async Task<IEnumerable<TransactionDto>> Handle(TransactionQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new TransactionDto()
        {
            Id = item.Id,
            Code = item.Code,
            Amount = item.Amount,
            AccountId = item.AccountId,
            WalletId = item.WalletId,
            Type = item.Type,
            ObjectRef = item.ObjectRef,
            AuthorizeRef = item.AuthorizeRef,
            TransactionRef = item.TransactionRef,
            Source = item.Source,
            MetaData = item.MetaData,
            Status = item.Status,
            ParentId = item.ParentId,
            //ApplyDate = item.ApplyDate,
            RawData = item.RawData,
            Currency = item.Currency,
            AuthorizationTransactionId = item.AuthorizationTransactionId,
            AuthorizationTransactionCode = item.AuthorizationTransactionCode,
            AuthorizationTransactionResult = item.AuthorizationTransactionResult,
            CaptureTransactionId = item.CaptureTransactionId,
            CaptureTransactionResult = item.CaptureTransactionResult,
            RefundTransactionId = item.RefundTransactionId,
            RefundTransactionResult = item.RefundTransactionResult,
            TransactionDate = item.TransactionDate,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(TransactionQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
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
