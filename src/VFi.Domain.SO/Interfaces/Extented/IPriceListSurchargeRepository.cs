using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPriceListSurchargeRepository : IRepository<PriceListSurcharge>
{
    Task<IEnumerable<PriceListSurcharge>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<PriceListSurcharge>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<PriceListSurcharge>, int)> Filter(string? keyword, IFopRequest request);
    void Add(IEnumerable<PriceListSurcharge> details);
    void Sort(IEnumerable<PriceListSurcharge> t);

}
