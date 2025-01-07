using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ReportTypeRepository : IReportTypeRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<ReportType> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ReportTypeRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<ReportType>();
    }

    public void Add(ReportType ReportType)
    {
        DbSet.Add(ReportType);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<ReportType>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<ReportType> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(ReportType ReportType)
    {
        DbSet.Remove(ReportType);
    }

    public void Update(ReportType ReportType)
    {
        DbSet.Update(ReportType);
    }

    public async Task<ReportType> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
}
