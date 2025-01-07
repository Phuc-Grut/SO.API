using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class LeadCampaignRepository : ILeadCampaignRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<LeadCampaign> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public LeadCampaignRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<LeadCampaign>();
    }


    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<LeadCampaign>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<LeadCampaign> GetById(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(LeadCampaign LeadCampaign)
    {
        DbSet.Add(LeadCampaign);
    }

    public void Update(LeadCampaign LeadCampaign)
    {
        DbSet.Update(LeadCampaign);
    }

    public void Remove(LeadCampaign LeadCampaign)
    {
        DbSet.Remove(LeadCampaign);
    }

    public async Task<(IEnumerable<LeadCampaign>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.Include(x => x.Lead).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || EF.Functions.Like(x.Email, $"%{keyword}%") || EF.Functions.Like(x.Phone, $"%{keyword}%"));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("campaignId"))
            {
                query = query.Where(x => x.CampaignId == new Guid(item.Value + ""));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.Leader == new Guid(item.Value + "") || x.Member.Contains(item.Value + ""));
            }
            if (item.Key.Equals("leader"))
            {
                query = query.Where(x => x.Leader == new Guid(item.Value + ""));
            }
            if (item.Key.Equals("isState"))
            {
                query = query.Where(x => (x.StateId == null && Convert.ToInt32(item.Value) == 0) || (x.StateId != null && Convert.ToInt32(item.Value) == 1));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<LeadCampaign>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("campaignId"))
            {
                query = query.Where(x => x.CampaignId == (Guid)item.Value);
            }
            if (item.Key.Equals("id"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Id.ToString()));
            }
        }
        return await query.ToListAsync();
    }
    public async Task<IEnumerable<LeadCampaign>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.CreatedDate).ToListAsync();
    }
    public void Add(IEnumerable<LeadCampaign> t)
    {
        DbSet.AddRange(t);
    }
    public void Update(IEnumerable<LeadCampaign> t)
    {
        DbSet.UpdateRange(t);
    }
    public void Remove(IEnumerable<LeadCampaign> t)
    {
        DbSet.RemoveRange(t);
    }
    public bool CheckUsing(Guid id)
    {
        var LeadCampaign = DbSet.Where(x => x.Id == id).FirstOrDefault();
        return !(
            Db.Lead.Any(x => x.Id == LeadCampaign.Id));
    }
}
