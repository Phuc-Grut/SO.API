using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IRequestQuoteRepository : IRepository<RequestQuote>
{
    Task<RequestQuote> GetByCode(string code);
    Task<(IEnumerable<RequestQuote>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<RequestQuote>> Filter(Dictionary<string, object> filter);
    Task<IEnumerable<RequestQuote>> GetListCbx(Dictionary<string, object> filter);
    void UpdateStatus(RequestQuote t);
}
