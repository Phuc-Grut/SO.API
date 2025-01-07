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

public class ExportWarehouseProductRepository : IExportWarehouseProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ExportWarehouseProduct> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ExportWarehouseProductRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ExportWarehouseProduct>();
    }

    public void Add(ExportWarehouseProduct productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ExportWarehouseProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ExportWarehouseProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ExportWarehouseProduct t)
    {
        DbSet.Remove(t);
    }

    public void Update(ExportWarehouseProduct t)
    {
        DbSet.Update(t);
    }

    public async Task<IEnumerable<ExportWarehouseProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ExportWarehouseProduct>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {

            if (item.Key.Equals("exportWarehouseId", StringComparison.OrdinalIgnoreCase))
            {
                if (Guid.TryParse(item.Value.ToString(), out var exportWarehouseId))
                {
                    query = query.Where(x => x.ExportWarehouseId == exportWarehouseId);
                }
            }
            if (item.Key.Equals("orderProductId"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.OrderProductId.ToString()));
            }
            if (item.Key.Equals("id"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Id.ToString()));
            }
            if (item.Key.Equals("orderIds"))
            {
                var orderIds = item.Value as List<Guid>;
                if (orderIds != null && orderIds.Any())
                {
                    query = query.Where(x => orderIds.Contains(x.OrderId ?? Guid.Empty));
                }
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public async Task<(IEnumerable<ExportWarehouseProduct>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.Include(x => x.ExportWarehouse).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ExportWarehouse.Code.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public void Update(IEnumerable<ExportWarehouseProduct> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<ExportWarehouseProduct> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<ExportWarehouseProduct> t)
    {
        DbSet.RemoveRange(t);
    }

    public async Task<IEnumerable<ExportWarehouseProduct>> FilterContract(Guid id)
    {
        return await DbSet.Where(x => x.ExportWarehouseId == id).ToListAsync();
    }
    public async Task<IEnumerable<ExportWarehouseProduct>> GetByOrderId(string code)
    {
        return await DbSet.Where(x => x.OrderCode == code).Include(x => x.ExportWarehouse).ToListAsync();
    }
    public async Task<IEnumerable<ExportWarehouseProduct>> GetByOrderIds(IEnumerable<Guid> ids)
    {
        return await DbSet.Include(x=> x.ExportWarehouse)
            .Where(x => x.OrderId.HasValue && ids.Contains(x.OrderId.Value)).ToListAsync();
    }

    public async Task<IEnumerable<ExportWarehouseProduct>> GetByExportWarehouseId(Guid exportWarehouseId)
    {
        return await DbSet.Where(x => x.ExportWarehouseId == exportWarehouseId).ToListAsync();
    }
}
