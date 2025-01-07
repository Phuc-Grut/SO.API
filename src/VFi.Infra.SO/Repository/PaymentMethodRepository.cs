using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PaymentMethod> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public PaymentMethodRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<PaymentMethod>();
    }

    public void Add(PaymentMethod PaymentMethod)
    {
        DbSet.Add(PaymentMethod);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<PaymentMethod>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PaymentMethod> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(PaymentMethod PaymentMethod)
    {
        DbSet.Remove(PaymentMethod);
    }

    public void Update(PaymentMethod PaymentMethod)
    {
        DbSet.Update(PaymentMethod);
    }

    public async Task<PaymentMethod> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<PaymentMethod>, int)> Filter(string? keyword, int? status, IFopRequest request)
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


    public async Task<IEnumerable<PaymentMethod>> GetListCbx(int? status)
    {
        return await DbSet.ToListAsync();
    }
}
