using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using VFi.Domain.SO.Models;

namespace VFi.Domain.SO.Interfaces;

public interface IWMSRepository
{
    Task<ValidationResult> FulfillmentRequestAddExt(FulfillmentRequestAddExt t);
    Task<ValidationResult> FulfillmentRequestAddExtEndpoint(FulfillmentRequestAddExt t);
}
