using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Events;

namespace VFi.Infra.SO.Repository.EventSourcing;

public class EventStoreSqlRepository : IEventStoreRepository
{
    private readonly EventStoreSqlContext _context;

    public EventStoreSqlRepository(EventStoreSqlContext context)
    {
        _context = context;
    }
    //private readonly EventStoreSqlContext _context;

    //public EventStoreSqlRepository(IServiceProvider serviceProvider)
    //{
    //    var scope = serviceProvider.CreateScope();
    //    _context = scope.ServiceProvider.GetRequiredService<EventStoreSqlContext>();
    //}

    public async Task<IList<StoredEvent>> All(Guid aggregateId)
    {
        return await (from e in _context.StoredEvent where e.AggregateId == aggregateId select e).ToListAsync();
    }

    public void Store(StoredEvent theEvent)
    {
        _context.StoredEvent.Add(theEvent);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
