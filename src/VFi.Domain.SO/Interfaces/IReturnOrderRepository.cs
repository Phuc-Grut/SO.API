using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IReturnOrderRepository : IRepository<ReturnOrder>
{
    Task<ReturnOrder> GetByCode(string code);
    Task<(IEnumerable<ReturnOrder>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<ReturnOrder>> Filter(Dictionary<string, object> filter);
    Task<IEnumerable<ReturnOrder>> GetListCbx(int? status);
    void UploadFile(ReturnOrder t);
}
