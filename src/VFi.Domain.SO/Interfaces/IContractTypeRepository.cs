using MediatR;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IContractTypeRepository : IRepository<ContractType>
{
    Task<ContractType> GetByCode(string code);
    Task<IEnumerable<ContractType>> GetListCbx(int? status);
    Task<(IEnumerable<ContractType>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);

}
