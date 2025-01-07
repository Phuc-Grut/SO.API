using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PriceListPurchaseDetailQueryAll : IQuery<IEnumerable<PriceListPurchaseDetailDto>>
{
    public PriceListPurchaseDetailQueryAll()
    {
    }
}

public class PriceListPurchaseDetailQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PriceListPurchaseDetailQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class PriceListPurchaseDetailQueryCheckCode : IQuery<bool>
{
    public PriceListPurchaseDetailQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PriceListPurchaseDetailQueryById : IQuery<PriceListPurchaseDetailDto>
{
    public PriceListPurchaseDetailQueryById()
    {
    }

    public PriceListPurchaseDetailQueryById(Guid PriceListPurchaseDetailId)
    {
        PriceListPurchaseDetailId = PriceListPurchaseDetailId;
    }

    public Guid PriceListPurchaseDetailId { get; set; }
}
public class PriceListPurchaseDetailPagingQuery : FopQuery, IQuery<PagedResult<List<PriceListPurchaseDetailDto>>>
{
    public PriceListPurchaseDetailPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class PriceListPurchaseDetailQueryHandler :
                                         IQueryHandler<PriceListPurchaseDetailQueryAll, IEnumerable<PriceListPurchaseDetailDto>>,
                                         IQueryHandler<PriceListPurchaseDetailQueryById, PriceListPurchaseDetailDto>
// IQueryHandler<PriceListPurchaseDetailPagingQuery, PagedResult<List<PriceListPurchaseDetailDto>>>
{
    private readonly IPriceListPurchaseDetailRepository _repository;
    public PriceListPurchaseDetailQueryHandler(IPriceListPurchaseDetailRepository repository)
    {
        _repository = repository;
    }


    public async Task<PriceListPurchaseDetailDto> Handle(PriceListPurchaseDetailQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PriceListPurchaseDetailId);
        var result = new PriceListPurchaseDetailDto()
        {
            Id = item.Id,
            PriceListPurchaseId = item.PriceListPurchaseId,
            PriceListPurchase = item.PriceListPurchase,
            Note = item.Note,
            PurchaseGroupId = item.PurchaseGroupId,
            PurchaseGroupName = item.PurchaseGroupName,
            PurchaseGroupCode = item.PurchaseGroupCode,
            BuyFeeFix = item.BuyFeeFix,
            BuyFee = item.BuyFee,
            BuyFeeMin = item.BuyFeeMin,
            Currency = item.Currency,
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

    //public async Task<PagedResult<List<PriceListPurchaseDetailDto>>> Handle(PriceListPurchaseDetailPagingQuery request, CancellationToken cancellationToken)
    //{
    //    var response = new PagedResult<List<PriceListPurchaseDetailDto>>();
    //    var filter = new Dictionary<string, object>();
    //    var fopRequest = FopExpressionBuilder<PriceListPurchaseDetail>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

    //    var (items, count) = await _repository.Filter(request.Keyword, fopRequest);
    //    var data = items.Select(item => new PriceListPurchaseDetailDto()
    //    {
    //        Id = item.Id,
    //        PriceListPurchaseId = item.PriceListPurchaseId,
    //        PriceListPurchase = item.PriceListPurchase,
    //        Code = item.Code,
    //        Name = item.Name,
    //        Description = item.Description,
    //        Note = item.Note,
    //        PurchaseGroupId = item.PurchaseGroupId,
    //        PurchaseGroup = item.PurchaseGroup,
    //        BuyFee = item.BuyFee,
    //        BuyFeeMin = item.BuyFeeMin,
    //        Currency = item.Currency,
    //        Status = item.Status,
    //        CreatedBy = item.CreatedBy,
    //        CreatedDate = item.CreatedDate,
    //        UpdatedBy = item.UpdatedBy,
    //        UpdatedDate = item.UpdatedDate,
    //        CreatedByName = item.CreatedByName,
    //        UpdatedByName = item.UpdatedByName
    //    }).ToList();
    //    response.Items = data;
    //    response.TotalCount = count;
    //    response.PageNumber = request.PageNumber;
    //    response.PageSize = request.PageSize;
    //    return response;
    //}

    public async Task<IEnumerable<PriceListPurchaseDetailDto>> Handle(PriceListPurchaseDetailQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PriceListPurchaseDetailDto()
        {
            Id = item.Id,
            PriceListPurchaseId = item.PriceListPurchaseId,
            PriceListPurchase = item.PriceListPurchase,
            Note = item.Note,
            PurchaseGroupId = item.PurchaseGroupId,
            PurchaseGroupCode = item.PurchaseGroupCode,
            PurchaseGroupName = item.PurchaseGroupName,
            BuyFeeFix = item.BuyFeeFix,
            BuyFee = item.BuyFee,
            BuyFeeMin = item.BuyFeeMin,
            Currency = item.Currency,
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


}
