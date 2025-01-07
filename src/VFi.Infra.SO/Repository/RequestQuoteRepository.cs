using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class RequestQuoteRepository : IRequestQuoteRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<RequestQuote> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public RequestQuoteRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<RequestQuote>();
    }

    public void Add(RequestQuote RequestQuote)
    {
        DbSet.Add(RequestQuote);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<RequestQuote>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<RequestQuote> GetById(Guid id)
    {
        return await DbSet.Include(x => x.Quotation).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Remove(RequestQuote RequestQuote)
    {
        DbSet.Remove(RequestQuote);
    }

    public void Update(RequestQuote RequestQuote)
    {
        DbSet.Update(RequestQuote);
    }

    public async Task<RequestQuote> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<RequestQuote>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.Code.Contains(keyword));
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.EmployeeId == new Guid(item.Value + "") || x.CreatedBy == new Guid(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<RequestQuote>> GetListCbx(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId.Equals(item.Value) || x.CustomerId == null);
            }
        }
        return await query.ToListAsync();
    }
    public void UpdateStatus(RequestQuote t)
    {
        Db.Entry(t).Property(x => x.Status).IsModified = true;
    }

    public async Task<IEnumerable<RequestQuote>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.EmployeeId == (Guid)item.Value);
            }
        }
        return await query.ToListAsync();
    }
}
