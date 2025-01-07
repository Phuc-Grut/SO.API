using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;


public class PaymentInvoiceQueryComboBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public PaymentInvoiceQueryComboBox(PaymentInvoiceQueryParams queryParams)
    {
        QueryParams = queryParams;
    }
    public PaymentInvoiceQueryParams QueryParams { get; set; }
}
public class PaymentInvoiceQueryCheckCode : IQuery<bool>
{

    public PaymentInvoiceQueryCheckCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class PaymentInvoiceQueryById : IQuery<PaymentInvoiceDto>
{
    public PaymentInvoiceQueryById()
    {
    }

    public PaymentInvoiceQueryById(Guid paymentInvoiceId)
    {
        PaymentInvoiceId = paymentInvoiceId;
    }

    public Guid PaymentInvoiceId { get; set; }
}
public class PaymentInvoicePagingQuery : FopQuery, IQuery<PagedResultPaymentInvoice<List<PaymentInvoiceDto>>>
{
    public PaymentInvoicePagingQuery(string? keyword, int? status, string filter, string order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Status = status;
        Keyword = keyword;
    }
}

public class PaymentInvoiceQueryHandler : IQueryHandler<PaymentInvoiceQueryComboBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<PaymentInvoiceQueryById, PaymentInvoiceDto>,
                                         IQueryHandler<PaymentInvoicePagingQuery, PagedResultPaymentInvoice<List<PaymentInvoiceDto>>>
{
    private readonly IPaymentInvoiceRepository _respository;
    public PaymentInvoiceQueryHandler(IPaymentInvoiceRepository respository)
    {
        _respository = respository;
    }

    public async Task<PaymentInvoiceDto> Handle(PaymentInvoiceQueryById request, CancellationToken cancellationToken)
    {
        var item = await _respository.GetById(request.PaymentInvoiceId);
        var result = new PaymentInvoiceDto()
        {
            Id = item.Id,
            Type = item.Type,
            Code = item.Code,
            OrderId = item.OrderId,
            OrderCode = item.OrderCode,
            SaleDiscountId = item.SaleDiscountId,
            ReturnOrderId = item.ReturnOrderId,
            RerferenceId = (item.Type == "11" || item.Type == "00") ? item.OrderId : item.Type == "01" ? item.SaleDiscountId : item.Type == "02" ? item.ReturnOrderId : null,
            RerferenceCode = (item.Type == "11" || item.Type == "00") ? item.OrderCode : item.Type == "01" ? item.SaleDiscount?.Code : item.Type == "02" ? item.ReturnOrder?.Code : null,
            Description = item.Description,
            Amount = item.Amount,
            Currency = item.Currency,
            CurrencyName = item.CurrencyName,
            Calculation = item.Calculation,
            ExchangeRate = item.ExchangeRate,
            PaymentDate = item.PaymentDate,
            PaymentMethodName = item.PaymentMethodName,
            PaymentMethodCode = item.PaymentMethodCode,
            PaymentMethodId = item.PaymentMethodId,
            Bank = item.BankName + " - " + item.BankNumber,
            BankName = item.BankName,
            BankAccount = item.BankAccount,
            BankNumber = item.BankNumber,
            PaymentCode = item.PaymentCode,
            PaymentNote = item.PaymentNote,
            Note = item.Note,
            Status = item.Status,
            Locked = item.Locked,
            PaymentStatus = item.PaymentStatus,
            AccountId = item.AccountId,
            AccountName = item.AccountName,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            File = JsonConvert.DeserializeObject<List<FileDto>>(string.IsNullOrEmpty(item.File) ? "" : item.File)
        };
        return result;
    }

    public async Task<PagedResultPaymentInvoice<List<PaymentInvoiceDto>>> Handle(PaymentInvoicePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResultPaymentInvoice<List<PaymentInvoiceDto>>();

        var fopRequest = FopExpressionBuilder<PaymentInvoice>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (items, count, output) = await _respository.Filter(request.Keyword, request.Status, fopRequest);
        var data = items.Select(item => new PaymentInvoiceDto()
        {
            Id = item.Id,
            Type = item.Type,
            Code = item.Code,
            OrderId = item.OrderId,
            OrderCode = item.OrderCode,
            SaleDiscountId = item.SaleDiscountId,
            ReturnOrderId = item.ReturnOrderId,
            RerferenceId = (item.Type == "11" || item.Type == "00") ? item.OrderId : item.Type == "01" ? item.SaleDiscountId : item.Type == "02" ? item.ReturnOrderId : null,
            RerferenceCode = (item.Type == "11" || item.Type == "00") ? item.OrderCode : item.Type == "01" ? item.SaleDiscount?.Code : item.Type == "02" ? item.ReturnOrder?.Code : null,
            Description = item.Description,
            Amount = item.Amount,
            Currency = item.Currency,
            CurrencyName = item.CurrencyName,
            Calculation = item.Calculation,
            ExchangeRate = item.ExchangeRate,
            PaymentDate = item.PaymentDate,
            PaymentMethodName = item.PaymentMethodName,
            PaymentMethodId = item.PaymentMethodId,
            BankName = item.BankName,
            BankAccount = item.BankAccount,
            BankNumber = item.BankNumber,
            PaymentCode = item.PaymentCode,
            PaymentNote = item.PaymentNote,
            Note = item.Note,
            Status = item.Status,
            Locked = item.Locked,
            PaymentStatus = item.PaymentStatus,
            AccountId = item.AccountId,
            AccountName = item.AccountName,
            CustomerId = item.CustomerId,
            CustomerName = item.CustomerName,
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
        response.TongThu = output.Where(x => x.Status == 1 && (x.Type == "11" || x.Type == "13")).Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));
        response.TongChi = output.Where(x => x.Status == 1 && (x.Type == "00" || x.Type == "01" || x.Type == "02" || x.Type == "08")).Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));

        response.TongGiamGia = output.Where(x => x.Status == 1 && x.Type == "01").Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));
        response.TongTraHang = output.Where(x => x.Status == 1 && x.Type == "02").Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));
        response.TongHuyDon = output.Where(x => x.Status == 1 && x.Type == "03").Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));
        response.TongTTDon = output.Where(x => x.Status == 1 && x.Type == "11").Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));
        response.TongThuHuyDon = output.Where(x => x.Status == 1 && (x.Type == "13")).Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));
        response.TongNapTien = output.Where(x => x.Status == 1 && x.Type == "18").Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));
        response.TongRutTien = output.Where(x => x.Status == 1 && x.Type == "08").Sum(x => x.Amount * (x.Calculation == "/" ? (1 / (x.ExchangeRate ?? 1)) : (x.ExchangeRate ?? 1)));

        return response;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(PaymentInvoiceQueryComboBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.Status != null)
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (request.QueryParams.OrderId != null)
        {
            filter.Add("orderId", request.QueryParams.OrderId);
        }
        var PaymentInvoices = await _respository.Filter(filter, null);
        var result = PaymentInvoices.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.BankAccount
        });
        return result;
    }
}
