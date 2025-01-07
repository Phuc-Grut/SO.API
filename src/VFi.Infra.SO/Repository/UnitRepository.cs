using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;

namespace VFi.Infra.SO.Repository;


public class UnitRepository : IUnitRepository
{
    private readonly IContextUser _context;
    private readonly PIMApiContext _apiContext;
    private readonly string Path_Get_By_Unit = "/api/unit/paging";
    public UnitRepository(PIMApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
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
}
