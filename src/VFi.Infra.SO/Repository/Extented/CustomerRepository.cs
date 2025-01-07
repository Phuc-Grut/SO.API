using System.Security.Cryptography.X509Certificates;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Domain.SO.Interfaces;

public partial class CustomerRepository
{

    public async Task<Customer> GetFullById(Guid id)
    {
        return await DbSet.Include(x => x.CustomerPriceListCross).Include(x => x.CustomerAddress).Include(x => x.CustomerContact).Include(x => x.CustomerBank).FirstOrDefaultAsync(x => x.Id == id);
    }
}
