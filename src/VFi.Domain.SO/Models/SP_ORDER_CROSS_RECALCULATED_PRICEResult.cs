using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.SO.Models
{
    public partial class SP_ORDER_CROSS_RECALCULATED_PRICEResult
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public decimal? TotalAmountTax { get; set; }
        public decimal? DomesticShipping { get; set; }
        public decimal? OldPaymentPrice { get; set; }
        public decimal? NewPaymentPrice { get; set; }
    }
}
