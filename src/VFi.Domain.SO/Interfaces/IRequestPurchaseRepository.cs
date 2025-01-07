using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRequestPurchaseRepository : IRepository<RequestPurchase>
{
    Task<RequestPurchase> GetByCode(string code);
    Task<(IEnumerable<RequestPurchase>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<RequestPurchase>> Filter(Dictionary<string, object> filter);
    Task<IEnumerable<RequestPurchase>> GetListCbx(int? status);
    void Purchase(RequestPurchase t);
    Task<RequestPurchase> CheckPO(string listPOCode);
    Task<RequestPurchase> GetRemoveOrderId(Guid requestId, Guid? orderId);
}
