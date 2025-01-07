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

internal class QuotationRepository : IQuotationRepository
{
    protected readonly SqlCoreContext Db;

    protected readonly DbSet<Quotation> DbSet;

    public QuotationRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Quotation>();
    }

    public IUnitOfWork UnitOfWork => Db;

    public void Add(Quotation t)
    {
        DbSet.Add(t);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<(IEnumerable<Quotation>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(x => x.CustomerName.Contains(keyword) || x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.AccountId == new Guid(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<Quotation>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Quotation> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }

    public async Task<Quotation> GetById(Guid id)
    {
        return await DbSet.Include(x => x.OrderProduct).Include(x => x.OrderServiceAdd).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Quotation>> GetListBox(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Include(x => x.Contract).AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || x.CustomerName.Contains(keyword) || x.AccountName.Contains(keyword));
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == Convert.ToInt32(item.Value));
            }
            if (item.Key.Equals("date"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.ExpiredDate >= date);
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId == new Guid(item.Value.ToString()));
            }
            if (item.Key.Equals("isContract") && item.Value == "0")
            {
                var listContract = Db.Contract.Where(x => x.QuotationId != null).Select(y => y.QuotationId).ToList();
                query = query.Where(x => !listContract.Contains(x.Id));
            }
            if (item.Key.Equals("isOrder") && item.Value == "0")
            {
                var listOrder = Db.Order.Where(x => x.QuotationId != null).Select(y => y.QuotationId).ToList();
                query = query.Where(x => !listOrder.Contains(x.Id));
            }
            if (item.Key.Equals("fromDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.Date >= date);
            }
            if (item.Key.Equals("toDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value).Add(new TimeSpan(23, 59, 59)) : DateTime.Now;
                query = query.Where(x => x.Date <= date);
            }
            if (item.Key.Equals("currency"))
            {
                query = query.Where(x => x.Currency.Equals(item.Value));
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public void Remove(Quotation t)
    {
        DbSet.Remove(t);
    }

    public void Update(Quotation t)
    {
        DbSet.Update(t);
    }
    public void Approve(Quotation t)
    {
        Db.Entry(t).Property(x => x.Status).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveDate).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveBy).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveByName).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveComment).IsModified = true;
    }
    public void UploadFile(Quotation t)
    {
        Db.Entry(t).Property(x => x.File).IsModified = true;
    }

    public async Task<IEnumerable<Quotation>> Filter(Guid accountId)
    {
        var query = DbSet.AsQueryable();

        query = query.Where(x => accountId == x.AccountId);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Quotation>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.AccountId == (Guid)item.Value);
            }
            if (item.Key.Equals("groupEmployeeId"))
            {
                query = query.Where(x => x.GroupEmployeeId == (Guid)item.Value);
            }
            if (item.Key.Equals("quotationTermId"))
            {
                query = query.Where(x => x.QuotationTermId == (Guid)item.Value);
            }
        }
        return await query.ToListAsync();
    }
}
