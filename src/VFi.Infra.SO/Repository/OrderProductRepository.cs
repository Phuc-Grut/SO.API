using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul.Filtering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class OrderProductRepository : IOrderProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderProduct> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderProductRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderProduct>();
    }

    public void Add(OrderProduct productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(OrderProduct t)
    {
        DbSet.Remove(t);
    }

    public void Update(OrderProduct t)
    {
        DbSet.Update(t);
    }

    public async Task<IEnumerable<OrderProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ProductName.Contains(keyword) || x.ProductCode.Contains(keyword) || x.OrderCode.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<OrderProduct>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("id"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Id.ToString()));
            }
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("quotationId"))
            {
                query = query.Where(x => x.QuotationId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("contractId"))
            {
                query = query.Where(x => x.ContractId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<OrderProduct> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<OrderProduct> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<OrderProduct> t)
    {
        DbSet.RemoveRange(t);
    }

    public async Task<IEnumerable<OrderProduct>> FilterContract(Guid id)
    {
        return await DbSet.Where(x => x.ContractId == id).OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public async Task<IEnumerable<OrderProduct>> GetById(IEnumerable<ExportWarehouseProduct> exportWarehouseProduct)
    {
        var query = DbSet.AsQueryable();
        var List = exportWarehouseProduct.ToList().Select(x => x.OrderProductId).ToList();
        return await query.Where(x => List.Contains(x.Id)).ToListAsync();
    }
    public async Task<IEnumerable<OrderProduct>> GetByOrderIds(IEnumerable<Guid> orderIds)
    {
        return await DbSet.Include(x => x.Order)
            .Where(op => op.OrderId.HasValue && orderIds.Contains(op.OrderId.Value))
            .ToListAsync();
    }


    public async Task<(IEnumerable<OrderProduct>, int)> Filter(string keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.Include(x => x.Order).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ProductCode.Contains(keyword)
            || x.ProductName.Contains(keyword)
            || x.OrderCode.Contains(keyword)
            || x.UnitCode.Contains(keyword)
            || x.UnitName.Contains(keyword)
          );
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.Order.CustomerId.Equals(item.Value));
            }
            if (item.Key.Equals("diferenceStatus"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => !list.Contains(x.Order.Status.ToString()));
            }
            if (item.Key.Equals("orderType"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Order.OrderType));
            }

            if (item.Key.Equals("status"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Order.Status.ToString()));
            }
            if (item.Key.Equals("fromDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.Order.OrderDate >= date);
            }
            if (item.Key.Equals("toDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value).Add(new TimeSpan(23, 59, 59)) : DateTime.Now;
                query = query.Where(x => x.Order.OrderDate <= date);
            }
            if (item.Key.Equals("statusReturn") && item.Value == "0")
            {
                query = query.Where(x => x.Quantity > (x.QuantityReturned ?? 0));
            }
            if (item.Key.Equals("statusSales") && item.Value == "0")
            {
                query = query.Where(x => x.Quantity > (x.QuantitySales ?? 0));
            }
            if (item.Key.Equals("warehouseId"))
            {
                query = query.Where(x => x.WarehouseId.Equals(item.Value));
            }
            if (item.Key.Equals("currency"))
            {
                query = query.Where(x => x.Order.Currency.Equals(item.Value));
            }
            if (item.Key.Equals("statusPurchase") && item.Value == "0")
            {
                query = query.Where(x => x.Quantity > (x.QuantityPurchased ?? 0));
            }
            if (item.Key.Equals("statusProduction") && item.Value == "0")
            {
                query = query.Where(x => (double?)x.Quantity > (x.QuantityProductioned ?? 0));
            }
            if (item.Key.Equals("statusExport") && item.Value == "0")
            {
                query = query.Where(x => x.Quantity > (x.QuantityExported ?? 0));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderByDescending(x => x.Order.Code).ThenByDescending(y => y.Order.OrderDate).ThenBy(y => y.DisplayOrder).ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<OrderProduct>> GetContractByOrderId(string code)
    {
        return await DbSet.Where(x => x.OrderCode == code).Include(x => x.Contract).ToListAsync();
    }
    public async Task<IEnumerable<OrderProduct>> GetQuotationByOrderId(string code)
    {
        return await DbSet.Where(x => x.OrderCode == code).Include(x => x.Quotation).ToListAsync();
    }
}
