using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderExpressDetailRepository : IRepository<OrderExpressDetail>
{
    Task<OrderExpressDetail> GetByCode(string code);
    Task<IEnumerable<OrderExpressDetail>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<OrderExpressDetail>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<OrderExpressDetail>, int)> Filter(string? keyword, IFopRequest request);
    void Add(IEnumerable<OrderExpressDetail> items);
    void Update(IEnumerable<OrderExpressDetail> items);
    void Remove(IEnumerable<OrderExpressDetail> items);
}
