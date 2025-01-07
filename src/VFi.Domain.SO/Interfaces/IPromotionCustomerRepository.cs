using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IPromotionCustomerRepository : IRepository<PromotionCustomer>
{
    Task<IEnumerable<PromotionCustomer>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<PromotionCustomer>> GetListListBox(Dictionary<string, object> filter);
    Task<IEnumerable<PromotionCustomer>> Filter(Guid id);
    void Add(IEnumerable<PromotionCustomer> items);
    void Remove(IEnumerable<PromotionCustomer> t);
}
