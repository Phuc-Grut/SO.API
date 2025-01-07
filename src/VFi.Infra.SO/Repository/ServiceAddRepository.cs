using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

public class ServiceAddRepository : IServiceAddRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ServiceAdd> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ServiceAddRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ServiceAdd>();
    }

    public void Add(ServiceAdd serviceAdd)
    {
        DbSet.Add(serviceAdd);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ServiceAdd>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ServiceAdd> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ServiceAdd serviceAdd)
    {
        DbSet.Remove(serviceAdd);
    }

    public void Update(ServiceAdd serviceAdd)
    {
        DbSet.Update(serviceAdd);
    }
    public async Task<bool> CheckExist(string? code, Guid? id)
    {
        if (id == null)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            return await DbSet.AnyAsync(x => x.Code.Equals(code));
        }
        return await DbSet.AnyAsync(x => x.Code.Equals(code) && x.Id != id);
    }
    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }
    public async Task<IEnumerable<ServiceAdd>> Filter(string? keyword, int? status, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, int? status)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<ServiceAdd>> GetListListBox(string? keyword, int? status)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword)
            || EF.Functions.Like(x.Name, $"%{keyword}%")
            || (!string.IsNullOrEmpty(x.Tags) && x.Tags.Contains(keyword)));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public async Task<IEnumerable<ServiceAdd>> Filter(string keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            else if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.Code.Contains(item.Value.ToString()));
            }
            if (item.Key.Equals("tags"))
            {
                query = query.Where(x => x.Tags.Contains(item.Value.ToString()));
            }

        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<ServiceAdd> serviceAdds)
    {
        DbSet.UpdateRange(serviceAdds);
    }
}
