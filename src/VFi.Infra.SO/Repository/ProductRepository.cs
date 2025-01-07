using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.Repository;

public class ProductRepository : IProductRepository
{

    private readonly PIMApiContext _apiContext;
    private readonly string Path_Get_By_CategoryRootId = "/api/product/get-by-categoryrootid";
    private readonly string Path_Get_By_List_CategoryRootId = "/api/product/get-by-list-categoryrootid";
    private readonly string Path_Get_Product_By_Code = "/api/product/get-by-code";
    private readonly string Path_Get_By_List_Product = "/api/product/paging";
    private readonly IContextUser _context;
    public ProductRepository(PIMApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
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
    public async Task<List<Product>> GetProductPaging(int pageNumber, int pageSize, string order, string filter)
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

}
