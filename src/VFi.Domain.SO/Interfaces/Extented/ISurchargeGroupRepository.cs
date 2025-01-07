using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface ISurchargeGroupRepository : IRepository<SurchargeGroup>
{
    Task<SurchargeGroup> GetByCode(string code);
    Task<IEnumerable<SurchargeGroup>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
    Task<IEnumerable<SurchargeGroup>> GetListBox(Dictionary<string, object> filter);
    Task<(IEnumerable<SurchargeGroup>, int)> Filter(string? keyword, IFopRequest request);
    void Update(IEnumerable<SurchargeGroup> t);
    void Sort(IEnumerable<SurchargeGroup> t);
}
