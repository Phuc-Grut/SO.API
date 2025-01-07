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

namespace VFi.Infra.SO.Repository;

public class OrderInvoiceRepository : IOrderInvoiceRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<OrderInvoice> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public OrderInvoiceRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<OrderInvoice>();
    }

    public void Add(OrderInvoice productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<OrderInvoice>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<OrderInvoice> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(OrderInvoice t)
    {
        DbSet.Remove(t);
    }

    public void Update(OrderInvoice t)
    {
        DbSet.Update(t);
    }
    public void Update(IEnumerable<OrderInvoice> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<OrderInvoice> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<OrderInvoice> t)
    {
        DbSet.RemoveRange(t);
    }
}
