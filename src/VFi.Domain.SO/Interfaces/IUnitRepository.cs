using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.SO.Interfaces;

public interface IUnitRepository
{
    Task<List<Unit>> GetUnitPaging(int pageNumber, int pageSize, string order, string filter);

}
