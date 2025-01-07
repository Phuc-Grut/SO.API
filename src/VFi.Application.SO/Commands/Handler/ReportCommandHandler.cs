using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.SO.Commands;

internal class ReportCommandHandler : CommandHandler, IRequestHandler<ReportAddCommand, ValidationResult>,
                                                        IRequestHandler<ReportDeleteCommand, ValidationResult>,
                                                        IRequestHandler<ReportEditCommand, ValidationResult>,
                                                        IRequestHandler<ReportLoadDataCommand, ValidationResult>
{
    private readonly IReportRepository _repository;
    private readonly IContextUser _context;
    private readonly ISOContextProcedures _soContextProcedures;

    public ReportCommandHandler(
        IReportRepository reportRepository,
        IContextUser contextUser,
        ISOContextProcedures soContextProcedures)
    {
        _repository = reportRepository;
        _context = contextUser;
        _soContextProcedures = soContextProcedures;
    }
    public void Dispose()
    {
        _repository.Dispose();
    }

    public async Task<ValidationResult> Handle(ReportAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var createName = _context.UserClaims.FullName;
        var createdBy = _context.GetUserId();
        var createdDate = DateTime.Now;
        var Report = new Report
        {
            Id = request.Id,
            Name = request.Name,
            ReportTypeId = request.ReportTypeId,
            ReportTypeCode = request.ReportTypeCode,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            CustomerId = request.CustomerId,
            CustomerCode = request.CustomerCode,
            CustomerName = request.CustomerName,
            EmployeeId = request.EmployeeId,
            EmployeeCode = request.EmployeeCode,
            EmployeeName = request.EmployeeName,
            CategoryRootId = request.CategoryRootId,
            CategoryRootName = request.CategoryRootName,
            ProductId = request.ProductId,
            ProductCode = request.ProductCode,
            ProductName = request.ProductName,
            CustomerGroupId = request.CustomerGroupId,
            CustomerGroupCode = request.CustomerGroupCode,
            CustomerGroupName = request.CustomerGroupName,
            CurrencyCode = request.CurrencyCode,
            Status = request.Status,
            LoadDate = request.LoadDate,
            CreatedDate = createdDate,
            CreatedBy = createdBy,
            CreatedByName = createName
        };

        _repository.Add(Report);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ReportDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var Report = new Report
        {
            Id = request.Id
        };

        _repository.Remove(Report);
        return await Commit(_repository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ReportEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_repository))
            return request.ValidationResult;
        var obj = await _repository.GetById(request.Id);
        if (obj is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Report is not exist") } };
        }
        var updateName = _context.UserClaims.FullName;
        var updatedBy = _context.GetUserId();
        var updatedDate = DateTime.Now;
        var item = await _repository.GetById(request.Id);
        item.Id = request.Id;
        item.Name = request.Name;
        item.ReportTypeId = request.ReportTypeId;
        item.ReportTypeCode = request.ReportTypeCode;
        item.FromDate = request.FromDate;
        item.ToDate = request.ToDate;
        item.CustomerId = request.CustomerId;
        item.CustomerCode = request.CustomerCode;
        item.CustomerName = request.CustomerName;
        item.EmployeeId = request.EmployeeId;
        item.EmployeeCode = request.EmployeeCode;
        item.EmployeeName = request.EmployeeName;
        item.CategoryRootId = request.CategoryRootId;
        item.CategoryRootName = request.CategoryRootName;
        item.ProductId = request.ProductId;
        item.ProductCode = request.ProductCode;
        item.ProductName = request.ProductName;
        item.CustomerGroupId = request.CustomerGroupId;
        item.CustomerGroupCode = request.CustomerGroupCode;
        item.CustomerGroupName = request.CustomerGroupName;
        item.CurrencyCode = request.CurrencyCode;
        item.Status = request.Status;
        item.LoadDate = request.LoadDate;
        item.UpdatedDate = updatedDate;
        item.UpdatedBy = updatedBy;
        item.UpdatedByName = updateName;

        _repository.Update(item);
        return await Commit(_repository.UnitOfWork);
    }
    public async Task<ValidationResult> Handle(ReportLoadDataCommand request, CancellationToken cancellationToken)
    {
        var report = await _repository.GetById(request.ReportId);
        if (report is null)
        {
            return new ValidationResult() { Errors = new List<ValidationFailure>() { new ValidationFailure("id", "Report is not exist") } };
        }
        report.Status = 1;
        _repository.Update(report);
        return await Commit(_repository.UnitOfWork);
    }
}
