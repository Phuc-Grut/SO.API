using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class GroupEmployeeRepository : IGroupEmployeeRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<GroupEmployee> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public GroupEmployeeRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<GroupEmployee>();
    }

    public void Add(GroupEmployee GroupEmployee)
    {
        DbSet.Add(GroupEmployee);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<GroupEmployee>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<GroupEmployee> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(GroupEmployee GroupEmployee)
    {
        DbSet.Remove(GroupEmployee);
    }

    public void Update(GroupEmployee GroupEmployee)
    {
        DbSet.Update(GroupEmployee);
    }

    public async Task<GroupEmployee> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<GroupEmployee>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.Include(x => x.GroupEmployeeMapping).AsQueryable();
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
    public async Task<IEnumerable<GroupEmployee>> GetListCbx(Dictionary<string, object> filter)
    {
        var query = DbSet.Include(x => x.GroupEmployeeMapping).AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("leadId"))
            {
                query = query.Where(x => x.GroupEmployeeMapping.Where(x => x.IsLeader == true).Select(x => x.EmployeeId).Contains(new Guid(item.Value + "")));
            }
        }
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<GroupEmployee>> GetByListId(List<Guid>? listId)
    {
        if (listId == null || !listId.Any())
        {
            return Enumerable.Empty<GroupEmployee>();
        }
        var query = DbSet.AsQueryable();
        query = query.Where(x => listId.Contains(x.Id));
        return await query.Include(x => x.GroupEmployeeMapping).ToListAsync();
    }

    public async Task<IEnumerable<GroupEmployee>> Filter(IEnumerable<string>? name)
    {
        var a = name?.Select(x => x.ToLower());
        var query = DbSet.Where(x => a.Contains(x.Name.ToLower()));
        return await query.ToListAsync();
    }
}
