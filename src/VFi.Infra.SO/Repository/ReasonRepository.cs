using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ReasonRepository : IReasonRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Reason> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ReasonRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Reason>();
    }

    public void Add(Reason Reason)
    {
        DbSet.Add(Reason);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Reason>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Reason> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(Reason Reason)
    {
        DbSet.Remove(Reason);
    }

    public void Update(Reason Reason)
    {
        DbSet.Update(Reason);
    }

    public async Task<Reason> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<Reason> GetByName(string name)
    {
        return await DbSet.FirstOrDefaultAsync(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
    }
    public async Task<(IEnumerable<Reason>, int)> Filter(string? keyword, int? status, IFopRequest request)
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
        return (await filtered.ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<Reason>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }
}
