using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Queries;

public class FopQuery : IFopQuery
{
    public string Filter { get; set; }

    public string Order { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
    public int? Status { get; set; }
    public string? Keyword { get; set; }
}
