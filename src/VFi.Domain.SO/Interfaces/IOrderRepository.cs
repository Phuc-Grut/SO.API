using VFi.Domain.SO.Models;
using VFi.Domain.SO.Models.Extented.OrderCross;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetByCode(string code);
    Task<(IEnumerable<Order>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<Order>> Filter(Dictionary<string, object> filter);
    Task<IEnumerable<Order>> GetListListBox(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    void Approve(Order t);
    Task<IEnumerable<Order>> GetReference(Dictionary<string, object> filter);
    Task<IEnumerable<Order>> GetDataReport(Dictionary<string, object> filter);
    void UploadFile(Order t);
    Task<Order?> GetByCode(Guid customerId, string code);
    Task<IEnumerable<Order>> GetByCode(Guid customerId, IList<string> code);
    bool CheckUsing(Guid id);


    Task<IEnumerable<OrderInformation>> GetInfoByCodes(Guid customerId, IList<string> code);
    Task<IEnumerable<OrderInformation>> GetAuctionUnpaid(Guid customerId);

    Task<IEnumerable<Order>> GetAuctionWithoutDomesticTracking(DateTime fromDate, DateTime toDate, int? top = 50);
    Task<IEnumerable<Order>> GetOrderWithDomesticTracking(DateTime fromDate, DateTime toDate, int? top = 50);
    Task<IEnumerable<Order>> GetMercariWithoutDomesticTracking(DateTime fromDate, DateTime toDate, int? top = 50);
    Task<(IEnumerable<Order>, int)> FilterCustom(string? keyword, List<string>? codes, List<string>? tracking, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<Order>> GetByIds(IList<Guid> ids);
    Task<IEnumerable<Order>> GetByIdsWithCustomerId(IList<Guid> ids);
    Task<bool> ExistId(Guid id);
    Task<IEnumerable<Order>> GetByCodes(IList<string> codes);
    Task<Order> GetByIdWithCustomerAndProducts(Guid id);
}

