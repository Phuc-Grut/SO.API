using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IStorePriceListRepository : IRepository<StorePriceList>
{
    Task<StorePriceList> GetByCode(string code);
    Task<IEnumerable<StorePriceList>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<StorePriceList>> GetListCbx(int? status);
    Task<(IEnumerable<StorePriceList>, int)> Filter(IFopRequest request);

    void Update(IEnumerable<StorePriceList> items);
    void Add(IEnumerable<StorePriceList> items);
    void Remove(IEnumerable<StorePriceList> t);
}
