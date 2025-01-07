using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class CampaignStatusRepository : ICampaignStatusRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<CampaignStatus> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CampaignStatusRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<CampaignStatus>();
    }


    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<CampaignStatus>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<CampaignStatus> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    public void Add(CampaignStatus CampaignStatus)
    {
        DbSet.Add(CampaignStatus);
    }

    public void Update(CampaignStatus CampaignStatus)
    {
        DbSet.Update(CampaignStatus);
    }

    public void Remove(CampaignStatus CampaignStatus)
    {
        DbSet.Remove(CampaignStatus);
    }
    public async Task<(IEnumerable<CampaignStatus>, int)> Filter(string? keyword, int? status, Guid? campaignId, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || EF.Functions.Like(x.Description, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        if (campaignId != null)
        {
            query = query.Where(x => x.CampaignId == campaignId);
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.OrderBy(x => x.DisplayOrder).ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<CampaignStatus>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Add(IEnumerable<CampaignStatus> list)
    {
        DbSet.AddRange(list);
    }
    public void Update(IEnumerable<CampaignStatus> list)
    {
        DbSet.UpdateRange(list);
    }
    public void Remove(IEnumerable<CampaignStatus> list)
    {
        DbSet.RemoveRange(list);
    }
    public bool CheckUsing(Guid id)
    {
        var campaignStatus = DbSet.Where(x => x.Id == id).FirstOrDefault();
        return !(
            Db.Campaign.Any(x => x.Id == campaignStatus.CampaignId));
    }
}
