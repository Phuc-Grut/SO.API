using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Interfaces;

namespace VFi.Application.SO.Commands.Validations
{
    public abstract class OrderItemFetchDomesticDeliveryValidation<T> : AbstractValidator<T> where T : OrderItemFetchDomesticDeliveryCommand
    {
        
        public OrderItemFetchDomesticDeliveryValidation()
        {
        }
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty);
        }
             
    }
    public class OrderItemFetchDomesticDeliveryValidation : OrderItemFetchDomesticDeliveryValidation<OrderItemFetchDomesticDeliveryCommand>
    {
        public OrderItemFetchDomesticDeliveryValidation() 
        {
            ValidateId();
        }
    }


}
