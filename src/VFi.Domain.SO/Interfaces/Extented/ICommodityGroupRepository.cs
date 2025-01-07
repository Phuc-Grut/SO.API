using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ICommodityGroupRepository : IRepository<CommodityGroup>
{
    Task<CommodityGroup> GetByCode(string code);
    Task<IEnumerable<CommodityGroup>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<CommodityGroup>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<CommodityGroup>, int)> Filter(string? keyword, IFopRequest request);
    void Update(IEnumerable<CommodityGroup> t);
    void Sort(IEnumerable<CommodityGroup> t);
}
