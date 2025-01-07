using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs
{
    public class PreviewRecalculatedPriceDto
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
