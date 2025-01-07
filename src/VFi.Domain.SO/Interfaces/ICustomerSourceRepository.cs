using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerSourceRepository : IRepository<CustomerSource>
{
    Task<CustomerSource> GetByCode(string code);
    Task<(IEnumerable<CustomerSource>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<CustomerSource>> GetListCbx(int? status);
    void Update(IEnumerable<CustomerSource> t);
    Task<IEnumerable<CustomerSource>> Filter(IEnumerable<string>? name);

}
