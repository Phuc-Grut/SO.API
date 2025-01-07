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

public class ExportProductRepository : IExportProductRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ExportProduct> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ExportProductRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ExportProduct>();
    }

    public void Add(ExportProduct productAttributeOption)
    {
        DbSet.Add(productAttributeOption);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ExportProduct>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ExportProduct> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ExportProduct t)
    {
        DbSet.Remove(t);
    }

    public void Update(ExportProduct t)
    {
        DbSet.Update(t);
    }

    public async Task<IEnumerable<ExportProduct>> GetListListBox(Dictionary<string, object> filter, string? keyword)
    {
        var query = DbSet.AsQueryable();
        return await query.ToListAsync();
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ExportProduct>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("exportId"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.ExportId.ToString()));
            }
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<ExportProduct> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<ExportProduct> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<ExportProduct> t)
    {
        DbSet.RemoveRange(t);
    }

    public async Task<IEnumerable<ExportProduct>> FilterContract(Guid id)
    {
        return await DbSet.Where(x => x.ExportId == id).ToListAsync();
    }
}
