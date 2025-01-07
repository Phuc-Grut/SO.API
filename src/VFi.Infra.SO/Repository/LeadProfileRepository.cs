using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class LeadProfileRepository : ILeadProfileRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<LeadProfile> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public LeadProfileRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<LeadProfile>();
    }


    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<LeadProfile>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<LeadProfile> GetById(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(LeadProfile LeadProfile)
    {
        DbSet.Add(LeadProfile);
    }

    public void Update(LeadProfile LeadProfile)
    {
        DbSet.Update(LeadProfile);
    }

    public void Remove(LeadProfile LeadProfile)
    {
        DbSet.Remove(LeadProfile);
    }

    public async Task<(IEnumerable<LeadProfile>, int)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Description, $"%{keyword}%"));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public void Update(IEnumerable<LeadProfile> t)
    {
        DbSet.UpdateRange(t);
    }

    public bool CheckUsing(Guid id)
    {
        var LeadProfile = DbSet.Where(x => x.Id == id).FirstOrDefault();
        return !(
            Db.Lead.Any(x => x.Id == LeadProfile.Id));
    }
}
