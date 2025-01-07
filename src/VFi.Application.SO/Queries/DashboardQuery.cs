using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class DasboardCountCustomer : IQuery<List<SP_DB_COUNT_CUSTOMERResult>>
{
    public DasboardCountCustomer()
    {
    }
}
public class DasboardOverView : IQuery<List<SP_DB_OVERVIEWResult>>
{
    public DasboardOverView(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}
public class DasboardSellingProduct : IQuery<List<SP_DB_SELLING_PRODUCTResult>>
{
    public DasboardSellingProduct(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}
public class DasboardOrderStatusCounter : IQuery<List<SP_ORDER_STATUS_COUNTERResult>>
{
    public DasboardOrderStatusCounter(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}

public class DasboardCustomerSale : IQuery<List<SP_DB_CUSTMER_SALEResult>>
{
    public DasboardCustomerSale(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}
public class DasboardStoreSale : IQuery<List<SP_DB_STORE_SALEResult>>
{
    public DasboardStoreSale(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}
public class DasboardSalesChannelSale : IQuery<List<SP_DB_SALESCHANNEL_SALEResult>>
{
    public DasboardSalesChannelSale(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}

public class DasboardSalesProcduct : IQuery<List<SP_DB_SALES_PRODUCT_BY_TIME>>
{
    public DasboardSalesProcduct(string? currency, int yaer, int type)
    {
        Currency = currency;
        Yaer = yaer;
        Type = type;
    }

    public string? Currency { get; set; }
    public int Yaer { get; set; }
    public int Type { get; set; }
}
public class DasboardContract : IQuery<List<SP_DB_CONTRACT>>
{
    public DasboardContract(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}
public class DasboardEmployeeExcellent : IQuery<List<SP_DB_EMPLOYEE_EXCELLENT>>
{
    public DasboardEmployeeExcellent(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}
public class DasboardPayment : IQuery<List<SP_DB_PAYMENT>>
{
    public DasboardPayment(DashboardParams queryParams)
    {
        QueryParams = queryParams;
    }
    public DashboardParams QueryParams { get; set; }
}


public class DashboardQueryHandler : IQueryHandler<DasboardCountCustomer, List<SP_DB_COUNT_CUSTOMERResult>>,
                                    IQueryHandler<DasboardOverView, List<SP_DB_OVERVIEWResult>>,
                                    IQueryHandler<DasboardSellingProduct, List<SP_DB_SELLING_PRODUCTResult>>,
                                    IQueryHandler<DasboardCustomerSale, List<SP_DB_CUSTMER_SALEResult>>,
                                    IQueryHandler<DasboardStoreSale, List<SP_DB_STORE_SALEResult>>,
                                    IQueryHandler<DasboardSalesChannelSale, List<SP_DB_SALESCHANNEL_SALEResult>>,
                                    IQueryHandler<DasboardSalesProcduct, List<SP_DB_SALES_PRODUCT_BY_TIME>>,
                                    IQueryHandler<DasboardContract, List<SP_DB_CONTRACT>>,
                                    IQueryHandler<DasboardEmployeeExcellent, List<SP_DB_EMPLOYEE_EXCELLENT>>,
                                    IQueryHandler<DasboardPayment, List<SP_DB_PAYMENT>>,
                                    IQueryHandler<DasboardOrderStatusCounter, List<SP_ORDER_STATUS_COUNTERResult>>
{
    private readonly ISOContextProcedures _soContextProcedures;
    private readonly ISOExtProcedures _sOExtProcedures;
    public DashboardQueryHandler(ISOContextProcedures soContextProcedures, ISOExtProcedures sOExtProcedures)
    {
        _soContextProcedures = soContextProcedures;
        _sOExtProcedures = sOExtProcedures;
    }
    public async Task<List<SP_DB_COUNT_CUSTOMERResult>> Handle(DasboardCountCustomer request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_COUNT_CUSTOMERAsync();
        var result = rs.Select(item =>
        {
            return new SP_DB_COUNT_CUSTOMERResult()
            {
                Id = item.Id,
                Key = item.Key,
                Title = item.Title,
                Value = item.Value
            };
        }
        ).ToList();
        return result;
    }
    public async Task<List<SP_DB_OVERVIEWResult>> Handle(DasboardOverView request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_OVERVIEWAsync(
                        request.QueryParams.Currency,
                        request.QueryParams.StartDate,
                        request.QueryParams.EndDate
                    );
        var result = rs.Select(item =>
        {
            return new SP_DB_OVERVIEWResult()
            {
                Id = item.Id,
                Key = item.Key,
                Title = item.Title,
                Value = item.Value ?? 0,
                Value2 = item.Value2 ?? 0
            };
        }
        ).ToList();
        return result;
    }
    public async Task<List<SP_DB_SELLING_PRODUCTResult>> Handle(DasboardSellingProduct request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_SELLING_PRODUCTAsync(
                        request.QueryParams.Currency,
                        request.QueryParams.StartDate,
                        request.QueryParams.EndDate
                    );
        var result = rs.Select(item =>
        {
            return new SP_DB_SELLING_PRODUCTResult()
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                CountOrder = item.CountOrder,
                Amount = item.Amount
            };
        }
        ).ToList();
        return result;
    }
    public async Task<List<SP_DB_CUSTMER_SALEResult>> Handle(DasboardCustomerSale request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_CUSTMER_SALEAsync(
                        request.QueryParams.Currency,
                        request.QueryParams.StartDate,
                        request.QueryParams.EndDate
                    );
        var result = rs.Select(item =>
        {
            return new SP_DB_CUSTMER_SALEResult()
            {
                Id = item.Id,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                Amount = item.Amount
            };
        }
        ).ToList();
        return result;
    }
    public async Task<List<SP_DB_STORE_SALEResult>> Handle(DasboardStoreSale request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_STORE_SALEAsync(
                        request.QueryParams.Currency,
                        request.QueryParams.StartDate,
                        request.QueryParams.EndDate
                    );
        var result = rs.Select(item =>
        {
            return new SP_DB_STORE_SALEResult()
            {
                Id = item.Id,
                StoreCode = item.StoreCode,
                StoreName = item.StoreName,
                Amount = item.Amount
            };
        }
        ).ToList();
        return result;
    }
    public async Task<List<SP_DB_SALESCHANNEL_SALEResult>> Handle(DasboardSalesChannelSale request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_SALESCHANNEL_SALEAsync(
                        request.QueryParams.Currency,
                        request.QueryParams.StartDate,
                        request.QueryParams.EndDate
                    );
        var result = rs.Select(item =>
        {
            return new SP_DB_SALESCHANNEL_SALEResult()
            {
                Id = item.Id,
                ChannelId = item.ChannelId,
                ChannelName = item.ChannelName,
                NumberOfOrder = item.NumberOfOrder
            };
        }
        ).ToList();
        return result;
    }

