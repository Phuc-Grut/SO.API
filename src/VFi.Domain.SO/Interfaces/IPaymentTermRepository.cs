using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPaymentTermRepository : IRepository<PaymentTerm>
{
    Task<PaymentTerm> GetByCode(string code);
    Task<(IEnumerable<PaymentTerm>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<PaymentTerm>> GetListCbx(int? status);
}
