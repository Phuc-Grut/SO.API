using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class PaymentMethodQueryAll : IQuery<IEnumerable<PaymentMethodDto>>
{
    public PaymentMethodQueryAll()
    {
    }
}

public class PaymentMethodQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PaymentMethodQueryComboBox(int? status)
    {
        Status = status;
    }
    public int? Status { get; set; }
}
public class PaymentMethodQueryCheckCode : IQuery<bool>
{

    public PaymentMethodQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PaymentMethodQueryById : IQuery<PaymentMethodDto>
{
    public PaymentMethodQueryById()
    {
    }

    public PaymentMethodQueryById(Guid paymentMethodId)
    {
        PaymentMethodId = paymentMethodId;
    }

    public Guid PaymentMethodId { get; set; }
}
public class PaymentMethodPagingQuery : FopQuery, IQuery<PagedResult<List<PaymentMethodDto>>>
{
    public PaymentMethodPagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
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


public class PaymentMethodQueryHandler : IQueryHandler<PaymentMethodQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<PaymentMethodQueryAll, IEnumerable<PaymentMethodDto>>,
                                         IQueryHandler<PaymentMethodQueryCheckCode, bool>,
                                         IQueryHandler<PaymentMethodQueryById, PaymentMethodDto>,
                                         IQueryHandler<PaymentMethodPagingQuery, PagedResult<List<PaymentMethodDto>>>
{
    private readonly IPaymentMethodRepository _repository;
    public PaymentMethodQueryHandler(IPaymentMethodRepository paymentMethodRespository)
    {
        _repository = paymentMethodRespository;
    }
    public async Task<bool> Handle(PaymentMethodQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _repository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<PaymentMethodDto> Handle(PaymentMethodQueryById request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetById(request.PaymentMethodId);
        var result = new PaymentMethodDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy
        };
        return result;
    }

    public async Task<PagedResult<List<PaymentMethodDto>>> Handle(PaymentMethodPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<PaymentMethodDto>>();

        var fopRequest = FopExpressionBuilder<PaymentMethod>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _repository.Filter(request.Keyword, request.Status, fopRequest);
        var data = datas.Select(item => new PaymentMethodDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedByName = item.UpdatedByName
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<IEnumerable<PaymentMethodDto>> Handle(PaymentMethodQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAll();
        var result = items.Select(item => new PaymentMethodDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            UpdatedDate = item.UpdatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedBy = item.UpdatedBy
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(PaymentMethodQueryComboBox request, CancellationToken cancellationToken)
    {

        var PaymentMethods = await _repository.GetListCbx(request.Status);
        var result = PaymentMethods.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
