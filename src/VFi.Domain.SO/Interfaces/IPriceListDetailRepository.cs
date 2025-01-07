using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IPriceListDetailRepository : IRepository<PriceListDetail>
{
    Task<IEnumerable<PriceListDetail>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    Task<IEnumerable<PriceListDetail>> Filter(Guid id);
    void Update(IEnumerable<PriceListDetail> items);
    void Add(IEnumerable<PriceListDetail> items);
    void Remove(IEnumerable<PriceListDetail> t);
}
