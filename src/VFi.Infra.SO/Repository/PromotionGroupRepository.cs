using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class PromotionGroupRepository : IPromotionGroupRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PromotionGroup> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PromotionGroupRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PromotionGroup>();
    }

    public void Add(PromotionGroup PromotionGroup)
    {
        DbSet.Add(PromotionGroup);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PromotionGroup>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PromotionGroup> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PromotionGroup PromotionGroup)
    {
        DbSet.Remove(PromotionGroup);
    }

    public void Update(PromotionGroup PromotionGroup)
    {
        DbSet.Update(PromotionGroup);
    }

    public async Task<PromotionGroup> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<PromotionGroup>, int)> Filter(string? keyword, int? status, IFopRequest request)
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


    public async Task<IEnumerable<PromotionGroup>> GetListCbx(int? status)
    {
        return await DbSet.Where(x => x.Status == status || status == null).ToListAsync();
    }
}