    public async Task<List<SP_DB_SALES_PRODUCT_BY_TIME>> Handle(DasboardSalesProcduct request, CancellationToken cancellationToken)
    {

        DateTime date = new DateTime(request.Yaer, 1, 1);

        var rs = await _soContextProcedures.SP_DB_SALES_PRODUCT_BY_TIME(
                       request.Currency,
                       date,
                       request.Type
                   );

        var result = rs.Select(item =>
        {
            return new SP_DB_SALES_PRODUCT_BY_TIME()
            {
                Id = item.Id,
                Name = item.Name,
                Amount = item.Amount
            };
        }).ToList();

        return result;
    }

    public async Task<List<SP_DB_CONTRACT>> Handle(DasboardContract request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_CONTRACT(
                         request.QueryParams.Currency,
                         request.QueryParams.StartDate,
                         request.QueryParams.EndDate
                     );
        var result = rs.Select(item =>
        {
            return new SP_DB_CONTRACT()
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity
            };
        }
        ).ToList();
        return result;

    }

    public async Task<List<SP_DB_EMPLOYEE_EXCELLENT>> Handle(DasboardEmployeeExcellent request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_EMPLOYEE_EXCELLENT(
                        request.QueryParams.Currency,
                        request.QueryParams.StartDate,
                        request.QueryParams.EndDate
                    );
        var result = rs.Select(item =>
        {
            return new SP_DB_EMPLOYEE_EXCELLENT()
            {
                Id = item.Id,
                Name = item.Name,
                Amount = item.Amount
            };
        }
        ).ToList();
        return result;
    }


    public async Task<List<SP_DB_PAYMENT>> Handle(DasboardPayment request, CancellationToken cancellationToken)
    {
        var rs = await _soContextProcedures.SP_DB_PAYMENT(
                        request.QueryParams.Currency,
                        request.QueryParams.StartDate,
                        request.QueryParams.EndDate
                    );

        var result = rs.Select(item =>
        {
            return new SP_DB_PAYMENT()
            {
                Id = item.Id,
                Name = item.Name,
                Amount = item.Amount
            };
        }
        ).ToList();

        return result;
    }

    public async Task<List<SP_ORDER_STATUS_COUNTERResult>> Handle(DasboardOrderStatusCounter request, CancellationToken cancellationToken)
    {
        var rs = await _sOExtProcedures.SP_ORDER_STATUS_COUNTERAsync(
                         request.QueryParams.StartDate,
                         request.QueryParams.EndDate);
        return rs.ToList();
    }
}
