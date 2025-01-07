using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPostOfficeRepository : IRepository<PostOffice>
{
    Task<PostOffice> GetByCode(string code);
    Task<IEnumerable<PostOffice>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<PostOffice>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<PostOffice>, int)> Filter(string? keyword, IFopRequest request);
    void Sort(IEnumerable<PostOffice> t);
}
