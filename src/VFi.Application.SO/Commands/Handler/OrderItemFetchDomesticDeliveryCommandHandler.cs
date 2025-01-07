using FluentValidation.Results;
using MediatR;
using VFi.Application.SO.Commands;
using VFi.Domain.SO.Enums;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands.Handler
{
    internal class OrderItemFetchDomesticDeliveryCommandHandler : CommandHandler, IRequestHandler<OrderItemFetchDomesticDeliveryCommand, ValidationResult>
    {
        private readonly IOrderRepository _repository;
        private readonly IEventRepository _eventRepository;



        public OrderItemFetchDomesticDeliveryCommandHandler(IOrderRepository repository, IEventRepository eventRepository)
        {
            _repository = repository;
            _eventRepository = eventRepository;
        }
        public void Dispose()
        {
            _repository.Dispose();
        }

        public async Task<ValidationResult> Handle(OrderItemFetchDomesticDeliveryCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!request.IsValid())
                return request.ValidationResult;
            var order = await _repository.GetById(request.Id);

            cancellationToken.ThrowIfCancellationRequested();

            if (order.Status.HasValue && order.Status.Value != (int)OrderStatus.Purchased)
            {
                return new ValidationResult(new List<ValidationFailure>{
                            new ValidationFailure(nameof(order.Status), "Order not purchased!")
            });
            }
            if (order.DomesticStatus == 10)
            {
                return new ValidationResult(new List<ValidationFailure>
                        {
                            new ValidationFailure(nameof(order.DomesticStatus), "Order completed!")
                        });
            }
            if (string.IsNullOrEmpty(order.DomesticTracking))
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(order.DomesticTracking),"Order without DomesticTracking!")
                });
            }
            var message = new Domain.SO.Events.OrderItemFetchDomesticDeliveryQueueEvent();
            message.Data = request.Data;
            message.Tenant = request.Tenant;
            message.Data_Zone = request.Data_Zone;
            message.Id = request.Id;
            message.CreatedDate = request.CreatedDate;
            message.DomesticCarrier = order.DomesticCarrier;
            message.DomesticTracking = order.DomesticTracking;
            message.CreatedBy = request.CreatedBy;
            message.CreatedName = request.CreatedName;

            cancellationToken.ThrowIfCancellationRequested();

            _ = await _eventRepository.OrderItemFetchDomesticDelivery(message);
            return new ValidationResult();
        }

    }
}
