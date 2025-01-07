using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class PriceListPurchaseQueryAll : IQuery<IEnumerable<PriceListPurchaseDto>>
{
    public PriceListPurchaseQueryAll()
    {
    }
}

public class PriceListPurchaseQueryComboBox : IQuery<IEnumerable<PriceListPurchaseListBoxDto>>
{
    public PriceListPurchaseQueryComboBox(PriceListPurchaseParams queryParams)
    {
        QueryParams = queryParams;
    }
    public PriceListPurchaseParams QueryParams { get; set; }
}

public class PriceListPurchaseQueryById : IQuery<PriceListPurchaseDto>
{
    public PriceListPurchaseQueryById(Guid id)
    {
        PriceListPurchaseId = id;
    }

    public Guid PriceListPurchaseId { get; set; }
}

public class PriceListPurchaseQueryByAccount : FopQuery, IQuery<PagedResult<List<PriceListPurchaseDetailDto>>>
{
    public PriceListPurchaseQueryByAccount(Guid id, string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        AccountId = id;
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public string Keyword { get; set; }

    public Guid AccountId { get; set; }
}

public class PriceListPurchasePagingQuery : FopQuery, IQuery<PagedResult<List<PriceListPurchaseDto>>>
{
    public PriceListPurchasePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
    }
    public string? Keyword { get; set; }
    public int? Status { get; set; }
}

public class PriceListPurchaseQuery : IQueryHandler<PriceListPurchaseQueryComboBox, IEnumerable<PriceListPurchaseListBoxDto>>,
                                      IQueryHandler<PriceListPurchaseQueryAll, IEnumerable<PriceListPurchaseDto>>,
                                      IQueryHandler<PriceListPurchaseQueryById, PriceListPurchaseDto>,
                                      IQueryHandler<PriceListPurchaseQueryByAccount, PagedResult<List<PriceListPurchaseDetailDto>>>,
                                      IQueryHandler<PriceListPurchasePagingQuery, PagedResult<List<PriceListPurchaseDto>>>
{
    private readonly IPriceListPurchaseRepository _repository;
    private readonly IPriceListPurchaseDetailRepository _priceListPurchaseDetailRepository;
    private readonly ICustomerRepository _customerRepository;
    public PriceListPurchaseQuery(IPriceListPurchaseRepository respository, ICustomerRepository customerRepository, IPriceListPurchaseDetailRepository priceListPurchaseDetailRepository)
    {
        _repository = respository;
        _customerRepository = customerRepository;
        _priceListPurchaseDetailRepository = priceListPurchaseDetailRepository;
    }

    public async Task<PagedResult<List<PriceListPurchaseDetailDto>>> Handle(PriceListPurchaseQueryByAccount request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PriceListPurchaseDetailDto>>();
        var customer = await _customerRepository.GetByAccountId(request.AccountId);
        Guid? priceListCrossId = null;
        if (customer is null || customer.PriceListPurchaseId is null)
        {
            var priceListCross = await _repository.GetDefault();
            if (priceListCross is null)
            {
                return response;
            }
            else
            {
                priceListCrossId = priceListCross.Id;
            }
        }
        else
        {
            priceListCrossId = customer.PriceListPurchaseId;
        }

        var fopRequest = FopExpressionBuilder<PriceListPurchase>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        filter.Add("priceListPurchaseId", priceListCrossId);
        var (datas, count) = await _priceListPurchaseDetailRepository.Filter(request.Keyword, filter, fopRequest);

        var result = datas.Select(x => new PriceListPurchaseDetailDto()
        {
            Id = x.Id,
            PriceListPurchaseId = x.PriceListPurchaseId,
            PriceListPurchase = x.PriceListPurchase,
            Note = x.Note,
            PurchaseGroupId = x.PurchaseGroupId,
            PurchaseGroupCode = x.PurchaseGroupCode,
            PurchaseGroupName = x.PurchaseGroupName,
            BuyFee = x.BuyFee,
            BuyFeeMin = x.BuyFeeMin,
            Currency = x.Currency,
            Status = x.Status,
            BuyFeeFix = x.BuyFeeFix,
            CreatedBy = x.CreatedBy,
            UpdatedBy = x.UpdatedBy,
            CreatedByName = x.CreatedByName,
            UpdatedByName = x.UpdatedByName,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate
        }).ToList();

        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PriceListPurchaseDto> Handle(PriceListPurchaseQueryById request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetById(request.PriceListPurchaseId);
        var result = new PriceListPurchaseDto()
        {
            Id = data.Id,
            Code = data.Code,
            Name = data.Name,
            Description = data.Description,
            Status = data.Status,
            DisplayOrder = data.DisplayOrder,
            CreatedDate = data.CreatedDate,
            UpdatedDate = data.UpdatedDate,
            CreatedBy = data.CreatedBy,
            UpdatedBy = data.UpdatedBy,
            CreatedByName = data.CreatedByName,
            UpdatedByName = data.UpdatedByName,
            Details = data.PriceListPurchaseDetail.OrderBy(x => x.DisplayOrder).Select(x => new PriceListPurchaseDetailDto()
            {
                Id = x.Id,
                PriceListPurchaseId = x.PriceListPurchaseId,
                PriceListPurchase = x.PriceListPurchase,
                Note = x.Note,
                PurchaseGroupId = x.PurchaseGroupId,
                PurchaseGroupCode = x.PurchaseGroupCode,
                PurchaseGroupName = x.PurchaseGroupName,
                BuyFee = x.BuyFee,
                BuyFeeMin = x.BuyFeeMin,
                Currency = x.Currency,
                Status = x.Status,
                BuyFeeFix = x.BuyFeeFix,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = x.CreatedByName,
                UpdatedByName = x.UpdatedByName,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            }).ToList()
        };
        return result;
    }

    public async Task<PagedResult<List<PriceListPurchaseDto>>> Handle(PriceListPurchasePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PriceListPurchaseDto>>();
        var fopRequest = FopExpressionBuilder<PriceListPurchase>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        var filterListBox = new Dictionary<string, object>();
        if (request.Status != null)
            filterListBox.Add("status", request.Status.ToString());
        var (datas, count) = await _repository.Filter(request.Keyword, filterListBox, fopRequest);
        var data = datas.Select(x => new PriceListPurchaseDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            Status = x.Status
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<PriceListPurchaseDto>> Handle(PriceListPurchaseQueryAll request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAll();
        var result = data.Select(x => new PriceListPurchaseDto()
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            DisplayOrder = x.DisplayOrder,
            Status = x.Status
        });
        return result;
    }

    public async Task<IEnumerable<PriceListPurchaseListBoxDto>> Handle(PriceListPurchaseQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.Status != null)
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (request.QueryParams.Date != null)
        {
            filter.Add("date", request.QueryParams.Date);
        }
        if (request.QueryParams.ProductId != null)
        {
            filter.Add("productId", request.QueryParams.ProductId);
        }
        if (request.QueryParams.Quantity != null)
        {
            filter.Add("quantity", request.QueryParams.Quantity);
        }
        if (request.QueryParams.ProductId != null && request.QueryParams.Quantity != null)
        {
            filter.Add("quantity_productId", request.QueryParams.ProductId);
        }
        var priceList = await _repository.GetListCbx(filter);
        var result = priceList.Select(x => new PriceListPurchaseListBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name,
            Details = x.PriceListPurchaseDetail?.Select(x => new PriceListPurchaseDetailDto()
            {
                Id = x.Id,
                BuyFeeMin = x.BuyFeeMin
            }).ToList()
        });
        return result;
    }
}
