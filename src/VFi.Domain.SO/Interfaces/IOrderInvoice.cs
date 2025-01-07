using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderInvoiceRepository : IRepository<OrderInvoice>
{
    void Update(IEnumerable<OrderInvoice> items);
    void Add(IEnumerable<OrderInvoice> items);
    void Remove(IEnumerable<OrderInvoice> items);
}
