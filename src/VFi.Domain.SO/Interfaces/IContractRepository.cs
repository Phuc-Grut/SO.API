using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IContractRepository : IRepository<Contract>
{
    Task<Contract> GetByCode(string code);
    Task<IEnumerable<Contract>> GetListBox(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<(IEnumerable<Contract>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<Contract>> Filter(Dictionary<string, object> filter);
    void Approve(Contract t);
    void UploadFile(Contract t);
}

