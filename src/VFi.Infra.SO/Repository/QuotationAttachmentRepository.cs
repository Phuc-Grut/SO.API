using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;

namespace VFi.Infra.SO.Repository;

internal class QuotationAttachmentRepository : IQuotationAttachmentRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<QuotationAttachment> DbSet;

    public QuotationAttachmentRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<QuotationAttachment>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(QuotationAttachment t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<QuotationAttachment>> Filter(Guid id, int pagesize, int pageindex)
    {
        var query = DbSet.Select(x => x).Where(x => x.QuotationId == id);
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<int> FilterCount(Guid id)
    {
        var query = DbSet.Select(x => x).Where(x => x.QuotationId == id);
        return await query.CountAsync();
    }

    public async Task<IEnumerable<QuotationAttachment>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<QuotationAttachment>> GetAll(Guid id)
    {
        return await DbSet.Where(x => x.QuotationId == id).ToListAsync();
    }

    public async Task<QuotationAttachment> GetById(Guid id)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
    }


    public void Remove(QuotationAttachment t)
    {
        DbSet.Remove(t);
    }

    public void Update(QuotationAttachment t)
    {
        DbSet.Update(t);
    }
}
