using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class WalletQueryByAccount : IQuery<IEnumerable<WalletDto>>
{
    public WalletQueryByAccount()
    {
    }
    public Guid AccountId { get; set; }
    public string WalletCode { get; set; }
    public int? Status { get; set; }
}

public class WalletQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public WalletQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class WalletQueryCheckCode : IQuery<bool>
{

    public WalletQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class WalletQueryById : IQuery<WalletDto>
{
    public WalletQueryById()
    {
    }

    public WalletQueryById(Guid WalletId)
    {
        WalletId = WalletId;
    }

    public Guid WalletId { get; set; }
}
public class WalletPagingQuery : FopQuery, IQuery<PagedResult<List<WalletDto>>>
{
    public WalletPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class WalletQueryHandler : IQueryHandler<WalletQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<WalletQueryByAccount, IEnumerable<WalletDto>>,
                                         IQueryHandler<WalletQueryById, WalletDto>,
                                         IQueryHandler<WalletPagingQuery, PagedResult<List<WalletDto>>>
{
    private readonly IWalletRepository _repository;
    private readonly IWalletTypeRepository _repositoryWalletType;
    public WalletQueryHandler(IWalletRepository repository, IWalletTypeRepository repositoryWalletType)
    {
        _repository = repository;
        _repositoryWalletType = repositoryWalletType;
    }

    public async Task<WalletDto> Handle(WalletQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.WalletId);
        var result = new WalletDto()
        {
            Id = item.Id,
            AccountId = item.AccountId,
            WalletCode = item.WalletCode,
            Cash = item.Cash.GetValueOrDefault(),
            CashHold = item.CashHold.GetValueOrDefault(),
            CashHoldBid = item.CashHoldBid.GetValueOrDefault(),
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        };
        return result;
    }

    public async Task<PagedResult<List<WalletDto>>> Handle(WalletPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<WalletDto>>();
        var filter = new Dictionary<string, object>();
        var fopRequest = FopExpressionBuilder<Wallet>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
        var data = items.Select(item => new WalletDto()
        {
            Id = item.Id,
            AccountId = item.AccountId,
            WalletCode = item.WalletCode,
            Cash = item.Cash.GetValueOrDefault(),
            CashHold = item.CashHold.GetValueOrDefault(),
            CashHoldBid = item.CashHoldBid.GetValueOrDefault(),
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
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

    public async Task<IEnumerable<WalletDto>> Handle(WalletQueryByAccount request, CancellationToken cancellationToken)
    {

        var filter = new Dictionary<string, object>();
        filter.Add("accountid", request.AccountId);
        if (!string.IsNullOrEmpty(request.WalletCode))
        {
            filter.Add("code", request.WalletCode);
        }
        if (request.Status.HasValue)
            filter.Add("status", request.Status);
        var items = await _repository.Filter("", filter, 100, 1);
        if (!items.Any())
        {
            var walletTypes = await _repositoryWalletType.GetAll();

            foreach (var w in walletTypes.Where(x => x.Status.HasValue && x.Status.Value == 1))
            {

                _repository.Add(new Wallet()
                {
                    Id = Guid.NewGuid(),
                    AccountId = request.AccountId,
                    WalletCode = w.Code,
                    Cash = 0,
                    CashHold = 0,
                    Status = 1,
                    CreatedDate = DateTime.Now

                });
            }

            items = await _repository.Filter("", filter, 100, 1);
        }
        var result = items.Select(item => new WalletDto()
        {
            Id = item.Id,
            AccountId = item.AccountId,
            WalletCode = item.WalletCode,
            Cash = item.Cash.GetValueOrDefault(),
            CashHold = item.CashHold.GetValueOrDefault(),
            CashHoldBid = item.CashHoldBid.GetValueOrDefault(),
            Status = item.Status,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedByName = item.UpdatedByName,
            CreatedByName = item.CreatedByName
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(WalletQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        var items = await _repository.GetListBox(filter);
        var result = items.Select(x => new ComboBoxDto()
        {
            Key = x.WalletCode,
            Value = x.Id,
            Label = x.WalletCode
        });
        return result;
    }
}
