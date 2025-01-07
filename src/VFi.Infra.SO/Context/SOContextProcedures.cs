

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Domain;

namespace VFi.Infra.PIM.Context;


public partial class SOContextProcedures : ISOContextProcedures
{
    private readonly SqlCoreContext _context;

    public SOContextProcedures(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
    }
    public virtual async Task<int> SP_CROSS_ORDER_RECALCULATE_PRICEAsync(Guid? id, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var outputParameter = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = System.Data.ParameterDirection.Output,
            SqlDbType = System.Data.SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            new SqlParameter
            {
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 255,
                Value = userName ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            outputParameter,
        };

        var _ =  await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_CROSS_ORDER_RECALCULATE_PRICE] @id, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue((int)outputParameter.Value);
        
        return _;
    }

    public virtual async Task<List<SP_GET_TOTAL_COSTESTIMATE_Result>> SP_GET_TOTAL_COSTESTIMATEAsync(string? ListProductOrderDetailId, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Ids",
                Size = -1,
                Value = ListProductOrderDetailId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_GET_TOTAL_COSTESTIMATE_Result>("EXEC @returnValue = [mes].[SP_GET_TOTAL_COSTESTIMATE] @Ids", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_COUNT_CUSTOMERResult>> SP_DB_COUNT_CUSTOMERAsync(NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
    {
        var parameterreturnValue = new SqlParameter
        {
            ParameterName = "returnValue",
            Direction = System.Data.ParameterDirection.Output,
            SqlDbType = System.Data.SqlDbType.Int,
        };

        var sqlParameters = new[]
        {
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_COUNT_CUSTOMERResult>("EXEC @returnValue = [so].[SP_DB_COUNT_CUSTOMER]", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_CREATE_ORDERAsync(Guid? id, string code, Guid? customerId, string orderCode, string storeCode, string channelCode, string currencyCode, decimal? exchangeRate, string paymentTermCode, string paymentMethodCode, string shippingMethodCode, int? buyFee, string routerShipping, DataTable products, string image, string description, string note, Guid? userId, string userName, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "id",
                Value = id ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Value = code ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "customerId",
                Value = customerId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "orderCode",
                Size = 50,
                Value = orderCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "storeCode",
                Size = 50,
                Value = storeCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "channelCode",
                Size = 50,
                Value = channelCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "currencyCode",
                Size = 50,
                Value = currencyCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "exchangeRate",
                Precision = 18,
                Value = exchangeRate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Decimal,
            },
            new SqlParameter
            {
                ParameterName = "paymentTermCode",
                Size = 50,
                Value = paymentTermCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "paymentMethodCode",
                Size = 50,
                Value = paymentMethodCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "shippingMethodCode",
                Size = 50,
                Value = shippingMethodCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "buyFee",
                Value = buyFee ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "routerShipping",
                Size = 510,
                Value = routerShipping ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "products",
                Value = products ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Structured,
                TypeName = "[so].[ProductType]",
            },
            new SqlParameter
            {
                ParameterName = "image",
                Size = 510,
                Value = image ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "description",
                Size = 200,
                Value = description ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "note",
                Size = 1000,
                Value = note ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            new SqlParameter
            {
                ParameterName = "userId",
                Value = userId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "userName",
                Size = 510,
                Value = userName ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.NVarChar,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_CREATE_ORDER] @id, @code, @customerId, @orderCode, @storeCode, @channelCode, @currencyCode, @exchangeRate, @paymentTermCode, @paymentMethodCode, @shippingMethodCode, @buyFee, @routerShipping, @products, @image, @description, @note, @userId, @userName", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<List<SP_DB_OVERVIEWResult>> SP_DB_OVERVIEWAsync(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_OVERVIEWResult>("EXEC @returnValue = [so].[SP_DB_OVERVIEW] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_SELLING_PRODUCTResult>> SP_DB_SELLING_PRODUCTAsync(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_SELLING_PRODUCTResult>("EXEC @returnValue = [so].[SP_DB_SELLING_PRODUCT] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_CUSTMER_SALEResult>> SP_DB_CUSTMER_SALEAsync(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_CUSTMER_SALEResult>("EXEC @returnValue = [so].[SP_DB_CUSTMER_SALE] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_STORE_SALEResult>> SP_DB_STORE_SALEAsync(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_STORE_SALEResult>("EXEC @returnValue = [so].[SP_DB_STORE_SALE] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_SALESCHANNEL_SALEResult>> SP_DB_SALESCHANNEL_SALEAsync(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_SALESCHANNEL_SALEResult>("EXEC @returnValue = [so].[SP_DB_SALESCHANNEL_SALE] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }



    public virtual async Task<int> SP_Rpt_SoChiTietBanHang_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, int? Status, int? DiferenceStatus, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "Status",
                Value = Status ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "DiferenceStatus",
                Value = DiferenceStatus ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_SoChiTietBanHang_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate, @Status, @DiferenceStatus", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_Rpt_TongHopBanHangTheoKhachHang_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TongHopBanHangTheoKhachHang_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_Rpt_TongHopBanHangTheoMatHang_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TongHopBanHangTheoMatHang_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_Rpt_TinhHinhThucHienDonBanHang_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, int? Status, int? DiferenceStatus, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "Status",
                Value = Status ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "DiferenceStatus",
                Value = DiferenceStatus ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TinhHinhThucHienDonBanHang_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate, @Status, @DiferenceStatus", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<int> SP_Rpt_TinhHinhThucHienHopDongBan_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, int? Status, int? DiferenceStatus, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "Status",
                Value = Status ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            new SqlParameter
            {
                ParameterName = "DiferenceStatus",
                Value = DiferenceStatus ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TinhHinhThucHienHopDongBan_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate, @Status, @DiferenceStatus", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_Rpt_TongHopBanHangTheoNhanVien_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TongHopBanHangTheoNhanVien_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_Rpt_TongHopBanHangTheoNhomKhachHang_LoadDataAsync(Guid? ReportId, string EmployeeId, string CustomerGroupId, DateTime? FromDate, DateTime? ToDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = -1,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CustomerGroupId",
                Size = -1,
                Value = CustomerGroupId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TongHopBanHangTheoNhomKhachHang_LoadData] @ReportId, @EmployeeId, @CustomerGroupId, @FromDate, @ToDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_Rpt_TongHopBanHangTheoKhachHangVaDonBanHang_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TongHopBanHangTheoKhachHangVaDonBanHang_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<int> SP_Rpt_TongHopBanHangTheoMatHangVaKhachHang_LoadDataAsync(Guid? ReportId, string CustomerCode, string EmployeeId, string CategoryRootId, string ProductCode, DateTime? FromDate, DateTime? ToDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "ReportId",
                Value = ReportId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            },
            new SqlParameter
            {
                ParameterName = "CustomerCode",
                Size = 250,
                Value = CustomerCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "EmployeeId",
                Size = 500,
                Value = EmployeeId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "CategoryRootId",
                Size = 250,
                Value = CategoryRootId ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "ProductCode",
                Size = 250,
                Value = ProductCode ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "FromDate",
                Value = FromDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "ToDate",
                Value = ToDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [so].[SP_Rpt_TongHopBanHangTheoMatHangVaKhachHang_LoadData] @ReportId, @CustomerCode, @EmployeeId, @CategoryRootId, @ProductCode, @FromDate, @ToDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }
    public virtual async Task<List<SP_DB_SALES_PRODUCT_BY_TIME>> SP_DB_SALES_PRODUCT_BY_TIME(string Currency, DateTime Yaer, int Type, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "Year",
                Value = Yaer ,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "Type",
                Value = Type,
                SqlDbType = System.Data.SqlDbType.Int,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_SALES_PRODUCT_BY_TIME>("EXEC @returnValue = [so].[SP_DB_Sales_Product_By_Time] @Currency, @Year, @Type", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_CONTRACT>> SP_DB_CONTRACT(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_CONTRACT>("EXEC @returnValue = [so].[SP_DB_CONTRACT] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_EMPLOYEE_EXCELLENT>> SP_DB_EMPLOYEE_EXCELLENT(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_EMPLOYEE_EXCELLENT>("EXEC @returnValue = [so].[SP_DB_EMPLOYEE_EXCELLENT] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<List<SP_DB_PAYMENT>> SP_DB_PAYMENT(string Currency, DateTime? StartDate, DateTime? EndDate, NetDevPack.Domain.OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                ParameterName = "Currency",
                Size = 50,
                Value = Currency ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            },
            new SqlParameter
            {
                ParameterName = "StartDate",
                Value = StartDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            new SqlParameter
            {
                ParameterName = "EndDate",
                Value = EndDate ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.DateTime,
            },
            parameterreturnValue,
        };
        var _ = await _context.SqlQueryAsync<SP_DB_PAYMENT>("EXEC @returnValue = [so].[SP_DB_PAYMENT] @Currency, @StartDate, @EndDate", sqlParameters, cancellationToken);

        returnValue?.SetValue(parameterreturnValue.Value);

        return _;
    }

    public virtual async Task<SP_ORDER_CROSS_RECALCULATED_PRICEResult> SP_GET_CROSS_ORDER_RECALCULATE_PRICEAsync(Guid? id, NetDevPack.Domain.OutputParameter<int> returnValue, CancellationToken cancellationToken)
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
            ParameterName = "id",
            Value = id ?? (object)DBNull.Value,
            SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
        },
        parameterreturnValue
    };
        var _ = await _context.SqlQueryAsync<SP_ORDER_CROSS_RECALCULATED_PRICEResult>(
            "EXEC @returnValue = so.SP_CROSS_ORDER_RECALCULATE_PRICE_PREVIEW @id",
            sqlParameters,
            cancellationToken);
        returnValue?.SetValue((int)parameterreturnValue.Value);
        return _.FirstOrDefault();
    }
}
