using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Application.SO.Commands.Validations;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands
{
    public class OrderItemFetchDomesticDeliveryCommand : Command
    {
        public Guid Id { get; set; }
        public string? DomesticCarrier { get; set; }
        public string? DomesticTracking { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? CreatedName { get; set; }
        public string? Data { get; set; }
        public string? Data_Zone { get; set; }
        public string? Tenant { get; set; }
        public bool IsValid()
        {
            ValidationResult = new OrderItemFetchDomesticDeliveryValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
