using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPriceListCrossDetailRepository : IRepository<PriceListCrossDetail>
{
    Task<IEnumerable<PriceListCrossDetail>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<(IEnumerable<PriceListCrossDetail>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<PriceListCrossDetail>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<PriceListCrossDetail>, int)> Filter(string? keyword, IFopRequest request);
    void Update(IEnumerable<PriceListCrossDetail> details);
    void Remove(IEnumerable<PriceListCrossDetail> details);
    void Add(IEnumerable<PriceListCrossDetail> details);
}
