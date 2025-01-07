using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PO.Interfaces;

public class SalesDiscountRepository : ISalesDiscountRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<SalesDiscount> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public SalesDiscountRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<SalesDiscount>();
    }

    public void Add(SalesDiscount item)
    {
        DbSet.Add(item);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<SalesDiscount>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<SalesDiscount> GetById(Guid id)
    {
        return await DbSet.Include(x => x.PaymentInvoice).Include(x => x.SalesDiscountProduct).Include(x => x.CustomerAddress).FirstOrDefaultAsync(x => x.Id == id);
    }


    public void Remove(SalesDiscount item)
    {
        DbSet.Remove(item);
    }

    public void Update(SalesDiscount item)
    {
        DbSet.Update(item);
    }

    public async Task<SalesDiscount> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));

    }
    public async Task<IEnumerable<SalesDiscount>> GetByOrder(Guid id)
    {
        return await DbSet.Where(x => x.SalesOrderId == id).ToListAsync();
    }
    public async Task<IEnumerable<SalesDiscount>> GetByOrder(string ordercode, bool? getDetail)
    {
        var query = DbSet.Where(x => x.SalesOrderCode == ordercode).AsQueryable();
        if (getDetail == true)
        {
            query = query.Include(x => x.SalesDiscountProduct);
        }
        return await query.ToListAsync();
    }
    public async Task<IEnumerable<SalesDiscount>> GetByOrder(List<string> ordercode, bool? getDetail)
    {
        var query = DbSet.Where(x => ordercode.Contains(x.SalesOrderCode)).AsQueryable();
        if (getDetail == true)
        {
            query = query.Include(x => x.SalesDiscountProduct);
        }
        return await query.ToListAsync();
    }
    public async Task<(IEnumerable<SalesDiscount>, int)> Filter(string? keyword, Guid? customerId, int? status, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) ||
            EF.Functions.Like(x.CurrencyName, $"%{keyword}%") ||
            x.SalesOrderCode.Contains(keyword));
        }
        if (customerId != null)
        {
            query = query.Where(x => x.CustomerId == customerId);
        }
        //if(startDate != null && endDate != null)
        //{
        //    query = query.Where(x => x.ReturnDate >= startDate && x.ReturnDate <= endDate);
        //}
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.Include(x => x.Customer).Include(x => x.CustomerAddress).ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<SalesDiscountProduct>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        string[]? listCurrency = null;
        string[]? listCustomer = null;
        string[]? listProduct = null;
        string[]? listEmp = null;
        DateTime? fromDate = null;
        DateTime? toDate = null;
        int? status = null;
        bool? notWithOrder = false;

        foreach (var item in filter)
        {
            if (item.Key.Equals("listCustomer") && item.Value != "")
            {
                listCustomer = ((string)item.Value).Split(",");
            }
            else if (item.Key.Equals("listCurrency") && item.Value != "")
            {
                listCurrency = ((string)item.Value).Split(",");
            }
            else if (item.Key.Equals("listEmp") && item.Value != "")
            {
                listEmp = ((string)item.Value).Split(",");
            }
            else if (item.Key.Equals("listProduct") && item.Value != "")
            {
                listProduct = ((string)item.Value).Split(",");
            }
            else if (item.Key.Equals("fromDate"))
            {
                fromDate = Convert.ToDateTime(item.Value);
            }
            else if (item.Key.Equals("toDate"))
            {
                toDate = Convert.ToDateTime(item.Value).Add(new TimeSpan(23, 59, 59));
            }
            else if (item.Key.Equals("status"))
            {
                status = Convert.ToInt32(item.Value);
            }
            else if (item.Key.Equals("notWithOrder"))
            {
                notWithOrder = Convert.ToBoolean(item.Value);
            }
        }
        query = query.Where(x => (listCustomer == null || listCustomer.Contains(x.CustomerId.ToString()))
            && (listEmp == null || listEmp.Contains(x.EmployeeId.ToString()))
            && (listCurrency == null || listCurrency.Contains(x.CurrencyCode))
            && (fromDate == null || fromDate <= (DateTime)x.DiscountDate)
            && (toDate == null || toDate >= (DateTime)x.DiscountDate)
            && (status == null || x.Status == status)
        );
        if (notWithOrder == true)
        {
            query = query.Where(x => x.SalesOrderId == null);
        }
        var result = query.Include(x => x.SalesDiscountProduct)
            .SelectMany(x =>
                x.SalesDiscountProduct
                .Where(y => listProduct == null || listProduct.Contains(y.ProductId.ToString()))
                .Select(y => new SalesDiscountProduct
                {
                    Id = y.Id,
                    ProductId = y.ProductId,
                    Quantity = y.Quantity,
                    TaxRate = y.TaxRate,
                    UnitCode = y.UnitCode,
                    UnitPrice = y.UnitPrice,
                    UnitType = y.UnitType
                })
            );

        return await result.ToListAsync();
    }
}
