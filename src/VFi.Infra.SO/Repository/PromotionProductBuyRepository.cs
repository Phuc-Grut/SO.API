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

public class PromotionProductBuyRepository : IPromotionProductBuyRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PromotionProductBuy> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PromotionProductBuyRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PromotionProductBuy>();
    }

    public void Add(PromotionProductBuy productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PromotionProductBuy>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PromotionProductBuy> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PromotionProductBuy productAttributeOption)
    {
        DbSet.Remove(productAttributeOption);
    }

    public void Update(PromotionProductBuy productAttributeOption)
    {
        DbSet.Update(productAttributeOption);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }
    public void Update(IEnumerable<PromotionProductBuy> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<PromotionProductBuy> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<PromotionProductBuy> t)
    {
        DbSet.RemoveRange(t);
    }
}
