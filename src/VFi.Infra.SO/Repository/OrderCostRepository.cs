using System.Linq;
using System.Security.Principal;
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

public class OrderCostRepository : IOrderCostRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderCost> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderCostRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderCost>();
    }

    public void Add(OrderCost OrderCost)
    {
        DbSet.Add(OrderCost);
        //DbSet.Add(Contact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderCost>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderCost> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(OrderCost OrderCost)
    {
        DbSet.Remove(OrderCost);
    }

    public void Update(OrderCost OrderCost)
    {
        DbSet.Update(OrderCost);
    }

    public async Task<OrderCost> GetByCode(string? code)
    {
        return await DbSet.FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<OrderCost>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x);
        //if (!String.IsNullOrEmpty(keyword))
        //{
        //    query = DbSet.Where(x => x.Description.Contains(keyword));
        //}

        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<IEnumerable<OrderCost>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }
    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        return await query.CountAsync();
    }

    public async Task<(IEnumerable<OrderCost>, int)> Filter(IFopRequest request)
    {
        var (filtered, totalCount) = DbSet.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
}
