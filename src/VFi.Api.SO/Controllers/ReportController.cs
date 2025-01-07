using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.SO.ViewModels;
using VFi.Application.SO.Commands;
using VFi.Application.SO.Events;
using VFi.Application.SO.Queries;
using VFi.Domain.SO.Interfaces;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.SO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ReportController> _logger;
    private readonly IReportTypeRepository _reportTypeRepository;

    public ReportController(
        IContextUser context,
        IMediatorHandler mediator,
        ILogger<ReportController> logger,
        IReportTypeRepository reportTypeRepository
        )
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
        _reportTypeRepository = reportTypeRepository;
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] FopPagingRequest request)
    {
        var query = new ReportPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = new ReportQueryById(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddReportRequest request, CancellationToken cancellationToken)
    {
        var reportType = await _reportTypeRepository.GetByCode(request.ReportTypeCode);
        var id = Guid.NewGuid();
        var command = new ReportAddCommand(
          id,
          request.Name,
          reportType?.Id,
          request.ReportTypeCode,
          request.FromDate,
          request.ToDate,
          request.CustomerId,
          request.CustomerCode,
          request.CustomerName,
          request.EmployeeId,
          request.EmployeeCode,
          request.EmployeeName,
          request.CategoryRootId,
          request.CategoryRootName,
          request.ProductId,
          request.ProductCode,
          request.ProductName,
          request.CustomerGroupId,
          request.CustomerGroupCode,
          request.CustomerGroupName,
          request.CurrencyCode,
          0,
          request.LoadDate
      );
        var result = await _mediator.SendCommand(command);
        if (!result.IsValid)
        {
            return Ok(result);
        }
        return LoadEvent(new LoadDataReportRequest()
        {
            ReportId = id,
            CategoryRootId = request.CategoryRootId,
            CustomerCode = request.CustomerCode,
            CustomerGroupId = request.CustomerGroupId,
            EmployeeId = request.EmployeeId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            ProductCode = request.ProductCode,
            ReportType = request.ReportTypeCode
        }, new FluentValidation.Results.ValidationResult(), cancellationToken);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditReportRequest request, CancellationToken cancellationToken)
    {
        var command = new ReportEditCommand(
          request.Id,
          request.Name,
          request.ReportTypeId,
          request.ReportTypeCode,
          request.FromDate,
          request.ToDate,
          request.CustomerId,
          request.CustomerCode,
          request.CustomerName,
          request.EmployeeId,
          request.EmployeeCode,
          request.EmployeeName,
          request.CategoryRootId,
          request.CategoryRootName,
          request.ProductId,
          request.ProductCode,
          request.ProductName,
          request.CustomerGroupId,
          request.CustomerGroupCode,
          request.CustomerGroupName,
          request.CurrencyCode,
          0,
          request.LoadDate
       );

        var result = await _mediator.SendCommand(command);

        if (!result.IsValid)
        {
            return Ok(result);
        }
        return LoadEvent(new LoadDataReportRequest()
        {
            ReportId = request.Id,
            CategoryRootId = request.CategoryRootId,
            CustomerCode = request.CustomerCode,
            CustomerGroupId = request.CustomerGroupId,
            EmployeeId = request.EmployeeId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            ProductCode = request.ProductCode,
            ReportType = request.ReportTypeCode
        }, new FluentValidation.Results.ValidationResult(), cancellationToken);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ReportId = new Guid(id);
        if (await _mediator.Send(new ReportQueryById(ReportId)) == null)
            return BadRequest(new ValidationResult("Report not exists"));

        var result = await _mediator.SendCommand(new ReportDeleteCommand(ReportId));

        return Ok(result);
    }

    [HttpGet("paging-detail")]
    public async Task<IActionResult> RP_Detail_Paging([FromQuery] ReportPagingDetailRequest request)
    {
        if (request.TypeReport == "SCTBH")
        {
            var query = new RptSoChiTietBanHangPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THTMH")
        {
            var query = new RptTongHopBanTheoMatHangPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THTKH")
        {
            var query = new RptTongHopBanTheoKhachHangPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THDHB")
        {
            var query = new RptTinhHinhThucHienDonBanHangFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THHD")
        {
            var query = new RptTinhHinhThucHienHopDongBanFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THTNV")
        {
            var query = new RptTongHopBanTheoNhanVienPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THTNKH")
        {
            var query = new RptTongHopBanTheoNhomKhachHangPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THTKHVDH")
        {
            var query = new RptTongHopBanHangTheoKhachHangVaDonBanHangPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else if (request.TypeReport == "THTMHVKH")
        {
            var query = new RptTongHopBanHangTheoMatHangVaKhachHangPagingFilterQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        else
        {
            return Ok();
        }
    }

    [HttpPost("load-data-event")]
    public async Task<IActionResult> LoadDataEventAsync([FromBody] LoadDataReportRequest request, CancellationToken cancellationToken)
    {
        var command = new ReportLoadDataCommand(request.ReportId);

        var result = await _mediator.SendCommand(command);

        if (!result.IsValid)
        {
            return Ok(result);
        }
        return LoadEvent(request, result, cancellationToken);
    }

    private IActionResult LoadEvent(LoadDataReportRequest request, FluentValidation.Results.ValidationResult result, CancellationToken cancellationToken)
    {
        if (request.ReportType == "RP001")
        {
            var ev = new AddRptSoChiTietBanHangEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP007")
        {
            var ev = new AddRptTinhHinhThucHienDonBanHangEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP008")
        {
            var ev = new AddRptTinhHinhThucHienHopDongBanEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP002")
        {
            var ev = new AddRptTongHopBanHangTheoMatHangEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP003")
        {
            var ev = new AddRptTongHopBanHangTheoKhachHangEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP006")
        {
            var ev = new AddRptTongHopBanHangTheoNhanVienEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP004")
        {
            var ev = new AddRptTongHopBanHangTheoNhomKhachHangEvent()
            {
                ReportId = request.ReportId,
                EmployeeId = request.EmployeeId,
                CustomerGroupId = request.CustomerGroupId,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP009")
        {
            var ev = new AddRptTongHopBanHangTheoKhachHangVaDonBanHangEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        else if (request.ReportType == "RP005")
        {
            var ev = new AddRptTongHopBanHangTheoMatHangVaKhachHangEvent()
            {
                ReportId = request.ReportId,
                CustomerCode = request.CustomerCode,
                EmployeeId = request.EmployeeId,
                CategoryRootId = request.CategoryRootId,
                ProductCode = request.ProductCode,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };
            ev.Tenant = _context.Tenant;
            ev.Data = _context.Data;
            ev.Data_Zone = _context.Data_Zone;

            _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
            return Ok(result);
        }
        return Ok(result);
    }
}
