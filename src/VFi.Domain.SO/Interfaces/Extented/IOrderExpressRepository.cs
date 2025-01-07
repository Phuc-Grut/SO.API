using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderExpressRepository : IRepository<OrderExpress>
{
    Task<OrderExpress> GetByCode(string code);
    Task<IEnumerable<OrderExpress>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<OrderExpress>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<OrderExpress>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<(IEnumerable<OrderExpress>, int)> Filter(string? keyword, Guid customerId, int? status, IFopRequest request);
    Task<OrderExpress?> GetByCode(Guid customerId, string code);
    void Approve(OrderExpress t);
}
