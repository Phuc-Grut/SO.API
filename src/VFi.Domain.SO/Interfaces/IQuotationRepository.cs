using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IQuotationRepository : IRepository<Quotation>
{
    Task<Quotation> GetByCode(string code);
    Task<(IEnumerable<Quotation>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<Quotation>> Filter(Dictionary<string, object> filter);
    Task<IEnumerable<Quotation>> GetListBox(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
    void Approve(Quotation t);
    void UploadFile(Quotation t);
}
