using System.Linq;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public class ContractRepository : IContractRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Contract> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public ContractRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Contract>();
    }

    public void Add(Contract Contract)
    {
        DbSet.Add(Contract);
        //DbSet.Add(Contact);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Contract>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Contract> GetById(Guid id)
    {
        return await DbSet.Include(x => x.ContractType).Include(x => x.OrderProduct).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Remove(Contract Contract)
    {
        DbSet.Remove(Contract);
    }

    public void Update(Contract Contract)
    {
        DbSet.Update(Contract);
    }

    public async Task<Contract> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code));
    }
    public async Task<IEnumerable<Contract>> GetListBox(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex)
    {
        var query = DbSet.Include(x => x.Quotation).Include(x => x.OrderProduct).AsQueryable();
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
                query = query.Where(x => x.StartDate <= date && x.EndDate >= date);
            }
            if (item.Key.Equals("customerId"))
            {
                query = query.Where(x => x.CustomerId == new Guid(item.Value.ToString()));
            }
            if (item.Key.Equals("isOrder") && item.Value == "0")
            {
                var listOrder = Db.Order.Where(x => x.ContractId != null).Select(y => y.ContractId).ToList();
                query = query.Where(item => item.OrderProduct.Where(x => x.Quantity - (Db.OrderProduct.Where(y => y.Order.ContractId == item.Id && y.Order.Status != 9 && y.ProductId == x.ProductId).Sum(y => y.Quantity) ?? 0) > 0).Any() || (!listOrder.Contains(item.Id)));
            }
            if (item.Key.Equals("fromDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value) : DateTime.Now;
                query = query.Where(x => x.StartDate >= date);
            }
            if (item.Key.Equals("toDate"))
            {
                var date = item.Value != "" ? Convert.ToDateTime(item.Value).Add(new TimeSpan(23, 59, 59)) : DateTime.Now;
                query = query.Where(x => x.EndDate <= date);
            }
        }
        return await query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
    }

    public async Task<(IEnumerable<Contract>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request)
    {
        var query = DbSet.Include(x => x.OrderProduct).AsQueryable();
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
    public void Approve(Contract t)
    {
        Db.Entry(t).Property(x => x.Status).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveDate).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveBy).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveByName).IsModified = true;
        Db.Entry(t).Property(x => x.ApproveComment).IsModified = true;
    }
    public void UploadFile(Contract t)
    {
        Db.Entry(t).Property(x => x.File).IsModified = true;
    }

    public async Task<IEnumerable<Contract>> Filter(Guid accountId)
    {
        var query = DbSet.AsQueryable();

        query = query.Where(x => accountId == x.AccountId);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Contract>> Filter(Dictionary<string, object> filter)
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
            if (item.Key.Equals("contractTermId"))
            {
                query = query.Where(x => x.ContractTermId == (Guid)item.Value);
            }
            if (item.Key.Equals("contractTypeId"))
            {
                query = query.Where(x => x.ContractTypeId == (Guid)item.Value);
            }
        }
        return await query.ToListAsync();
    }
}
