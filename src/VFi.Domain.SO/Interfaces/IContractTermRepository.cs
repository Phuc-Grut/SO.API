using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IContractTermRepository : IRepository<ContractTerm>
{
    Task<ContractTerm> GetByCode(string code);
    Task<(IEnumerable<ContractTerm>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<ContractTerm>> GetListCbx(int? status);
    void Update(IEnumerable<ContractTerm> t);
}
