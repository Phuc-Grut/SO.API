using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Domain
{
    public class POCreatePurchaseResponse
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public int status { get; set; }
    }
}
