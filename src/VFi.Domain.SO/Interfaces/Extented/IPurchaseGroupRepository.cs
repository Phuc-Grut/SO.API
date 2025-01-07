using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPurchaseGroupRepository : IRepository<PurchaseGroup>
{
    Task<PurchaseGroup> GetByCode(string code);
    Task<IEnumerable<PurchaseGroup>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<PurchaseGroup>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<PurchaseGroup>, int)> Filter(string? keyword, IFopRequest request);
    void Update(IEnumerable<PurchaseGroup> t);
    void Sort(IEnumerable<PurchaseGroup> t);
}
