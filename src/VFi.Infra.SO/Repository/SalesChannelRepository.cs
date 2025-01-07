using System.Linq;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class SalesChannelRepository : ISalesChannelRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<SalesChannel> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public SalesChannelRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<SalesChannel>();
    }

    public void Add(SalesChannel SalesChannel)
    {
        DbSet.Add(SalesChannel);
        //DbSet.Add(Contact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<SalesChannel>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<SalesChannel> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(SalesChannel SalesChannel)
    {
        DbSet.Remove(SalesChannel);
    }

    public void Update(SalesChannel SalesChannel)
    {
        DbSet.Update(SalesChannel);
    }

    public async Task<SalesChannel> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<IEnumerable<SalesChannel>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }


    public async Task<(IEnumerable<SalesChannel>, int)> Filter(string? keyword, int? status, IFopRequest request)
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
}
