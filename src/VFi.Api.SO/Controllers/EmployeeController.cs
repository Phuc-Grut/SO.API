using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.DTOs;
using VFi.Application.SO.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IMediatorHandler mediator, IContextUser context, ILogger<EmployeeController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-list-cbx")]
    public async Task<IActionResult> Get([FromQuery] ComboBoxRequest request)
    {
        var result = await _mediator.Send(new EmployeeQueryComboBox(request.Status, request.GroupId));
        return Ok(result);
    }

    [HttpGet("get-by-list-id/{listId}")]
    public async Task<IActionResult> GetByListId(string listId)
    {
        var result = await _mediator.Send(new EmployeeQueryByListId(listId));
        return Ok(result);
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new EmployeeQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] EmployeePagingRequest request)
    {
        var query = new EmployeePagingQuery(request.Status, request.Keyword, request.GroupId, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddEmployeeRequest request)
    {
        var id = Guid.NewGuid();
        var Code = request.Code;
        int UsedStatus = 1;
        if (request.IsAuto == true)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var groups = request.Groups?.Select(x => new GroupEmployeeMappingDto()
        {
            EmployeeId = id,
            GroupEmployeeId = x.GroupEmployeeId,
            IsLeader = x.IsLeader
        }).ToList();

        var listAddress = request.ListAddress?.Select(x => new CustomerAddressDto()
        {
            Id = Guid.NewGuid(),
            EmployeeId = id,
            Name = x.Name,
            Country = x.Country,
            Province = x.Province,
            District = x.District,
            Ward = x.Ward,
            Address = x.Address,
            BillingDefault = x.BillingDefault,
            ShippingDefault = x.ShippingDefault,
            Email = x.Email,
            Phone = x.Phone,
            //ZipCode = x.ZipCode,
            Note = x.Note,
            SortOrder = x.SortOrder,
            Status = x.Status ?? 1
        }).ToList();

        var listContact = request.ListContact?.Select(x => new CustomerContactDto()
        {
            Id = Guid.NewGuid(),
            EmployeeId = id,
            Gender = x.Gender,
            Facebook = x.Facebook,
            Tags = x.Tags,
            Address = x.Address,
            Name = x.Name,
            JobTitle = x.JobTitle,
            Phone = x.Phone,
            Email = x.Email,
            Note = x.Note,
            Status = x.Status ?? 1,
            SortOrder = x.SortOrder
        }).ToList();

        var listBank = request.ListBank?.Select(x => new CustomerBankDto()
        {
            Id = Guid.NewGuid(),
            EmployeeId = id,
            Name = x.Name,
            BankBranch = x.BankBranch,
            AccountName = x.AccountName,
            AccountNumber = x.AccountNumber,
            Default = x.Default,
            Status = x.Status ?? 1,
            SortOrder = x.SortOrder
        }).ToList();

        var cmd = new EmployeeAddCommand(
          id,
          request.IsCustomer,
          Code,
          request.AccountId,
          request.AccountName,
          request.Email,
          request.Name,
          request.Image,
          request.Phone,
          request.Country,
          request.Province,
          request.District,
          request.Ward,
          request.Address,
          request.Gender,
          request.Year,
          request.Month,
          request.Day,
          request.TaxCode,
          request.GroupEmployee,
          request.Description,
          request.Status,
          groups,
          listAddress,
          listBank,
          listContact
      );
        var result = await _mediator.SendCommand(cmd);
        if (result.IsValid == false && request.IsAuto == true && result.Errors[0].ToString() == "Code already exists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                cmd.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                var res = await _mediator.SendCommand(cmd);
                if (res.IsValid == true)
                {
                    return Ok(res);
                }
            }
        }
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditEmployeeRequest request)
    {
        var employeeId = request.Id;
        EmployeeDto dataEmployee = await _mediator.Send(new EmployeeQueryById(employeeId));
        if (dataEmployee == null)
            return BadRequest(new ValidationResult("Employee not exists"));

        var groups = request.Groups?.Select(x => new GroupEmployeeMappingDto()
        {
            Id = (Guid)(x.Id == null ? Guid.NewGuid() : Guid.Parse(x.Id)),
            EmployeeId = employeeId,
            GroupEmployeeId = x.GroupEmployeeId,
            IsLeader = x.IsLeader
        }).ToList();

        var delete = request.Deletes?.Select(x => new DeleteGroupEmployeeMappingDto()
        {
            Id = (Guid)x.Id
        }).ToList();

        var listAddress = request.ListAddress?.Select(x => new CustomerAddressDto()
        {
            Id = (Guid)(string.IsNullOrEmpty(x.Id) ? Guid.NewGuid() : Guid.Parse(x.Id)),
            EmployeeId = x.EmployeeId,
            Name = x.Name,
            Country = x.Country,
            Province = x.Province,
            District = x.District,
            Ward = x.Ward,
            Address = x.Address,
            BillingDefault = x.BillingDefault,
            ShippingDefault = x.ShippingDefault,
            Email = x.Email,
            Phone = x.Phone,
            //ZipCode = x.ZipCode,
            Note = x.Note,
            SortOrder = x.SortOrder,
            Status = x.Status ?? 1
        }).ToList();

        var listContact = request.ListContact?.Select(x => new CustomerContactDto()
        {
            Id = (Guid)(string.IsNullOrEmpty(x.Id) ? Guid.NewGuid() : Guid.Parse(x.Id)),
            EmployeeId = x.EmployeeId,
            Gender = x.Gender,
            Facebook = x.Facebook,
            Tags = x.Tags,
            Address = x.Address,
            Name = x.Name,
            JobTitle = x.JobTitle,
            Phone = x.Phone,
            Email = x.Email,
            Note = x.Note,
            Status = x.Status ?? 1,
            SortOrder = x.SortOrder
        }).ToList();

        var listBank = request.ListBank?.Select(x => new CustomerBankDto()
        {
            Id = (Guid)((x.Id == null || x.Id == Guid.Empty) ? Guid.NewGuid() : x.Id),
            EmployeeId = x.EmployeeId,
            Name = x.Name,
            BankBranch = x.BankBranch,
            AccountName = x.AccountName,
            AccountNumber = x.AccountNumber,
            Default = x.Default,
            Status = x.Status ?? 1,
            SortOrder = x.SortOrder
        }).ToList();
        var cmd = new EmployeeEditCommand(
           employeeId,
           request.IsCustomer,
           request.Code,
           request.AccountId,
           request.AccountName,
           request.Email,
           request.Name,
           request.Image,
           request.Phone,
           request.Country,
           request.Province,
           request.District,
           request.Ward,
           request.Address,
           request.Gender,
           request.Year,
           request.Month,
           request.Day,
           request.TaxCode,
           request.GroupEmployee,
           request.Description,
           request.Status,
           groups,
           delete,
           listAddress,
           listBank,
           listContact
       );

        var result = await _mediator.SendCommand(cmd);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var EmployeeId = new Guid(id);
        if (await _mediator.Send(new EmployeeQueryById(EmployeeId)) == null)
            return BadRequest(new ValidationResult("Employee not exists"));

        var result = await _mediator.SendCommand(new EmployeeDeleteCommand(EmployeeId));

        return Ok(result);
    }
}
