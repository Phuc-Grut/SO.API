using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IServiceAddRepository : IRepository<ServiceAdd>
{
    Task<Boolean> CheckExist(string? code, Guid? id);
    Task<IEnumerable<ServiceAdd>> Filter(string? keyword, int? status, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, int? status);
    Task<IEnumerable<ServiceAdd>> GetListListBox(string? keyword, int? status);
    Task<IEnumerable<ServiceAdd>> Filter(string keyword, Dictionary<string, object> filter);
    void Update(IEnumerable<ServiceAdd> serviceAdds);
    Task<Boolean> CheckExistById(Guid id);
}
