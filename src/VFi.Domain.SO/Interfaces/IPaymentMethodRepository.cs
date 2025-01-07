using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    Task<PaymentMethod> GetByCode(string code);
    Task<(IEnumerable<PaymentMethod>, int)> Filter(string? keyword, int? status, IFopRequest request);

    Task<IEnumerable<PaymentMethod>> GetListCbx(int? status);
}
