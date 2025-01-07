using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Domain.SO.Interfaces;

public partial class CustomerRepository : ICustomerRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Customer> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public CustomerRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Customer>();
    }

    public void Add(Customer item)
    {
        DbSet.Add(item);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Customer> GetById(Guid id)
    {
        return await DbSet.Include(x => x.CustomerAddress).Include(x => x.CustomerContact).Include(x => x.CustomerBank).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<Customer> GetByAccountId(Guid accountId)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.AccountId == accountId);
    }
    public async Task<Guid> GetIdByAccountId(Guid accountId)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.AccountId == accountId).Select(x => x.Id);
    }
    public async Task<Customer> GetByAccountEmail(string email)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.AccountEmail == email);
    }
    public async Task<Customer> GetByAccountUsername(string username)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.AccountUsername == username);
    }
    public void Remove(Customer Customer)
    {
        DbSet.Remove(Customer);
    }

    public void Update(Customer Customer)
    {
        DbSet.Update(Customer);
    }
    public async Task<bool> CheckTaxCode(string? taxCode, Guid? id)
    {
        if (String.IsNullOrEmpty(taxCode))
        {
            return false;
        }
        return await DbSet.AnyAsync(x => x.TaxCode.Equals(taxCode) && (x.Id != id || id == null));
    }
    public async Task<bool> CheckEmailCode(string? email, Guid? id)
    {
        if (String.IsNullOrEmpty(email))
        {
            return false;
        }
        return await DbSet.AnyAsync(x => x.Email.Equals(email) && (x.Id != id || id == null));
    }
    public async Task<bool> CheckPhoneCode(string? phone, Guid? id)
    {
        if (String.IsNullOrEmpty(phone))
        {
            return false;
        }
        return await DbSet.AnyAsync(x => x.Phone.Equals(phone) && (x.Id != id || id == null));
    }
    public async Task<Customer> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<(IEnumerable<Customer>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => (EF.Functions.Contains(x.Name, $"{keyword.KeywordStandardized()}")
            || EF.Functions.Like(x.Name, $"%{keyword.KeywordStandardized()}%"))
            || x.Code.Contains(keyword)
            || x.Phone.Contains(keyword) || x.Email.Contains(keyword)
            );
        }
        foreach (var item in filter)
        {
            if (item.Key.Equals("status"))
            {
                query = query.Where(x => x.Status == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("type"))
            {
                query = query.Where(x => x.Type == int.Parse(item.Value + ""));
            }
            if (item.Key.Equals("customerGroupId"))
            {
                List<Guid> list = new String(item.Value + "").Split(',').Select(x => Guid.Parse(x)).ToList();
                var groupCustomerMappingList = await Db.CustomerGroupMapping.Where(x => list.Contains(x.CustomerGroupId)).ToListAsync();
                query = query.Where(x => groupCustomerMappingList.Select(y => y.CustomerId).Contains(x.Id));
            }
            if (item.Key.Equals("employeeId"))
            {
                query = query.Where(x => x.EmployeeId == new Guid(item.Value + ""));
            }
            if (item.Key.Equals("groupEmployeeId"))
            {
                query = query.Where(x => x.GroupEmployeeId == new Guid(item.Value + ""));
            }
        }
        var (filtered, totalCount) = query.Include(x => x.CustomerGroupMapping)
            .ThenInclude(x => x.CustomerGroup).ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }

    public async Task<IEnumerable<Customer>> GetListCbx(int? status, string? keyword)
    {
        var query = DbSet.AsQueryable();
        if (!String.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword) || EF.Functions.Like(x.Name, $"%{keyword}%"));
        }
        return await query.Take(100).ToListAsync();
    }
    public async Task<IEnumerable<Customer>> Filter(List<Guid?> listId)
    {
        var query = DbSet.AsQueryable();
        query = query.Where(c => listId.Contains(c.Id));
        return await query.ToListAsync();
    }

    public async Task<bool> AccountEmailExist(string email)
    {
        var query = await DbSet.AsQueryable().Where(x => x.AccountEmail.Equals(email)).AnyAsync();
        return query;
    }
    public void UpdateAccount(Customer t)
    {
        Db.Entry(t).Property(x => x.AccountId).IsModified = true;
        Db.Entry(t).Property(x => x.AccountEmail).IsModified = true;
        Db.Entry(t).Property(x => x.AccountEmailVerified).IsModified = true;
        Db.Entry(t).Property(x => x.AccountUsername).IsModified = true;
        Db.Entry(t).Property(x => x.AccountCreatedDate).IsModified = true;
        Db.Entry(t).Property(x => x.AccountPhone).IsModified = true;
        Db.Entry(t).Property(x => x.AccountPhoneVerified).IsModified = true;
    }
    public bool CheckUsing(Guid id)
    {
        return !(
            Db.Order.Any(x => x.CustomerId.Equals(id)) // đơn
            || Db.Quotation.Any(x => x.CustomerId.Equals(id))//báo giá
            || Db.Contract.Any(x => x.CustomerId.Equals(id))//hợp đồng
            || Db.ProductionOrder.Any(x => x.CustomerId.Equals(id)) //yc dự toán - ycsx
            || Db.RequestQuote.Any(x => x.CustomerId.Equals(id))//yc báo giá
            || Db.PaymentInvoice.Any(x => x.CustomerId.Equals(id))//ql thu chi
            || Db.ReturnOrder.Any(x => x.CustomerId.Equals(id))//trả hàng
            || Db.ExportWarehouse.Any(x => x.CustomerId.Equals(id))//Dn xuất
            );
    }
    public void UpdateFinance(Customer t)
    {
        Db.Entry(t).Property(x => x.PriceListId).IsModified = true;
        Db.Entry(t).Property(x => x.PriceListName).IsModified = true;
        Db.Entry(t).Property(x => x.CurrencyId).IsModified = true;
        Db.Entry(t).Property(x => x.Currency).IsModified = true;
        Db.Entry(t).Property(x => x.CurrencyName).IsModified = true;
        Db.Entry(t).Property(x => x.DebtLimit).IsModified = true;
    }

    public async Task<IEnumerable<Customer>> Filter(Guid accountId)
    {
        var query = DbSet.AsQueryable();

        query = query.Where(x => accountId == x.EmployeeId);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Customer>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("accountId"))
            {
                query = query.Where(x => x.EmployeeId == (Guid)item.Value);
            }
            if (item.Key.Equals("groupEmployeeId"))
            {
                query = query.Where(x => x.GroupEmployeeId == (Guid)item.Value);
            }
            if (item.Key.Equals("customerSourceId"))
            {
                query = query.Where(x => x.CustomerSourceId == (Guid)item.Value);
            }
            if (item.Key.Equals("businessSector"))
            {
                query = query.Where(x => x.BusinessSector == (string)item.Value);
            }
        }
        return await query.ToListAsync();
    }

    public async Task<IList<Customer>> GetByAccountIds(IList<Guid> accountIds)
    {
        return await DbSet.Where(x => x.AccountId.HasValue && accountIds.Contains(x.AccountId.Value)).ToListAsync();
    }

    public async Task<IEnumerable<string>> Filter(IEnumerable<string>? code)
    {
        var query = DbSet.Where(x => code.Contains(x.Code)).Select(x => x.Code);
        return await query.ToListAsync();
    }

    public void Add(IEnumerable<Customer> t)
    {
        DbSet.AddRange(t);
    }
}
