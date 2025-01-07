using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IPromotionCustomerGroupRepository : IRepository<PromotionCustomerGroup>
{
    Task<IEnumerable<PromotionCustomerGroup>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<PromotionCustomerGroup>> GetListListBox(Dictionary<string, object> filter);
    Task<IEnumerable<PromotionCustomerGroup>> Filter(Guid id);
    void Add(IEnumerable<PromotionCustomerGroup> items);
    void Remove(IEnumerable<PromotionCustomerGroup> t);
}
