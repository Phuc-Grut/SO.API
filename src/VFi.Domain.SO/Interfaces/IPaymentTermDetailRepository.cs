using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IPaymentTermDetailRepository : IRepository<PaymentTermDetail>
{
    Task<IEnumerable<PaymentTermDetail>> Filter(Guid id, int pagesize, int pageindex);
    Task<int> FilterCount(Guid id);
    Task<IEnumerable<PaymentTermDetail>> GetAll(Guid id);
    void Add(IEnumerable<PaymentTermDetail> items);
    void Update(IEnumerable<PaymentTermDetail> items);
    void Remove(IEnumerable<PaymentTermDetail> items);
}
