using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IStoreRepository : IRepository<Store>
{
    Task<Boolean> CheckExist(string? code, Guid? id);
    Task<IEnumerable<Store>> GetListListBox(int? status);
    Task<(IEnumerable<Store>, int)> Filter(Dictionary<string, object> filter, IFopRequest request);
    void Update(IEnumerable<Store> stores);
    Task<Boolean> CheckExistById(Guid id);
}
