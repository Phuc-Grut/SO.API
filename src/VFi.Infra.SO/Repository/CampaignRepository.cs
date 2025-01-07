using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class CampaignRepository : ICampaignRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Campaign> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CampaignRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Campaign>();
    }


    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Campaign>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Campaign> GetById(Guid id)
    {
        return await DbSet.Include(x => x.CampaignStatuses).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(Campaign Campaign)
    {
        DbSet.Add(Campaign);
    }

    public void Update(Campaign Campaign)
    {
        DbSet.Update(Campaign);
    }

    public void Remove(Campaign Campaign)
    {
        DbSet.Remove(Campaign);
    }

    public async Task<Campaign> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<Campaign>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        //var query = DbSet.Include(x => x.LeadCampaigns).ThenInclude(x=>x.Lead).AsQueryable();
        var query = DbSet.Include(x => x.LeadCampaigns).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%") || EF.Functions.Like(x.LeaderName, $"%{keyword}%"));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.Leader == new Guid(item.Value + "") || x.Member.Contains(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.Select(item => new Campaign()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            Leader = item.Leader,
            LeaderName = item.LeaderName,
            Member = item.Member,
            Status = item.Status,
            CreatedDate = item.CreatedDate,
            CreatedBy = item.CreatedBy,
            UpdatedDate = item.UpdatedDate,
            UpdatedBy = item.UpdatedBy,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            LeadCount = item.LeadCampaigns.Count
        }).ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<Campaign>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.CreatedDate).ToListAsync();
    }
    public void Update(IEnumerable<Campaign> t)
    {
        DbSet.UpdateRange(t);
    }
    public bool CheckUsing(Guid id)
    {
        var campaign = DbSet.Where(x => x.Id == id).FirstOrDefault();
        return !(
            Db.Lead.Any(x => x.Id == campaign.Id));
    }
}
