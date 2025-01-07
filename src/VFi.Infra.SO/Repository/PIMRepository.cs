
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using Flurl.Http;
using Flurl.Http.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.Repository;

public class PIMRepository : IPIMRepository
{

    private readonly PIMApiContext _apiContext;
    private readonly MasterApiContext _apiMasterContext;

    private readonly string PATH_GET_INVENTORY_DETAIL_BY_LISTID = "/api/product/inventory-detail-by-list-id";
    private readonly string PATH_GET_PRODUCT_PRICE_BY_LISTID = "/api/product/get-price";
    private readonly string Path_Get_By_CategoryRootId = "/api/product/get-by-categoryrootid";
    private readonly string Path_Get_By_List_CategoryRootId = "/api/product/get-by-list-categoryrootid";
    private readonly string Path_Get_Product_By_Code = "/api/product/get-by-code";
    private readonly string Path_Get_Country_By_Name = "/api/country/get-by-list-name";
    private readonly string Path_Get_Province_By_Name = "/api/stateprovince/get-by-list-name";
    private readonly string Path_Get_District_By_Name = "/api/district/get-by-list-name";
    private readonly string Path_Get_Ward_By_Name = "/api/ward/get-by-list-name";
    private readonly string Path_Get_Product_By_List_Code = "/api/product/get-by-list-code";
    private readonly string Path_Get_By_List_Product = "/api/product/paging";
    private readonly string Path_Get_By_Unit = "/api/unit/paging";
    private readonly string Path_Get_By_Tax = "/api/taxcategory/get-listbox";
    private readonly IContextUser _context;
    public PIMRepository(PIMApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context, MasterApiContext apiMasterContext)
    {
        _apiContext = apiContext;
        _apiMasterContext = apiMasterContext;
        _context = context;
    }
    public async Task<Product> GetProductByCode(string code)
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var t = _apiContext.Client.Request(Path_Get_Product_By_Code + "/" + code)
                 .GetJsonAsync<Product>().Result;
            return await Task.FromResult(t);
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public async Task<IEnumerable<INVENTORY_DETAIL_BY_LISTID>> GetInventoryDetail(string? listId)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(PATH_GET_INVENTORY_DETAIL_BY_LISTID)
                             .SetQueryParam("$listProduct", listId).GetAsync().Result;
        var responseContent = await t.ResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<INVENTORY_DETAIL_BY_LISTID>>(responseContent);

    }


    public async Task<IEnumerable<SP_GET_PRODUCT_PRICE_BY_LISTID>> GetProductPrice(string? listId)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(PATH_GET_PRODUCT_PRICE_BY_LISTID)
                             .SetQueryParam("$listProduct", listId).GetAsync().Result;
        var responseContent = await t.ResponseMessage.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<SP_GET_PRODUCT_PRICE_BY_LISTID>>(responseContent);

    }
    public Task<string> GetByCategoryRootId(Guid id)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(Path_Get_By_CategoryRootId + "/" + id)
                             .GetStringAsync().Result;
        return Task.FromResult(t);
    }
    public Task<string> GetByListCategoryRootId(string id)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(Path_Get_By_List_CategoryRootId + "/" + id)
                             .GetStringAsync().Result;
        return Task.FromResult(t);
    }
    public async Task<List<Product>> GetProductByListCode(List<string> code)
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var requestPath = $"{Path_Get_Product_By_List_Code}";
            var t = await _apiContext.Client.Request(requestPath)
                 .PostJsonAsync(new { codes = string.Join(",", code) }).ReceiveJson<List<Product>>();
            return t;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
    public async Task<List<Unit>> GetUnitPaging(int pageNumber, int pageSize, string order, string filter)
    {
        _apiContext.Token = _context.GetToken();
        try
        {


            var encodedOrder = !string.IsNullOrEmpty(order) ? System.Net.WebUtility.UrlEncode(order) : null;
            var encodedFilter = !string.IsNullOrEmpty(filter) ? System.Net.WebUtility.UrlEncode(filter) : null;

            var queryString = $"?PageNumber={pageNumber}&PageSize={pageSize}";
            if (encodedOrder != null)
            {
                queryString += $"&Order={encodedOrder}";
            }
            if (encodedFilter != null)
            {
                queryString += $"&Filter={encodedFilter}";
            }

            var requestPath = $"{Path_Get_By_Unit}{queryString}";
            var jsonString = _apiContext.Client.Request(requestPath)
                      .GetStringAsync().Result;

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            var items = jsonObject.items;

            var unitsList = new List<Unit>();

            foreach (var item in items)
            {
                var unit = new Unit
                {
                    Id = Guid.Parse(item.id.ToString()),
                    Code = item.code.ToString(),
                    Name = item.name.ToString(),
                    GroupUnitCode = item.groupUnitCode.ToString(),


                };

                unitsList.Add(unit);
            }

            return unitsList;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Tax>> GetListBox()
    {
        var requestPath = $"{Path_Get_By_Tax}";
        var jsonString = _apiContext.Client.Request(requestPath)
                  .GetStringAsync().Result;
        var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);

        var taxList = new List<Tax>();
        foreach (var item in jsonObject)
        {
            var tax = new Tax
            {
                Id = Guid.Parse(item.value.ToString()),
                Name = item.label.ToString(),
                Code = item.key.ToString(),
                Rate = item.rate != null ? Convert.ToDouble(item.rate.ToString()) : (double?)null

            };
            taxList.Add(tax);
        }

        return taxList;
    }

    public async Task<IEnumerable<Product>> GetProductPaging(int pageNumber, int pageSize, string order, string filter)
    {
        _apiContext.Token = _context.GetToken();
        try
        {
            var encodedOrder = !string.IsNullOrEmpty(order) ? System.Net.WebUtility.UrlEncode(order) : null;
            var encodedFilter = !string.IsNullOrEmpty(filter) ? System.Net.WebUtility.UrlEncode(filter) : null;

            var queryString = $"?PageNumber={pageNumber}&PageSize={pageSize}";
            if (encodedOrder != null)
            {
                queryString += $"&Order={encodedOrder}";
            }
            if (encodedFilter != null)
            {
                queryString += $"&Filter={encodedFilter}";
            }

            var requestPath = $"{Path_Get_By_List_Product}{queryString}";
            var jsonString = _apiContext.Client.Request(requestPath)
                      .GetStringAsync().Result;

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            var items = jsonObject.items;

            var Product = new List<Product>();

            foreach (var item in items)
            {
                var product = new Product
                {
                    Id = Guid.Parse(item.id.ToString()),
                    Code = item.code.ToString(),
                    Name = item.name.ToString(),
                    UnitType = item.unitType.ToString(),
                    UnitCode = item.unitCode.ToString(),


                };

                Product.Add(product);
            }

            return Product;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Country>> GetCountryByName(List<string> code)
    {
        _apiMasterContext.Token = _context.GetToken();
        try
        {
            var requestPath = $"{Path_Get_Country_By_Name}";
            var t = await _apiMasterContext.Client.Request(requestPath)
                 .PostJsonAsync(new { listName = string.Join(",", code) }).ReceiveJson<List<Country>>();
            return t;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Province>> GetProvinceByName(List<string> code)
    {
        _apiMasterContext.Token = _context.GetToken();
        try
        {
            var requestPath = $"{Path_Get_Province_By_Name}";
            var t = await _apiMasterContext.Client.Request(requestPath)
                 .PostJsonAsync(new { listName = string.Join(",", code) }).ReceiveJson<List<Province>>();
            return t;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<District>> GetDistrictByName(List<string> code)
    {
        _apiMasterContext.Token = _context.GetToken();
        try
        {
            var requestPath = $"{Path_Get_District_By_Name}";
            var t = await _apiMasterContext.Client.Request(requestPath)
                 .PostJsonAsync(new { listName = string.Join(",", code) }).ReceiveJson<List<District>>();
            return t;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<List<Ward>> GetWardByName(List<string> code)
    {
        _apiMasterContext.Token = _context.GetToken();
        try
        {
            var requestPath = $"{Path_Get_Ward_By_Name}";
            var t = await _apiMasterContext.Client.Request(requestPath)
                 .PostJsonAsync(new { listName = string.Join(",", code) }).ReceiveJson<List<Ward>>();
            return t;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
