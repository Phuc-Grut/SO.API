using Microsoft.AspNetCore.Mvc;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPriceListRepository : IRepository<PriceList>
{
    bool CheckByCode(string? code, Guid? id);
    bool CheckUsing(Guid id);
    Task<(IEnumerable<PriceList>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<PriceList>> GetListCbx(Dictionary<string, object> filter);
    void Update(IEnumerable<PriceList> item);
}
