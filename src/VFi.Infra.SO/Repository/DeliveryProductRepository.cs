using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class DeliveryProductRepository : IDeliveryProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<DeliveryProduct> DbSet;
    protected readonly DbSet<OrderProduct> DbSetOrder;
    public IUnitOfWork UnitOfWork => Db;
    public DeliveryProductRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<DeliveryProduct>();
        DbSetOrder = Db.Set<OrderProduct>();
    }

    public void Add(DeliveryProduct DeliveryProduct)
    {
        DbSet.Add(DeliveryProduct);
        //DbSet.Add(Contact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<DeliveryProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<DeliveryProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(DeliveryProduct DeliveryProduct)
    {
        DbSet.Remove(DeliveryProduct);
    }

    public void Update(DeliveryProduct DeliveryProduct)
    {
        DbSet.Update(DeliveryProduct);
    }

    public async Task<DeliveryProduct> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<IEnumerable<DeliveryProduct>> GetListCbx(int? status)
    {
        return await DbSet.Where(x => x.Status == status).ToListAsync();
    }

    public async Task<(IEnumerable<DeliveryProduct>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || x.Code.Contains(keyword));
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

    public async Task<IEnumerable<DeliveryProduct>> GetByDeliveryProductId(Guid id)
    {
        return await DbSet.Where(x => x.OrderProductId == id).ToListAsync();

    }

    public async Task<IEnumerable<DeliveryProduct>> GetByDeliveryProductId(List<Guid> listId)
    {
        return await DbSet.Where(x => listId.Contains(x.OrderProductId)).ToListAsync();

    }

    public void Update(IEnumerable<DeliveryProduct> t)
    {
        DbSet.UpdateRange(t);

    }

    public void Add(IEnumerable<DeliveryProduct> t)
    {
        DbSet.AddRange(t);

    }

    public void Remove(IEnumerable<DeliveryProduct> t)
    {
        DbSet.RemoveRange(t);

    }

    public async Task<IEnumerable<DeliveryProduct>> GetListDetaiId(string id)
    {
        var list = id.Split(",").ToList();
        var query = DbSet.Where(x => list.Contains(x.OrderProductId.ToString())).AsQueryable().OrderBy(x => x.DisplayOrder);
        return await query.ToListAsync();
    }

    public async Task<(IEnumerable<DeliveryProduct>, int)> Filter(IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<DeliveryProduct>> GetByOrderId(Guid id)
    {
        var query = DbSetOrder.Where(x => x.OrderId == id).Include(x => x.DeliveryProduct).AsQueryable();
        return await query.SelectMany(x => x.DeliveryProduct).ToListAsync();
    }
}
