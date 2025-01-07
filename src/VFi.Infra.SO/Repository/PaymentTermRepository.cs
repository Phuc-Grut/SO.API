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
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

internal class PaymentTermRepository : IPaymentTermRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<PaymentTerm> DbSet;

    public PaymentTermRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PaymentTerm>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(PaymentTerm t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<(IEnumerable<PaymentTerm>, int)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%") || EF.Functions.Like(x.Description, $"%{keyword}%"));
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<PaymentTerm>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PaymentTerm> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<PaymentTerm> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<PaymentTerm>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }

    public void Remove(PaymentTerm t)
    {
        DbSet.Remove(t);
    }

    public void Update(PaymentTerm t)
    {
        DbSet.Update(t);
    }
}
