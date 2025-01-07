using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class QuotationTermRepository : IQuotationTermRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<QuotationTerm> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public QuotationTermRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<QuotationTerm>();
    }

    public void Add(QuotationTerm QuotationTerm)
    {
        DbSet.Add(QuotationTerm);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<QuotationTerm>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<QuotationTerm> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(QuotationTerm QuotationTerm)
    {
        DbSet.Remove(QuotationTerm);
    }

    public void Update(QuotationTerm QuotationTerm)
    {
        DbSet.Update(QuotationTerm);
    }

    public async Task<QuotationTerm> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<(IEnumerable<QuotationTerm>, int)> Filter(string? keyword, int? status, IFopRequest request)
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

    public async Task<IEnumerable<QuotationTerm>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.DisplayOrder).ToListAsync();
    }
    public void Update(IEnumerable<QuotationTerm> t)
    {
        DbSet.UpdateRange(t);
    }
}
