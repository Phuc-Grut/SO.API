using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ProductionOrderRepository : IProductionOrderRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ProductionOrder> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ProductionOrderRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ProductionOrder>();
    }

    public void Add(ProductionOrder ProductionOrder)
    {
        DbSet.Add(ProductionOrder);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ProductionOrder>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ProductionOrder> GetById(Guid id)
    {
        return await DbSet.Include(x => x.ProductionOrdersDetail).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Remove(ProductionOrder ProductionOrder)
    {
        DbSet.Remove(ProductionOrder);
    }

    public void Update(ProductionOrder ProductionOrder)
    {
        DbSet.Update(ProductionOrder);
    }
    public async Task<ProductionOrder> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<(IEnumerable<ProductionOrder>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.Code.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<ProductionOrder>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<ProductionOrder>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.SaleEmployeeId == (Guid)item.Value);
            }
        }
        return await query.ToListAsync();
    }
}
