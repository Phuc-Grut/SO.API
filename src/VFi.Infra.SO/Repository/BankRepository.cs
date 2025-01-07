using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class BankRepository : IBankRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Bank> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public BankRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Bank>();
    }
    public void Add(Bank Bank)
    {
        DbSet.Add(Bank);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Bank>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Bank> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(Bank Bank)
    {
        DbSet.Remove(Bank);
    }

    public void Update(Bank Bank)
    {
        DbSet.Update(Bank);
    }

    public async Task<Bank> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<Bank>, int)> Filter(string? keyword, int? status, IFopRequest request)
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


    public async Task<IEnumerable<Bank>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<Bank> t)
    {
        DbSet.UpdateRange(t);
    }
    public bool CheckUsing(Guid id)
    {
        var bank = DbSet.Where(x => x.Id == id).FirstOrDefault();
        return !(
            Db.CustomerBank.Any(x => x.BankCode == bank.Code));
    }
}
