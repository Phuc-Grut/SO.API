using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ICustomerPriceListCrossRepository : IRepository<CustomerPriceListCross>
{
    void Add(IEnumerable<CustomerPriceListCross> t);
    void Remove(IEnumerable<CustomerPriceListCross> t);
}
