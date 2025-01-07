using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ContractTermRepository : IContractTermRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ContractTerm> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ContractTermRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ContractTerm>();
    }

    public void Add(ContractTerm ContractTerm)
    {
        DbSet.Add(ContractTerm);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ContractTerm>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ContractTerm> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ContractTerm ContractTerm)
    {
        DbSet.Remove(ContractTerm);
    }

    public void Update(ContractTerm ContractTerm)
    {
        DbSet.Update(ContractTerm);
    }

    public async Task<ContractTerm> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<ContractTerm>, int)> Filter(string? keyword, int? status, IFopRequest request)
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

    public async Task<IEnumerable<ContractTerm>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }

    public void Update(IEnumerable<ContractTerm> t)
    {
        DbSet.UpdateRange(t);
    }
}
