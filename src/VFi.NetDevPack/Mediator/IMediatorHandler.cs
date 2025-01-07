using FluentValidation.Results;
using VFi.NetDevPack.Messaging;
using VFi.NetDevPack.Queries;

namespace VFi.NetDevPack.Mediator;

public interface IMediatorHandler
{
    Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default(CancellationToken));
    Task PublishEvent<T>(T @event) where T : Event;
    Task PublishEvent<T>(T @event, PublishStrategy strategy, CancellationToken cancellationToken) where T : Event;
    Task<ValidationResult> SendCommand<T>(T command, CancellationToken cancellationToken = default) where T : Command;
    Task<ValidationResult> SendCommandResult<T>(T command, CancellationToken cancellationToken = default) where T : CommandResult;
    //Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
    // TResponse Send<TQuery>(TQuery query) where TQuery : IQuery<TResponse>;
}
