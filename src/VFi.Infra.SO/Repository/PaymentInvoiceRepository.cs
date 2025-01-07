using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class PaymentInvoiceRepository : IPaymentInvoiceRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<PaymentInvoice> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    //public PaymentInvoiceRepository(IServiceProvider serviceProvider)
    //{
    //    var scope = serviceProvider.CreateScope();
    //    Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
    //    DbSet = Db.Set<PaymentInvoice>();
    //}

    public PaymentInvoiceRepository(SqlCoreContext context)
    {
        Db = context;
        DbSet = Db.Set<PaymentInvoice>();
    }


    public void Add(PaymentInvoice t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<(IEnumerable<PaymentInvoice>, int, IEnumerable<PaymentInvoice>)> Filter(string? keyword, int? status, IFopRequest request)
    {
        var query = DbSet.Include(x => x.SaleDiscount).Include(x => x.ReturnOrder).AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x =>
        x.Code.Contains(keyword)
        || x.Note.Contains(keyword)
        || x.CustomerName.Contains(keyword)
        || x.OrderCode.Contains(keyword)
        || x.SaleDiscount.Code.Contains(keyword)
        || x.ReturnOrder.Code.Contains(keyword)
        );
        var (filtered, totalCount) = query.ApplyFop(request);
        var output = DbSet.AsQueryable().Where(x => DateTime.Now.Year == x.PaymentDate.Value.Year && DateTime.Now.Month == x.PaymentDate.Value.Month);
        return (await filtered.ToListAsync(), totalCount, output);
    }

    public async Task<IEnumerable<PaymentInvoice>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<PaymentInvoice> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<PaymentInvoice> GetById(Guid id)
    {
        return await DbSet.Include(x => x.SaleDiscount).Include(x => x.ReturnOrder).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<PaymentInvoice>> Filter(Dictionary<string, object> filter, int? top)
    {
        var query = DbSet.AsQueryable();
        foreach (var item in filter)
        {
            if (item.Key.Equals("code"))
            {
                query = query.Where(x => x.Code.Contains(item.Value + ""));
            }
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("orderId"))
            {
                query = query.Where(x => x.OrderId.Equals(item.Value + ""));
            }
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.AccountId == (Guid)item.Value);
            }
            if (item.Key.Equals("pay_start_date"))
            {
                var startDate = (DateTime)item.Value;
                query = query.Where(x => x.PaymentDate > startDate);
            }
            if (item.Key.Equals("pay_end_date"))
            {
                var endDate = (DateTime)item.Value;
                query = query.Where(x => x.PaymentDate < endDate);
            }
        }
        if (top.HasValue)
            query = query.OrderByDescending(x => x.PaymentDate).Take(top.Value);
        return await query.ToListAsync();
    }

    public void Remove(PaymentInvoice t)
    {
        DbSet.Remove(t);
    }

    public void Update(PaymentInvoice t)
    {
        DbSet.Update(t);
    }
    public void Update(IEnumerable<PaymentInvoice> details)
    {
        DbSet.UpdateRange(details);
    }
    public void Add(IEnumerable<PaymentInvoice> details)
    {
        DbSet.AddRange(details);
    }
    public void Remove(IEnumerable<PaymentInvoice> t)
    {
        DbSet.RemoveRange(t);
    }
    public void Changelocked(PaymentInvoice t)
    {
        Db.Entry(t).Property(x => x.Locked).IsModified = true;
    }
}
