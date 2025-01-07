
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public class SalesDiscountProductRepository : ISalesDiscountProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<SalesDiscountProduct> DbSet;
    protected readonly DbSet<Models.Order> DbSetPP;
    public IUnitOfWork UnitOfWork => Db;
    public SalesDiscountProductRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<SalesDiscountProduct>();
        DbSetPP = Db.Set<Models.Order>();
    }

    public void Add(SalesDiscountProduct item)
    {
        DbSet.Add(item);
    }

    public void Add(IEnumerable<SalesDiscountProduct> t)
    {
        DbSet.AddRange(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<SalesDiscountProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<SalesDiscountProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(SalesDiscountProduct item)
    {
        DbSet.Remove(item);
    }
    public void Remove(IEnumerable<SalesDiscountProduct> t)
    {
        DbSet.RemoveRange(t);
    }

    public void Update(SalesDiscountProduct item)
    {
        DbSet.Update(item);
    }
    public void Update(IEnumerable<SalesDiscountProduct> t)
    {
        DbSet.UpdateRange(t);
    }
    public async Task<IEnumerable<SalesDiscountProduct>> GetByParentId(Guid id)
    {
        return await DbSet.Where(x => x.SalesDiscountId == id).OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public async Task<IEnumerable<SalesDiscountProduct>> GetByParentId(List<Guid> id)
    {
        return await DbSet.Where(x => id.Contains((Guid)x.SalesDiscountId)).OrderBy(x => x.DisplayOrder).ToListAsync();
    }

    public async Task<IEnumerable<SalesDiscountProduct>> GetByPurchaseProductId(Guid id)
    {
        return await DbSet.Where(x => x.OrderProductId == id).OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public async Task<IEnumerable<SalesDiscountProduct>> GetByOrderId(string code)
    {
        return await DbSet.Where(x => x.SalesOrderCode == code).Include(x => x.SalesDiscount).ToListAsync();
        // return await DbSet.Include(x => x.OrderProduct).ThenInclude(y => y.Order).Where(x => x.OrderProduct.Order.Code == code).Include(x => x.ReturnOrder).ToListAsync();

    }

}
