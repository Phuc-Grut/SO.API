using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;


public interface IWalletTypeRepository : IRepository<WalletType>
{
    Task<WalletType> GetByCode(string code);
    Task<IEnumerable<WalletType>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<WalletType>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<WalletType>, int)> Filter(string? keyword, IFopRequest request);

}
