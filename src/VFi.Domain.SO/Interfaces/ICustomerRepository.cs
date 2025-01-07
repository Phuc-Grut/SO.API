using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public partial interface ICustomerRepository : IRepository<Customer>
{
    Task<Boolean> CheckTaxCode(string? taxCode, Guid? id);
    Task<Boolean> CheckEmailCode(string? email, Guid? id);
    Task<Boolean> CheckPhoneCode(string? phone, Guid? id);
    Task<Customer> GetByCode(string code);
    Task<(IEnumerable<Customer>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    Task<IEnumerable<Customer>> GetListCbx(int? status, string? keyword);
    Task<IEnumerable<Customer>> Filter(List<Guid?> listId);
    Task<IEnumerable<Customer>> Filter(Dictionary<string, object> filter);
    Task<bool> AccountEmailExist(string email);
    Task<Customer> GetByAccountId(Guid id);
    Task<Customer> GetByAccountEmail(string email);
    Task<Customer> GetByAccountUsername(string username);
    void UpdateAccount(Customer t);
    Task<Guid> GetIdByAccountId(Guid accountId);
    bool CheckUsing(Guid id);
    void UpdateFinance(Customer t);
}
