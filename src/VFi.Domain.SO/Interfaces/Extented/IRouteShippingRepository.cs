using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRouteShippingRepository : IRepository<RouteShipping>
{
    Task<RouteShipping> GetByCode(string code);
    Task<IEnumerable<RouteShipping>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<RouteShipping>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<RouteShipping>, int)> Filter(string? keyword, int? status, IFopRequest request);

}
