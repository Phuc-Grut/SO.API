using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

public class GroupEmployeeMappingRepository : IGroupEmployeeMappingRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<GroupEmployeeMapping> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public GroupEmployeeMappingRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<GroupEmployeeMapping>();
    }

    public void Add(GroupEmployeeMapping GroupEmployeeMapping)
    {
        DbSet.Add(GroupEmployeeMapping);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<GroupEmployeeMapping>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.EmployeeId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("groupEmployeeId"))
            {
                query = query.Where(x => x.GroupEmployeeId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(string? keyword, Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.EmployeeId.Equals(new Guid(item.Value + "")));
            }
            if (item.Key.Equals("groupEmployeeId"))
            {
                query = query.Where(x => x.GroupEmployeeId.Equals(new Guid(item.Value + "")));
            }
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<GroupEmployeeMapping>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<GroupEmployeeMapping> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(GroupEmployeeMapping GroupEmployeeMapping)
    {
        DbSet.Remove(GroupEmployeeMapping);
    }

    public void Update(GroupEmployeeMapping GroupEmployeeMapping)
    {
        DbSet.Update(GroupEmployeeMapping);
    }

    public async Task<bool> CheckExistById(Guid id)
    {
        return await DbSet.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<GroupEmployeeMapping>> Filter(Guid id)
    {
        return await DbSet.Include(x => x.GroupEmployee).Where(x => x.EmployeeId == id).OrderBy(x => x.CreatedDate).ToListAsync();
    }
    public void Update(IEnumerable<GroupEmployeeMapping> options)
    {
        DbSet.UpdateRange(options);
    }
    public void Add(IEnumerable<GroupEmployeeMapping> options)
    {
        DbSet.AddRange(options);
    }
    public void Remove(IEnumerable<GroupEmployeeMapping> t)
    {
        DbSet.RemoveRange(t);
    }
}
