using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IReportTypeRepository : IRepository<ReportType>
{
    Task<ReportType> GetByCode(string code);
}
