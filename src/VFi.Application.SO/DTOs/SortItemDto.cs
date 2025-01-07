using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.SO.Models;

namespace VFi.Application.SO.DTOs;

public class SortItemDto
{
    public Guid Id { get; set; }
    public int SortOrder { get; set; }
}
