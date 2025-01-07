using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderProductController : ControllerBase
{
    private readonly IMediatorHandler _mediator;

    public OrderProductController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("validate-excel")]
    public async Task<IActionResult> ValidateExcel([FromForm] ImportExcelOrderProductRequest request)
    {
        List<ValidateField> listField = new List<ValidateField>()
        {
            new ValidateField(){Field="productName", IndexColumn= request.ProductName},

            new ValidateField(){Field="unitCode", IndexColumn= request.UnitCode},
            new ValidateField(){Field="productCode", IndexColumn= request.ProductCode},
            new ValidateField(){Field="unitName", IndexColumn= request.UnitName},
            new ValidateField(){Field="productPrice", IndexColumn= request.ProductPrice},
            new ValidateField(){Field="tranferRate", IndexColumn= request.ProductName},
            new ValidateField(){Field="productId", IndexColumn= request.ProductName},
            new ValidateField(){Field="productImage", IndexColumn= request.ProductName},
            new ValidateField(){Field="productRate", IndexColumn= request.ProductName},
            new ValidateField(){Field="allocationRate", IndexColumn= request.ProductName},
            new ValidateField(){Field="discountAmountDistribution", IndexColumn= request.ProductName},
            new ValidateField(){Field="amountNoTaxQD", IndexColumn= request.ProductName},
            new ValidateField(){Field="amountNoDiscountQD", IndexColumn= request.ProductName},
            new ValidateField(){Field="amountNoTax", IndexColumn= request.ProductName},
            new ValidateField(){Field="discountPercent", IndexColumn= request.ProductName},
            new ValidateField(){Field="discountType", IndexColumn= request.ProductName},
            new ValidateField(){Field="totalAmountDiscountQD", IndexColumn= request.ProductName},
            new ValidateField(){Field="totalAmountTaxQD", IndexColumn= request.ProductName},
            new ValidateField(){Field="tax", IndexColumn= request.ProductName},
            new ValidateField(){Field="amountDiscount", IndexColumn= request.ProductName},
            new ValidateField(){Field="amountNoDiscount", IndexColumn= request.ProductName},
            new ValidateField(){Field="amountTax", IndexColumn= request.ProductName},
            new ValidateField(){Field="deliveryDate", IndexColumn= request.ProductName},
            new ValidateField(){Field="quantity", IndexColumn= request.ProductName},
            new ValidateField(){Field="totalAmountDiscount", IndexColumn= request.ProductName},
            new ValidateField(){Field="totalAmountTax", IndexColumn= request.ProductName},
            new ValidateField(){Field="unitPrice", IndexColumn= request.ProductName},
        };

        //var result = await _mediator.Send();

        return Ok();
    }
}
