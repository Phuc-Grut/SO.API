using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class ExportTemplateRepository : IExportTemplateRepository
{
    private readonly MasterApiContext _apiContext;
    private readonly string PATH_GET_FILE = "/api/exporttemplate/get-file";
    private readonly IContextUser _context;

    public ExportTemplateRepository(MasterApiContext apiContext, IFlurlClientFactory flurlClientFac, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }

    public Task<byte[]> GetTemplate(string code)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request($"{PATH_GET_FILE}/{code}")
                            .GetBytesAsync();
        return t;
    }

    private class ComboboxMaster
    {
        public string? Key { get; set; }
        public string? Label { get; set; }
        public Guid Value { get; set; }
    }
}
