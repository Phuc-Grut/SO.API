using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class CustomerSourceRepository : ICustomerSourceRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CustomerSource> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerSourceRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CustomerSource>();
    }

    public void Add(CustomerSource CustomerSource)
    {
        DbSet.Add(CustomerSource);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CustomerSource>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CustomerSource> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(CustomerSource CustomerSource)
    {
        DbSet.Remove(CustomerSource);
    }

    public void Update(CustomerSource CustomerSource)
    {
        DbSet.Update(CustomerSource);
    }

    public async Task<CustomerSource> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<CustomerSource>, int)> Filter(string? keyword, int? status, IFopRequest request)
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
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<CustomerSource>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<CustomerSource> t)
    {
        DbSet.UpdateRange(t);
    }

    public async Task<IEnumerable<CustomerSource>> Filter(IEnumerable<string>? name)
    {
        var a = name?.Select(x => x.ToLower());
        var query = DbSet.Where(x => a.Contains(x.Name.ToLower()));
        return await query.ToListAsync();
    }
}
