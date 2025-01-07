using VFi.Domain.SO.Models;

namespace VFi.Domain.SO.Interfaces;

public interface IProductRepository
{
    Task<string> GetByCategoryRootId(Guid id);
    Task<string> GetByListCategoryRootId(string listId);
    Task<Product> GetProductByCode(string code);
    Task<List<Product>> GetProductPaging(int pageNumber, int pageSize, string order, string filter);
}
