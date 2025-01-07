using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class LeadActivityRepository : ILeadActivityRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<LeadActivity> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public LeadActivityRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<LeadActivity>();
    }


    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<LeadActivity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<LeadActivity> GetById(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(LeadActivity LeadActivity)
    {
        DbSet.Add(LeadActivity);
    }

    public void Update(LeadActivity LeadActivity)
    {
        DbSet.Update(LeadActivity);
    }

    public void Remove(LeadActivity LeadActivity)
    {
        DbSet.Remove(LeadActivity);
    }

    public async Task<(IEnumerable<LeadActivity>, int)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Campaign.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }



    public void Update(IEnumerable<LeadActivity> t)
    {
        DbSet.UpdateRange(t);
    }
    public bool CheckUsing(Guid id)
    {
        var LeadActivity = DbSet.Where(x => x.Id == id).FirstOrDefault();
        return !(
            Db.Lead.Any(x => x.Id == LeadActivity.Id));
    }
}
