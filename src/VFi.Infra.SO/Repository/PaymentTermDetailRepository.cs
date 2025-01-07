using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

internal class PaymentTermDetailRepository : IPaymentTermDetailRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<PaymentTermDetail> DbSet;

    public PaymentTermDetailRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PaymentTermDetail>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(PaymentTermDetail t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PaymentTermDetail>> Filter(Guid id, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x).Where(x => x.PaymentTermId == id);
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(Guid id)
    {
        var query = DbSet.Select(x => x).Where(x => x.PaymentTermId == id);
        return await query.CountAsync();
    }

    public async Task<IEnumerable<PaymentTermDetail>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<PaymentTermDetail>> GetAll(Guid id)
    {
        return await DbSet.Where(x => x.PaymentTermId == id).ToListAsync();
    }

    public async Task<PaymentTermDetail> GetById(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }


    public void Remove(PaymentTermDetail t)
    {
        DbSet.Remove(t);
    }

    public void Update(PaymentTermDetail t)
    {
        DbSet.Update(t);
    }
    public void Add(IEnumerable<PaymentTermDetail> list)
    {
        DbSet.AddRange(list);
    }
    public void Update(IEnumerable<PaymentTermDetail> list)
    {
        DbSet.UpdateRange(list);
    }
    public void Remove(IEnumerable<PaymentTermDetail> list)
    {
        DbSet.RemoveRange(list);
    }
}
