using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using VFi.Domain.SO.Models;

namespace VFi.Domain.SO.Interfaces;

public interface IPORepository
{
    Task<ValidationResult> AddExt(PORequestPurchase t);
    Task<IEnumerable<QuantityPuchased>> GetQuantityPuchased(int? notCancel, string? purchaseRequestCode);
}
