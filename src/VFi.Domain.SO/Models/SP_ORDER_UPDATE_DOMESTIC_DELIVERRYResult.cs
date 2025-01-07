using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Models
{
    public partial class SP_ORDER_UPDATE_DOMESTIC_DELIVERYResult
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime? DomesticDeliveryDate { get; set; }
        public int? Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
