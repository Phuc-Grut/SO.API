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

public class PromotionProductRepository : IPromotionProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PromotionProduct> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PromotionProductRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PromotionProduct>();
    }

    public void Add(PromotionProduct productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PromotionProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PromotionProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PromotionProduct productAttributeOption)
    {
        DbSet.Remove(productAttributeOption);
    }

    public void Update(PromotionProduct productAttributeOption)
    {
        DbSet.Update(productAttributeOption);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }
    public void Update(IEnumerable<PromotionProduct> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<PromotionProduct> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<PromotionProduct> t)
    {
        DbSet.RemoveRange(t);
    }
}
