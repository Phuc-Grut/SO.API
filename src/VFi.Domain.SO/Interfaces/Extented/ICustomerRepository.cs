using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public partial interface ICustomerRepository
{
    Task<IList<Customer>> GetByAccountIds(IList<Guid> accountIds);
    Task<Customer> GetFullById(Guid id);
    Task<IEnumerable<string>> Filter(IEnumerable<string>? code);
    void Add(IEnumerable<Customer> t);

}
