using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IQuotationTermRepository : IRepository<QuotationTerm>
{
    Task<QuotationTerm> GetByCode(string code);
    Task<(IEnumerable<QuotationTerm>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<QuotationTerm>> GetListCbx(int? status);
    void Update(IEnumerable<QuotationTerm> t);
}
