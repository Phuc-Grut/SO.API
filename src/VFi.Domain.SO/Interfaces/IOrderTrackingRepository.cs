using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IOrderTrackingRepository : IRepository<OrderTracking>
{
    Task<IEnumerable<OrderTracking>> Filter(Dictionary<string, object> filter);
    void Update(IEnumerable<OrderTracking> items);
    void Add(IEnumerable<OrderTracking> items);
    void Remove(IEnumerable<OrderTracking> t);
}
