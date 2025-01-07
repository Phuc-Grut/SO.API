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

public class ContractTypeRepository : IContractTypeRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ContractType> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ContractTypeRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ContractType>();
    }

    public void Add(ContractType ContractType)
    {
        DbSet.Add(ContractType);
        //DbSet.Add(Contact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ContractType>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ContractType> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ContractType ContractType)
    {
        DbSet.Remove(ContractType);
    }

    public void Update(ContractType ContractType)
    {
        DbSet.Update(ContractType);
    }

    public async Task<ContractType> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<IEnumerable<ContractType>> GetListCbx(int? status)
    {
        return await DbSet.Where(x => x.Status == status || status == null).ToListAsync();
    }


    public async Task<(IEnumerable<ContractType>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
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
