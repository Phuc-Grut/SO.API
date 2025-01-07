using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Domain.SO.Models.Extented;

namespace VFi.Infra.SO.Context.Extented;

public partial class SOExtProcedures : ISOExtProcedures
{
    //private readonly SqlCoreContext _context;

    //public SOExtProcedures(IServiceProvider serviceProvider)
    //{
    //    var scope = serviceProvider.CreateScope();
    //    _context = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
    //}
    private readonly SqlCoreContext _context;

    public SOExtProcedures(SqlCoreContext context)
    {
        _context = context;
    }

    public async Task<List<SP_ACCOUNT_REVENUE_LOADResult>> SP_ACCOUNT_REVENUE_LOADAsync(Guid? accountId, int? month, int? year, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = System.Data.ParameterDirection.Output,
            SqlDbType = System.Data.SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "month",
                Value = month ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "year",
                Value = year ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_ACCOUNT_REVENUE_LOADResult>("EXEC @returnValue = [so].[SP_ACCOUNT_REVENUE_LOAD] @accountId, @month, @year", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_ADD_HOLD_BID_WALLETResult>> SP_ADD_HOLD_BID_WALLETAsync(Guid? accountId, string walletCode, string code, decimal? amount, string note, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "amount",
                Precision = 19,
                Scale = 4,
                Value = amount ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 510,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_ADD_HOLD_BID_WALLETResult>("EXEC @returnValue = [so].[SP_ADD_HOLD_BID_WALLET] @accountId, @walletCode, @code, @amount, @note, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_DEACTIVE_BID_WALLETResult>> SP_DEACTIVE_BID_WALLETAsync(Guid? accountId, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DEACTIVE_BID_WALLETResult>("EXEC @returnValue = [so].[SP_DEACTIVE_BID_WALLET] @accountId, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_HOLD_BID_WALLETResult>> SP_HOLD_BID_WALLETAsync(Guid? accountId, string walletCode, string code, decimal? amount, int? bidQuantity, string note, string rawData, string document, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "amount",
                Precision = 19,
                Scale = 4,
                Value = amount ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "bidQuantity",
                Value = bidQuantity ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 255,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "rawData",
                Size = 255,
                Value = rawData ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "document",
                Size = 255,
                Value = document ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_HOLD_BID_WALLETResult>("EXEC @returnValue = [so].[SP_HOLD_BID_WALLET] @accountId, @walletCode, @code, @amount, @bidQuantity, @note, @rawData, @document, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_HOLD_WALLETResult>> SP_HOLD_WALLETAsync(Guid? accountId, string walletCode, string code, decimal? amount, string note, string rawData, Guid? RefId, DateTime? RefDate, string RefType, string document, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "amount",
                Precision = 19,
                Scale = 4,
                Value = amount ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 510,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "rawData",
                Size = 510,
                Value = rawData ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "RefId",
                Value = RefId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "RefDate",
                Value = RefDate ?? Convert.DBNull,
                SqlDbType = SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "RefType",
                Size = 50,
                Value = RefType ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "document",
                Size = 510,
                Value = document ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_HOLD_WALLETResult>("EXEC @returnValue = [so].[SP_HOLD_WALLET] @accountId, @walletCode, @code, @amount, @note, @rawData, @RefId, @RefDate, @RefType, @document, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_REFUND_HOLD_WALLETResult>> SP_REFUND_HOLD_WALLETAsync(Guid? accountId, string walletCode, string code, Guid? refId, string note, string rawData, string document, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "refId",
                Value = refId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 510,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "rawData",
                Size = 510,
                Value = rawData ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "document",
                Size = 510,
                Value = document ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_REFUND_HOLD_WALLETResult>("EXEC @returnValue = [so].[SP_REFUND_HOLD_WALLET] @accountId, @walletCode, @code, @refId, @note, @rawData, @document, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }


    //fix
    public async Task<List<SP_WITHDRAW_WALLETResult>> SP_WITHDRAW_WALLETAsync(Guid? accountId, string walletCode, string code, decimal? amount, string method, string note, string rawData, string document, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "amount",
                Precision = 19,
                Scale = 4,
                Value = amount ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "method",
                Size = 50,
                Value = method ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 510,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "rawData",
                Size = 510,
                Value = rawData ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "document",
                Size = 510,
                Value = document ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_WITHDRAW_WALLETResult>("EXEC @returnValue = [so].[SP_WITHDRAW_WALLET] @accountId, @walletCode, @code, @amount, @method, @note, @rawData, @document, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_WITHDRAW_WALLET_APPROVEResult>> SP_WITHDRAW_WALLET_APPROVEAsync(Guid? id, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_WITHDRAW_WALLET_APPROVEResult>("EXEC @returnValue = [so].[SP_WITHDRAW_WALLET_APPROVE] @id", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_REFUND_HOLD_BID_WALLETResult>> SP_REFUND_HOLD_BID_WALLETAsync(Guid? accountId, Guid? refId, string code, string note, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "refId",
                Value = refId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 510,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_REFUND_HOLD_BID_WALLETResult>("EXEC @returnValue = [so].[SP_REFUND_HOLD_BID_WALLET] @accountId, @refId, @code, @note, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_GET_PRICE_PUCHASE_BY_ACCOUNT_IDResult>> SP_GET_PRICE_PUCHASE_BY_ACCOUNT_IDAsync(Guid? accountId, string? purchaseGroupCode, decimal? price, NetDevPack.Domain.OutputParameter<int>? returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "purchaseGroupCode",
                Size = 50,
                Value = purchaseGroupCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "price",
                Precision = 19,
                Scale = 4,
                Value = price ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_PRICE_PUCHASE_BY_ACCOUNT_IDResult>("EXEC @returnValue = [so].[SP_GET_PRICE_PUCHASE_BY_ACCOUNT_ID] @accountId, @purchaseGroupCode, @price", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_CREATE_CROSS_ORDERResult>> SP_CREATE_CROSS_ORDERAsync(
        Guid? id,
        string code,
        Guid? customerId,
        string orderType,
        string purchaseGroup,
        string storeCode,
        string channelCode,
        string currencyCode,
        decimal? exchangeRate,
        string paymentTermCode,
        string paymentMethodCode,
        string shippingMethodCode,
        int? buyFee,
        decimal? domesticShiping,
        string routerShipping,
        string deliveryCountry,
        string deliveryProvince,
        string deliveryDistrict,
        string deliveryWard,
        string deliveryAddress,
        string deliveryName,
        string deliveryPhone,
        string deliveryNote,
        DataTable products,
        DataTable serviceAdd,
        string image,
        string images,
        string description,
        string note,
        decimal? totalPay,
        bool? payNow,
        Guid? userId,
        string userName,
        NetDevPack.Domain.OutputParameter<int> returnValue = null,
        CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "customerId",
                Value = customerId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "initOrderType",
                Size = 50,
                Value = orderType ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "purchaseGroup",
                Size = 50,
                Value = purchaseGroup ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "storeCode",
                Size = 50,
                Value = storeCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "channelCode",
                Size = 50,
                Value = channelCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "currencyCode",
                Size = 50,
                Value = currencyCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "exchangeRate",
                Precision = 18,
                Value = exchangeRate ?? Convert.DBNull,
                SqlDbType = SqlDbType.Decimal,
            },
            new SqlParameter
            {
                ParameterName = "paymentTermCode",
                Size = 50,
                Value = paymentTermCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "paymentMethodCode",
                Size = 50,
                Value = paymentMethodCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "shippingMethodCode",
                Size = 50,
                Value = shippingMethodCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "buyFee",
                Value = buyFee ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "domesticShiping",
                Precision = 19,
                Scale = 4,
                Value = domesticShiping ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "routerShipping",
                Size = 510,
                Value = routerShipping ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryCountry",
                Size = 510,
                Value = deliveryCountry ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryProvince",
                Size = 510,
                Value = deliveryProvince ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryDistrict",
                Size = 510,
                Value = deliveryDistrict ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryWard",
                Size = 510,
                Value = deliveryWard ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryAddress",
                Size = 510,
                Value = deliveryAddress ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryName",
                Size = 510,
                Value = deliveryName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryPhone",
                Size = 510,
                Value = deliveryPhone ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryNote",
                Size = 510,
                Value = deliveryNote ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "products",
                Value = products ?? Convert.DBNull,
                SqlDbType = SqlDbType.Structured,
                TypeName = "[so].[ProductCrossType]",
            },
            new SqlParameter
            {
                ParameterName = "serviceAdd",
                Value = serviceAdd ?? Convert.DBNull,
                SqlDbType = SqlDbType.Structured,
                TypeName = "[so].[OrderServiceAddType]",
            },
            new SqlParameter
            {
                ParameterName = "image",
                Size = 510,
                Value = image ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "images",
                Size = 8000,
                Value = images ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "description",
                Size = 200,
                Value = description ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 1000,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "totalPay",
                Precision = 19,
                Scale = 4,
                Value = totalPay ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "payNow",
                Value = payNow.GetValueOrDefault(false) ? 1: 0,
                SqlDbType = SqlDbType.Bit,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_CREATE_CROSS_ORDERResult>("EXEC @returnValue = " +
            "[so].[SP_CREATE_CROSS_ORDER] " +
            "@id, " +
            "@code, " +
            "@customerId, " +
            "@initOrderType, " +
            "@purchaseGroup, " +
            "@storeCode, " +
            "@channelCode, " +
            "@currencyCode, " +
            "@exchangeRate, " +
            "@paymentTermCode, " +
            "@paymentMethodCode, " +
            "@shippingMethodCode, " +
            "@buyFee, " +
            "@domesticShiping, " +
            "@routerShipping, " +
            "@deliveryCountry, @deliveryProvince, @deliveryDistrict, @deliveryWard, @deliveryAddress, @deliveryName, @deliveryPhone, @deliveryNote, " +
            "@products, " +
            "@serviceAdd, " +
            "@image, " +
            "@images, " +
            "@description, " +
            "@note, " +
            "@totalPay, " +
            "@payNow, " +
            "@userId, " +
            "@userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_DEPOSIT_FROM_BANKResult>> SP_DEPOSIT_FROM_BANKAsync(Guid? accountId, string walletCode, decimal? amount, string paymentCode, string paymentNote, DateTime? paymentDate, string bankName, string bankAccount, string bankNumber, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "amount",
                Precision = 19,
                Scale = 4,
                Value = amount ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "paymentCode",
                Size = 50,
                Value = paymentCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "paymentNote",
                Size = 510,
                Value = paymentNote ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "paymentDate",
                Value = paymentDate ?? Convert.DBNull,
                SqlDbType = SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "bankName",
                Size = 500,
                Value = bankName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "bankAccount",
                Size = 500,
                Value = bankAccount ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "bankNumber",
                Size = 500,
                Value = bankNumber ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DEPOSIT_FROM_BANKResult>("EXEC @returnValue = [so].[SP_DEPOSIT_FROM_BANK] @accountId, @walletCode, @amount, @paymentCode, @paymentNote, @paymentDate, @bankName, @bankAccount, @bankNumber, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_CREATE_INVOICE_PAY_ORDERResult>> SP_CREATE_INVOICE_PAY_ORDERAsync(Guid? id, decimal? totalPay, Guid? accountId, Guid? createdBy, string createdByName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "totalPay",
                Precision = 19,
                Scale = 4,
                Value = totalPay ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdBy",
                Value = createdBy ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdByName",
                Size = 510,
                Value = createdByName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };

        var _ = await _context.SqlQueryAsync<SP_CREATE_INVOICE_PAY_ORDERResult>("EXEC @returnValue = [so].[SP_CREATE_INVOICE_PAY_ORDER] " +
            "@id, " +
            "@totalPay, " +
            "@accountId, " +
            "@createdBy, " +
            "@createdByName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_CREATE_EXPRESS_ORDERResult>> SP_CREATE_EXPRESS_ORDERAsync(Guid? id, string code, Guid? customerId, string storeCode, string currencyCode, string shippingMethodCode, string routerShipping, string trackingCode, string trackingCarrier, int? weight, int? width, int? height, int? length, string deliveryCountry, string deliveryProvince, string deliveryDistrict, string deliveryWard, string deliveryAddress, string deliveryName, string deliveryPhone, string deliveryNote, DataTable products, DataTable serviceAdd, string image, string images, string description, string note, Guid? createdBy, string createdByName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "customerId",
                Value = customerId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "storeCode",
                Size = 50,
                Value = storeCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "currencyCode",
                Size = 50,
                Value = currencyCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "shippingMethodCode",
                Size = 50,
                Value = shippingMethodCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "routerShipping",
                Size = 510,
                Value = routerShipping ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "trackingCode",
                Size = 50,
                Value = trackingCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "trackingCarrier",
                Size = 510,
                Value = trackingCarrier ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "weight",
                Value = weight ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "width",
                Value = width ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "height",
                Value = height ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "length",
                Value = length ?? Convert.DBNull,
                SqlDbType = SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "deliveryCountry",
                Size = 510,
                Value = deliveryCountry ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryProvince",
                Size = 510,
                Value = deliveryProvince ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryDistrict",
                Size = 510,
                Value = deliveryDistrict ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryWard",
                Size = 510,
                Value = deliveryWard ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryAddress",
                Size = 510,
                Value = deliveryAddress ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryName",
                Size = 510,
                Value = deliveryName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryPhone",
                Size = 510,
                Value = deliveryPhone ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryNote",
                Size = 510,
                Value = deliveryNote ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "products",
                Value = products ?? Convert.DBNull,
                SqlDbType = SqlDbType.Structured,
                TypeName = "[so].[ProductExpressType]",
            },
            new SqlParameter
            {
                ParameterName = "serviceAdd",
                Value = serviceAdd ?? Convert.DBNull,
                SqlDbType = SqlDbType.Structured,
                TypeName = "[so].[OrderServiceAddType]",
            },
            new SqlParameter
            {
                ParameterName = "image",
                Size = 510,
                Value = image ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "images",
                Size = 8000,
                Value = images ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "description",
                Size = 200,
                Value = description ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 1000,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "createdBy",
                Value = createdBy ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdByName",
                Size = 510,
                Value = createdByName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_CREATE_EXPRESS_ORDERResult>("EXEC @returnValue = [so].[SP_CREATE_EXPRESS_ORDER] @id, @code, @customerId, @storeCode, @currencyCode, @shippingMethodCode, @routerShipping, @trackingCode, @trackingCarrier, @weight, @width, @height, @length, @deliveryCountry, @deliveryProvince, @deliveryDistrict, @deliveryWard, @deliveryAddress, @deliveryName, @deliveryPhone, @deliveryNote, @products, @serviceAdd, @image, @images, @description, @note, @createdBy, @createdByName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_GET_MY_INFOResult>> SP_GET_MY_INFOAsync(Guid? accountId, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_MY_INFOResult>("EXEC @returnValue = [so].[SP_GET_MY_INFO] @accountId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_GET_ORDER_COUNTERResult>> SP_GET_ORDER_COUNTERAsync(Guid? customerId, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "customerId",
                Value = customerId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_ORDER_COUNTERResult>("EXEC @returnValue = [so].[SP_GET_ORDER_COUNTER] @customerId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_GET_ORDEREXPRESS_COUNTERResult>> SP_GET_ORDEREXPRESS_COUNTERAsync(Guid? customerId, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "customerId",
                Value = customerId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_ORDEREXPRESS_COUNTERResult>("EXEC @returnValue = [so].[SP_GET_ORDEREXPRESS_COUNTER] @customerId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_ORDER_UPDATE_ADDRESSResult>> SP_ORDER_UPDATE_ADDRESSAsync(Guid? id, string deliveryCountry, string deliveryProvince, string deliveryDistrict, string deliveryWard, string deliveryAddress, string deliveryName, string deliveryPhone, string deliveryNote, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "deliveryCountry",
                Size = 510,
                Value = deliveryCountry ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryProvince",
                Size = 510,
                Value = deliveryProvince ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryDistrict",
                Size = 510,
                Value = deliveryDistrict ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryWard",
                Size = 510,
                Value = deliveryWard ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryAddress",
                Size = 510,
                Value = deliveryAddress ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryName",
                Size = 510,
                Value = deliveryName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryPhone",
                Size = 510,
                Value = deliveryPhone ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "deliveryNote",
                Size = 510,
                Value = deliveryNote ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_ORDER_UPDATE_ADDRESSResult>("EXEC @returnValue = [so].[SP_ORDER_UPDATE_ADDRESS] @id, @deliveryCountry, @deliveryProvince, @deliveryDistrict, @deliveryWard, @deliveryAddress, @deliveryName, @deliveryPhone, @deliveryNote, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }


    public async Task<List<SP_GET_MY_TOP_ORDERResult>> SP_GET_MY_TOP_ORDERAsync(Guid? customerId, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "customerId",
                Value = customerId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_MY_TOP_ORDERResult>("EXEC @returnValue = [so].[SP_GET_MY_TOP_ORDER] @customerId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_GET_MY_TOP_PAYMENTResult>> SP_GET_MY_TOP_PAYMENTAsync(Guid? accountId, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_MY_TOP_PAYMENTResult>("EXEC @returnValue = [so].[SP_GET_MY_TOP_PAYMENT] @accountId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_CANCEL_ORDER_AUCTIONResult>> SP_CANCEL_ORDER_AUCTIONAsync(Guid? id, Guid? createdBy, string createdByName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdBy",
                Value = createdBy ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdByName",
                Size = 510,
                Value = createdByName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_CANCEL_ORDER_AUCTIONResult>("EXEC @returnValue = [so].[SP_CANCEL_ORDER_AUCTION] @id, @createdBy, @createdByName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<int> SP_CANCEL_ORDER_NO_FEEAsync(Guid? id, Guid? createdBy, string createdByName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdBy",
                Value = createdBy ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdByName",
                Size = 510,
                Value = createdByName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_CANCEL_ORDER_NO_FEE] @id, @createdBy, @createdByName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<int> SP_CROSS_ORDER_ADD_SERVICEAsync(Guid? id, DataTable serviceAdd, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "serviceAdd",
                Value = serviceAdd ?? Convert.DBNull,
                SqlDbType = SqlDbType.Structured,
                TypeName = "[so].[OrderServiceAddType]",
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_CROSS_ORDER_ADD_SERVICE] @id, @serviceAdd, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_GET_BID_CREDIT_SETUPResult>> SP_GET_BID_CREDIT_SETUPAsync(Guid? accountId, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_BID_CREDIT_SETUPResult>("EXEC @returnValue = [so].[SP_GET_BID_CREDIT_SETUP] @accountId", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_CREATE_INVOICE_PAY_ORDER_EXPRESSResult>> SP_CREATE_INVOICE_PAY_ORDER_EXPRESSAsync(Guid? id, decimal? totalPay, Guid? accountId, Guid? createdBy, string createdByName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "totalPay",
                Precision = 19,
                Scale = 4,
                Value = totalPay ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdBy",
                Value = createdBy ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdByName",
                Size = 510,
                Value = createdByName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_CREATE_INVOICE_PAY_ORDER_EXPRESSResult>("EXEC @returnValue = [so].[SP_CREATE_INVOICE_PAY_ORDER_EXPRESS] @id, @totalPay, @accountId, @createdBy, @createdByName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_CREATE_INVOICE_WITHDRAWResult>> SP_CREATE_INVOICE_WITHDRAWAsync(Guid? accountId, string walletCode, decimal? amount, string method, string note, string rawData, string document, Guid? createdBy, string createdByName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "amount",
                Precision = 19,
                Scale = 4,
                Value = amount ?? Convert.DBNull,
                SqlDbType = SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "method",
                Size = 50,
                Value = method ?? Convert.DBNull,
                SqlDbType = SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 510,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "rawData",
                Size = 510,
                Value = rawData ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "document",
                Size = 510,
                Value = document ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "createdBy",
                Value = createdBy ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdByName",
                Size = 510,
                Value = createdByName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_CREATE_INVOICE_WITHDRAWResult>("EXEC @returnValue = [so].[SP_CREATE_INVOICE_WITHDRAW] @accountId, @walletCode, @amount, @method, @note, @rawData, @document, @createdBy, @createdByName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public async Task<List<SP_ORDER_STATUS_COUNTERResult>> SP_ORDER_STATUS_COUNTERAsync(DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_ORDER_STATUS_COUNTERResult>("EXEC @returnValue = [so].[SP_ORDER_STATUS_COUNTER] @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public async Task<List<SP_DEPOSIT_WALLETResult>> SP_DEPOSIT_WALLETAsync(Guid? accountId, string walletCode, decimal? amount, string paymentCode, string paymentNote, DateTime? paymentDate, string document, Guid? createdById, string createdByName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = System.Data.ParameterDirection.Output,
            SqlDbType = System.Data.SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "accountId",
                Value = accountId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "walletCode",
                Size = 50,
                Value = walletCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "amount",
                Precision = 19,
                Scale = 4,
                Value = amount ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Money,
            },
            new SqlParameter
            {
                ParameterName = "paymentCode",
                Size = 50,
                Value = paymentCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "paymentNote",
                Size = 510,
                Value = paymentNote ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "paymentDate",
                Value = paymentDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "document",
                Size = 510,
                Value = document ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "createdById",
                Value = createdById ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "createdByName",
                Size = 510,
                Value = createdByName ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DEPOSIT_WALLETResult>("EXEC @returnValue = [so].[SP_DEPOSIT_WALLET] @accountId, @walletCode, @amount, @paymentCode, @paymentNote, @paymentDate, @document, @createdById, @createdByName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }


    public async Task<List<SP_ORDER_UPDATE_BID_USERNAMEResult>> SP_ORDER_UPDATE_BID_USERNAMEAsync(Guid? id, string bidUsername, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterReturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "bidUsername",
                Size = 255,
                Value = bidUsername ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterReturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_ORDER_UPDATE_BID_USERNAMEResult>("EXEC @returnValue = [so].[SP_ORDER_UPDATE_BID_USERNAME] @id, @bidUsername, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterReturnValue.Value);

        return _;
    }

    public async Task<List<SP_ORDER_UPDATE_ADD_TRACKINGResult>> SP_ORDER_UPDATE_ADD_TRACKINGAsync(
        Guid? id,
        string trackingNumber,
        string trackingCarrier,
        Guid? userId,
        string userName,
        NetDevPack.Domain.OutputParameter<int> returnValue = null,
        CancellationToken cancellationToken = default)
    {
        var parameterReturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "trackingNumber",
                Size = 50,
                Value = trackingNumber ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "trackingCarrier",
                Size = 250,
                Value = trackingCarrier ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterReturnValue,
        };
        var _ = await
            _context.SqlQueryAsync<SP_ORDER_UPDATE_ADD_TRACKINGResult>("EXEC @returnValue = [so].[SP_ORDER_UPDATE_ADD_TRACKING] " +
            "@id, " +
            "@trackingNumber, " +
            "@trackingCarrier, " +
            "@userId, " +
            "@userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterReturnValue.Value);

        return _;
    }

    public async Task<List<SP_ORDER_UPDATE_TRACKINGResult>> SP_ORDER_UPDATE_TRACKINGAsync(
        Guid? id,
        string trackingNumber,
        string trackingCarrier,
        int domesticStatus,
        Guid? userId,
        string userName,
        NetDevPack.Domain.OutputParameter<int> returnValue = null,
        CancellationToken cancellationToken = default)
    {
        var parameterReturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "trackingNumber",
                Size = 50,
                Value = trackingNumber ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "trackingCarrier",
                Size = 250,
                Value = trackingCarrier ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "domesticStatus",
                Value = domesticStatus,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterReturnValue,
        };
        var _ = await
            _context.SqlQueryAsync<SP_ORDER_UPDATE_TRACKINGResult>("EXEC @returnValue = [so].[SP_ORDER_UPDATE_TRACKING] " +
            "@id, " +
            "@trackingNumber, " +
            "@trackingCarrier, " +
            "@domesticStatus, " +
            "@userId, " +
            "@userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterReturnValue.Value);

        return _;
    }

    public async Task<List<SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSResult>> SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSAsync(
        Guid? id,
        int? domesticStatus,
        Guid? userId,
        string userName,
        NetDevPack.Domain.OutputParameter<int> returnValue = null,
        CancellationToken cancellationToken = default)
    {
        var parameterReturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "domesticStatus",
                Value = domesticStatus ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterReturnValue,
        };
        var _ = await
            _context.SqlQueryAsync<SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUSResult>("EXEC @returnValue = [so].[SP_ORDER_UPDATE_DOMESTIC_DELIVERRY_STATUS] " +
            "@id, " +
            "@domesticStatus, " +
            "@userId, " +
            "@userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterReturnValue.Value);

        return _;
    }

    public async Task<List<SP_ORDER_UPDATE_SHIPMENT_ROUTINGResult>> SP_ORDER_UPDATE_SHIPMENT_ROUTINGAsync(Guid? id, string? shipmentRouting, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterReturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "shipmentRouting",
                Size = 150,
                Value = shipmentRouting ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterReturnValue,
        };
        var _ = await
            _context.SqlQueryAsync<SP_ORDER_UPDATE_SHIPMENT_ROUTINGResult>
                ("EXEC @returnValue = [so].[SP_ORDER_UPDATE_SHIPMENT_ROUTING] " +
                "@id, " +
                "@shipmentRouting, " +
                "@userId, " +
                "@userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterReturnValue.Value);

        return _;
    }


    public async Task<List<SP_ORDER_UPDATE_NOTEResult>> SP_ORDER_UPDATE_NOTEAsync(Guid? id, string? note, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterReturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 1500,
                Value = note ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterReturnValue,
        };
        var _ = await
            _context.SqlQueryAsync<SP_ORDER_UPDATE_NOTEResult>
                ("EXEC @returnValue = [so].[SP_ORDER_UPDATE_NOTE] " +
                "@id, " +
                "@note, " +
                "@userId, " +
                "@userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterReturnValue.Value);

        return _;
    }

    public async Task<List<SP_ORDER_UPDATE_DOMESTIC_DELIVERYResult>> SP_ORDER_UPDATE_DOMESTIC_DELIVERYAsync(
        Guid? id,
        DateTime? domesticDeliveryDate,
        int? domesticStatus,
        Guid? userId,
        string userName,
        NetDevPack.Domain.OutputParameter<int> returnValue = null,
        CancellationToken cancellationToken = default)
    {
        var parameterReturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = ParameterDirection.Output,
            SqlDbType = SqlDbType.Int,
        };
        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
            ParameterName = "domesticDeliveryDate",
            Value = domesticDeliveryDate.HasValue ? (object)domesticDeliveryDate.Value : DBNull.Value,
            SqlDbType = SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "domesticStatus",
                Value = domesticStatus ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = SqlDbType.NVarChar,
            },
            parameterReturnValue,
        };
        var _ = await
        _context.SqlQueryAsync<SP_ORDER_UPDATE_DOMESTIC_DELIVERYResult>
            ("EXEC @returnValue = [so].[SP_ORDER_UPDATE_DOMESTIC_DELIVERY] " +
            "@id, " +
            "@domesticDeliveryDate, " +
            "@domesticStatus, " +
            "@userId, " +
            "@userName",
            sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterReturnValue.Value);
        return null;
    }
}
