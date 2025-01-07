using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class LeadRepository : ILeadRepository
{
    protected readonly SqlCoreContext Db;
    protected readonly DbSet<Lead> DbSet;
    public IUnitOfWork UnitOfWork => Db;
    public LeadRepository(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        Db = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
        DbSet = Db.Set<Lead>();
    }

    public void Add(Lead Lead)
    {
        DbSet.Add(Lead);
    }

    public void Dispose()
    {
        Db.Dispose();
    }

    public async Task<IEnumerable<Lead>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<Lead> GetById(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Remove(Lead Lead)
    {
        DbSet.Remove(Lead);
    }

    public void Update(Lead Lead)
    {
        DbSet.Update(Lead);
    }

    public async Task<Lead> GetByCode(string code)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Code.Equals(code) && x.Code != "" && x.Code != null);
    }
    public async Task<(IEnumerable<Lead>, int)> Filter(string? keyword, int? status, int? convert, string? tags, string id, IFopRequest request)
    {
        var query = DbSet.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(x => x.Code.Contains(keyword)
            || x.Name.Contains(keyword)
            || (EF.Functions.Like(x.Name, $"%{keyword}%"))
            || EF.Functions.Like(x.Phone, $"%{keyword}%")
            || EF.Functions.Like(x.Email, $"%{keyword}%")
            || EF.Functions.Like(x.Group, $"%{keyword}%")
            || EF.Functions.Like(x.Source, $"%{keyword}%")
            || EF.Functions.Like(x.Employee, $"%{keyword}%")
            || EF.Functions.Like(x.Tags, $"%{keyword}%")
            );
        }
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        if (convert != null)
        {
            if (convert == 1)
            {
                query = query.Where(x => x.CustomerCode != null && x.CustomerCode != "");
            }
            else
            {
                query = query.Where(x => x.CustomerCode == null || x.CustomerCode == "");
            }
        }
        if (tags != null)
        {
            query = query.Where(x =>
            EF.Functions.Like(x.Tags, $"%{tags}%")
            );
        }
        if (id != null && id != "")
        {
            var list = (id + "").Split(",").ToList();
            query = query.Where(x => !list.Contains(x.Id.ToString()));
        }
        var (filtered, totalCount) = query.ApplyFop(request);
        return (await filtered.ToListAsync(), totalCount);
    }


    public async Task<IEnumerable<Lead>> GetListCbx(int? status)
    {
        var query = DbSet.AsQueryable();
        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }
        return await query.OrderBy(x => x.CreatedDate).ToListAsync();
    }
    public void Add(IEnumerable<Lead> t)
    {
        DbSet.AddRange(t);
    }

    public void Update(IEnumerable<Lead> t)
    {
        DbSet.UpdateRange(t);
    }

    public async Task<IEnumerable<Lead>> Filter(Dictionary<string, object> filter)
    {
        var query = DbSet.AsQueryable();

        foreach (var item in filter)
        {
            if (item.Key.Equals("groupEmployeeId"))
            {
                query = query.Where(x => x.GroupEmployeeId == (Guid)item.Value);
            }
            if (item.Key.Equals("id"))
            {
                var list = (item.Value + "").Split(",").ToList();
                query = query.Where(x => list.Contains(x.Id.ToString()));
            }
        }
        return await query.ToListAsync();
    }
}
