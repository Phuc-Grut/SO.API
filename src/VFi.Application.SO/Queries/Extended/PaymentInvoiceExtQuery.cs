using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class TopPaymentByAccountIdQuery : IQuery<IEnumerable<TopPaymentDto>>
{
    public TopPaymentByAccountIdQuery(Guid accountId)
    {
        AccountId = accountId;
    }
    public Guid AccountId { get; set; }
}
public class PaymentInvoiceExtQueryHandler :
                                         IQueryHandler<TopPaymentByAccountIdQuery, IEnumerable<TopPaymentDto>>
{
    private readonly IPaymentInvoiceRepository _respository;
    private readonly ISOExtProcedures _repository;
    public PaymentInvoiceExtQueryHandler(IPaymentInvoiceRepository respository,
        ISOExtProcedures sOContextProcedures)
    {
        _respository = respository;
        _repository = sOContextProcedures;
    }


    public async Task<IEnumerable<TopPaymentDto>> Handle(TopPaymentByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var dbresult = await _repository.SP_GET_MY_TOP_PAYMENTAsync(request.AccountId);

        return dbresult.Select(x => new TopPaymentDto()
        {
            Id = x.id,
            Type = x.Type,
            Code = x.COde,
            OrderCode = x.OrderCode,
            OrderExpressCode = x.OrderExpressCode,
            Description = x.Description,
            Amount = x.Amount,
            Currency = x.Currency,
            ExchangeRate = x.ExchangeRate,
            Status = x.Status,
            PaymentMethodName = x.PaymentMethodName,
            PaymentCode = x.PaymentCode,
            PaymentStatus = x.PaymentStatus,
            PaymentDate = x.PaymentDate,
            CreatedDate = x.CreatedDate
        });
    }

}
