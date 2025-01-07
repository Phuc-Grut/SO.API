using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IBusinessRepository : IRepository<Business>
{
    Task<Business> GetByCode(string code);
    Task<(IEnumerable<Business>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<Business>> GetListCbx(int? status);
    Task<IEnumerable<Business>> GetById(IEnumerable<CustomerBusinessMapping> mapping);
    void Update(IEnumerable<Business> t);
    Task<IEnumerable<Business>> Filter(IEnumerable<string>? name);

}
