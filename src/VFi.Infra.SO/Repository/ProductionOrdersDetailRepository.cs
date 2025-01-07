using System.Net.NetworkInformation;
using Consul;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Domain.SO.Interfaces;

public class ProductionOrdersDetailRepository : IProductionOrdersDetailRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ProductionOrdersDetail> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ProductionOrdersDetailRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ProductionOrdersDetail>();
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ProductionOrdersDetail>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ProductionOrdersDetail> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    public void Add(ProductionOrdersDetail _ProductionOrdersDetail)
    {
        DbSet.Add(_ProductionOrdersDetail);
    }
    public void Remove(ProductionOrdersDetail _ProductionOrdersDetail)
    {
        DbSet.Remove(_ProductionOrdersDetail);
    }

    public void Update(ProductionOrdersDetail _ProductionOrdersDetail)
    {
        DbSet.Update(_ProductionOrdersDetail);
    }

    public async Task<ProductionOrdersDetail> GetById(string id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }
    public async Task<(IEnumerable<ProductionOrdersDetail>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.ProductCode.Contains(keyword) || x.ProductName.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.Include(x => x.ProductionOrder).ToListAsync(), totalCount);
    }

    public async Task<(IEnumerable<ProductionOrdersDetail>, int)> Filter(string? keyword, IFopRequest request, int? type, int? productOrderStatus)
    {
        var query = DbSet.Include(x => x.ProductionOrder).AsQueryable();
        if (type != null)
        {
            query = query.Where(x => x.ProductionOrder.Type == type);
        }
        if (productOrderStatus != null)
        {
            query = query.Where(x => x.ProductionOrder.Status == productOrderStatus);
        }
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => (x.ProductCode.Contains(keyword) || x.ProductName.Contains(keyword)));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<(IEnumerable<ProductionOrdersDetail>, int)> FilterBom(string? keyword, IFopRequest request, int? type)
    {
        var query = DbSet.Include(x => x.ProductionOrder).AsQueryable();

        if (type != null)
        {
            query = query.Where(x => x.ProductionOrder.Type == type);
        }
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => (x.ProductCode.Contains(keyword) || x.ProductName.Contains(keyword)));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);

    }

    public async Task<int> FilterCount(string? keyword, int? status, string? warehouseId)
    {
        var query = DbSet.AsQueryable();
        return await query.OrderBy(x => x.CreatedDate).CountAsync();
    }

    public async Task<IEnumerable<ProductionOrdersDetail>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<ProductionOrdersDetail>> GetAll(Guid id)
    {
        return await DbSet.Where(x => x.ProductionOrdersId == id).ToListAsync();
    }
    public void Add(IEnumerable<ProductionOrdersDetail> list)
    {
        DbSet.AddRange(list);
    }
    public void Update(IEnumerable<ProductionOrdersDetail> list)
    {
        DbSet.UpdateRange(list);
    }
    public void Remove(IEnumerable<ProductionOrdersDetail> list)
    {
        DbSet.RemoveRange(list);
    }
}
