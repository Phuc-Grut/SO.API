using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.SO.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<Expense> GetByCode(string code);
    Task<(IEnumerable<Expense>, int)> Filter(string? keyword, int? status, IFopRequest request);
    Task<IEnumerable<Expense>> GetListCbx(int? status);
    void Update(IEnumerable<Expense> t);
}
