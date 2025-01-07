using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IWalletRepository : IRepository<Wallet>
{
    Task<Wallet> GetByCode(Guid accountId, string code);
    Task<IEnumerable<Wallet>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<Wallet>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<Wallet>, int)> Filter(string? keyword, IFopRequest request);
    void AddTransaction(WalletTransaction t);
}
