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

public class DeliveryMethodRepository : IDeliveryMethodRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<DeliveryMethod> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public DeliveryMethodRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<DeliveryMethod>();
    }

    public void Add(DeliveryMethod DeliveryMethod)
    {
        DbSet.Add(DeliveryMethod);
        //DbSet.Add(Contact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<DeliveryMethod>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<DeliveryMethod> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(DeliveryMethod DeliveryMethod)
    {
        DbSet.Remove(DeliveryMethod);
    }

    public void Update(DeliveryMethod DeliveryMethod)
    {
        DbSet.Update(DeliveryMethod);
    }

    public async Task<DeliveryMethod> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<IEnumerable<DeliveryMethod>> GetListCbx(int? status)
    {
        return await DbSet.Where(x => x.Status == status).ToListAsync();
    }

    public async Task<(IEnumerable<DeliveryMethod>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || x.Code.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }
}
