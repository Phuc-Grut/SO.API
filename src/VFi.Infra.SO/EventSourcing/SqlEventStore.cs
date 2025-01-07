using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VFi.Infra.SO.Repository.EventSourcing;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Events;
using VFi.NetDevPack.Messaging;

namespace VFi.Infra.SO.EventSourcing;

public class SqlEventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IContextUser _user;

    public SqlEventStore(IEventStoreRepository eventStoreRepository, IServiceProvider serviceProvider)
    {
        _eventStoreRepository = eventStoreRepository;
        var scope = serviceProvider.CreateScope();
        _user = scope.ServiceProvider.GetRequiredService<IContextUser>();
    }
    public void Save<T>(T theEvent) where T : Event
    {
        // Using Newtonsoft.Json because System.Text.Json
        // is a sad joke to be considered "Done"

        // The System.Text don't know how serialize a
        // object with inherited properties, I said is sad...
        // Yes! I tried: options = new JsonSerializerOptions { WriteIndented = true };

        var serializedData = JsonConvert.SerializeObject(theEvent);

        var storedEvent = new StoredEvent(
           theEvent,
           serializedData,
           _user.GetUserId().ToString() ?? _user.GetUserEmail());

        _eventStoreRepository.Store(storedEvent);
    }
}
