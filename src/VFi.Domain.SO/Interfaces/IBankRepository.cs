using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IBankRepository : IRepository<Bank>
{
    Task<Bank> GetByCode(string code);
    Task<(IEnumerable<Bank>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<Bank>> GetListCbx(int? status);
    void Update(IEnumerable<Bank> t);

    bool CheckUsing(Guid id);
}
