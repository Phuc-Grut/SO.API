using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using VFi.Domain.SO.Models;

namespace VFi.Domain.SO.Interfaces;

public interface IPIMRepository
{
    Task<IEnumerable<INVENTORY_DETAIL_BY_LISTID>> GetInventoryDetail(string? listId);
    Task<IEnumerable<SP_GET_PRODUCT_PRICE_BY_LISTID>> GetProductPrice(string? listId);
    Task<string> GetByCategoryRootId(Guid id);
    Task<string> GetByListCategoryRootId(string listId);
    Task<Product> GetProductByCode(string code);
    Task<List<Product>> GetProductByListCode(List<string> code);
    Task<List<Country>> GetCountryByName(List<string> code);
    Task<List<Province>> GetProvinceByName(List<string> code);
    Task<List<District>> GetDistrictByName(List<string> code);
    Task<List<Ward>> GetWardByName(List<string> code);

    Task<IEnumerable<Product>> GetProductPaging(int pageNumber, int pageSize, string order, string filter);
    Task<List<Unit>> GetUnitPaging(int pageNumber, int pageSize, string order, string filter);
    Task<List<Tax>> GetListBox();
}
