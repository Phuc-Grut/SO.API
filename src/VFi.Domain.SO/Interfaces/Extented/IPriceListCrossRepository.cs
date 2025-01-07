using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPriceListCrossRepository : IRepository<PriceListCross>
{
    Task<PriceListCross> GetByCode(string code);
    Task<PriceListCross> GetDefaultByRouterShipping(Guid routerShipping);
    Task<IEnumerable<PriceListCross>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<PriceListCross>> GetListBox(string keyword, Dictionary<string, object> filter);
    Task<(IEnumerable<PriceListCross>, int)> Filter(string? keyword, IFopRequest request);
    void Sort(IEnumerable<PriceListCross> t);


}
