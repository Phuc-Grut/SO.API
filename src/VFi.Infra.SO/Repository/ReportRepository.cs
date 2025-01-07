using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ReportRepository : IReportRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Report> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ReportRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Report>();
    }

    public void Add(Report item)
    {
        DbSet.Add(item);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Report>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Report> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);

    }

    public void Remove(Report item)
    {
        DbSet.Remove(item);
    }

    public void Update(Report item)
    {
        DbSet.Update(item);
    }

    public async Task<(IEnumerable<Report>, int)> Filter(string? keyword, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => EF.Functions.Like(x.Name, $"%{keyword}%") || x.CustomerName.Contains(keyword) || x.EmployeeName.Contains(keyword) || x.ProductName.Contains(keyword));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

}
