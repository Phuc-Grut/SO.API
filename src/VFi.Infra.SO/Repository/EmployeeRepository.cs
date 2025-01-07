using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class EmployeeRepository : IEmployeeRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Employee> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public EmployeeRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Employee>();
    }

    public void Add(Employee Employee)
    {
        DbSet.Add(Employee);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Employee> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    public async Task<Employee> GetByIdQuery(Guid id)
    {
        var query = DbSet.Where(x => x.Id == id);
        return await query.Include(x => x.CustomerAddresses).Include(x => x.CustomerBanks).Include(x => x.CustomerContacts).FirstOrDefaultAsync();
    }

    public async Task<Employee> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<Employee> GetByAccountId(Guid? accountId)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.AccountId == (accountId ?? Guid.Empty));
    }

    public void Remove(Employee Employee)
    {
        DbSet.Remove(Employee);
    }

    public void Update(Employee Employee)
    {
        DbSet.Update(Employee);
    }

    public async Task<(IEnumerable<Employee>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || x.Email.Contains(keyword) || x.Code.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("groupId"))
            {
                query = query.Where(x => x.GroupEmployeeMapping.Any(g => g.GroupEmployeeId.Equals(item.Value)));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
    public async Task<IEnumerable<Employee>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.AccountId.ToString()));
            }
            if (item.Key.Equals("id"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Id.ToString()));
            }
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("groupId"))
            {
                query = query.Where(x => x.GroupEmployeeMapping.Any(g => g.GroupEmployeeId.Equals(item.Value)));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<IEnumerable<Employee>> GetListCbx(int? status, Guid? groupId)
    {
        var query = DbSet.Include(x => x.GroupEmployeeMapping).ThenInclude(y => y.GroupEmployee).AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        if (groupId != null)
        {
            query = query.Where(x => x.GroupEmployeeMapping.Any(g => g.GroupEmployeeId.Equals(groupId)));
        }
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Employee>> Filter(IEnumerable<string>? name)
    {
        var a = name?.Select(x => x.ToLower());
        var query = DbSet.Where(x => a.Contains(x.Name.ToLower()));
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByListId(string? listId)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(listId))
        {
            var ids = listId?.Split(',').ToList();
            query = query.Where(x => ids.Contains(x.AccountId.ToString()));
        }
        return await query.ToListAsync();
    }
}
