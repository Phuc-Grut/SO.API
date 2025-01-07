using Consul;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.SO.Queries;

public class ReportPagingFilterQuery : FopQuery, IQuery<PagedResult<List<ReportDto>>>
{
    public ReportPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class ReportQueryById : IQuery<ReportDto>
{
    public ReportQueryById(Guid reportId)
    {
        ReportId = reportId;
    }

    public Guid ReportId { get; set; }
}
public class RptSoChiTietBanHangPagingFilterQuery : FopQuery, IQuery<PagedResult<List<RptSoChiTietBanHangDto>>>
{
    public RptSoChiTietBanHangPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTongHopBanTheoMatHangPagingFilterQuery : FopQuery, IQuery<PagedResult<List<RptTongHopBanHangTheoMatHangDto>>>
{
    public RptTongHopBanTheoMatHangPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTongHopBanTheoKhachHangPagingFilterQuery : FopQuery, IQuery<PagedResult<List<RptTongHopBanHangTheoKhachHangDto>>>
{
    public RptTongHopBanTheoKhachHangPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTongHopBanTheoNhanVienPagingFilterQuery : FopQuery, IQuery<PagedResult<List<RptTongHopBanHangTheoNhanVienDto>>>
{
    public RptTongHopBanTheoNhanVienPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTinhHinhThucHienDonBanHangFilterQuery : FopQuery, IQuery<PagedResult<List<RptTinhHinhThucHienDonBanHangDto>>>
{
    public RptTinhHinhThucHienDonBanHangFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTinhHinhThucHienHopDongBanFilterQuery : FopQuery, IQuery<PagedResult<List<RptTinhHinhThucHienHopDongBanDto>>>
{
    public RptTinhHinhThucHienHopDongBanFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTongHopBanTheoNhomKhachHangPagingFilterQuery : FopQuery, IQuery<PagedResult<List<RptTongHopBanHangTheoNhomKhachHangDto>>>
{
    public RptTongHopBanTheoNhomKhachHangPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTongHopBanHangTheoMatHangVaKhachHangPagingFilterQuery : FopQuery, IQuery<PagedResult<List<RptTongHopBanHangTheoMatHangVaKhachHangDto>>>
{
    public RptTongHopBanHangTheoMatHangVaKhachHangPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class RptTongHopBanHangTheoKhachHangVaDonBanHangPagingFilterQuery : FopQuery, IQuery<PagedResult<List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>>>
{
    public RptTongHopBanHangTheoKhachHangVaDonBanHangPagingFilterQuery(string keyword, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
public class ReportQueryHandler : IQueryHandler<ReportPagingFilterQuery, PagedResult<List<ReportDto>>>,
                                  IQueryHandler<RptSoChiTietBanHangPagingFilterQuery, PagedResult<List<RptSoChiTietBanHangDto>>>,
                                  IQueryHandler<RptTongHopBanTheoMatHangPagingFilterQuery, PagedResult<List<RptTongHopBanHangTheoMatHangDto>>>,
                                  IQueryHandler<RptTongHopBanTheoKhachHangPagingFilterQuery, PagedResult<List<RptTongHopBanHangTheoKhachHangDto>>>,
                                  IQueryHandler<RptTongHopBanTheoNhanVienPagingFilterQuery, PagedResult<List<RptTongHopBanHangTheoNhanVienDto>>>,
                                  IQueryHandler<RptTinhHinhThucHienDonBanHangFilterQuery, PagedResult<List<RptTinhHinhThucHienDonBanHangDto>>>,
                                  IQueryHandler<RptTinhHinhThucHienHopDongBanFilterQuery, PagedResult<List<RptTinhHinhThucHienHopDongBanDto>>>,
                                  IQueryHandler<RptTongHopBanHangTheoMatHangVaKhachHangPagingFilterQuery, PagedResult<List<RptTongHopBanHangTheoMatHangVaKhachHangDto>>>,
                                  IQueryHandler<RptTongHopBanHangTheoKhachHangVaDonBanHangPagingFilterQuery, PagedResult<List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>>>,
                                  IQueryHandler<RptTongHopBanTheoNhomKhachHangPagingFilterQuery, PagedResult<List<RptTongHopBanHangTheoNhomKhachHangDto>>>,
                                  IQueryHandler<ReportQueryById, ReportDto>
{
    private readonly IReportRepository _reportRepository;
    private readonly IRptSoChiTietBanHangRepository _rptSoChiTietBanHangRepository;
    private readonly IRptTongHopBanHangTheoMatHangRepository _rptTongHopBanHangTheoMatHangRepository;
    private readonly IRptTongHopBanHangTheoKhachHangRepository _rptTongHopBanHangTheoKhachHangRepository;
    private readonly IRptTongHopBanHangTheoNhanVienRepository _rptTongHopBanHangTheoNhanVienRepository;
    private readonly IRptTinhHinhThucHienDonBanHangRepository _rptTinhHinhThucHienDonBanHangRepository;
    private readonly IRptTinhHinhThucHienHopDongBanRepository _rptTinhHinhThucHienHopDongBanRepository;
    private readonly IRptTongHopBanHangTheoNhomKhachHangRepository _rptTongHopBanHangTheoNhomKhachHangRepository;
    private readonly IRptTongHopBanHangTheoKhachHangVaDonBanHangRepository _rptTongHopBanHangTheoKhachHangVaDonBanHangRepository;
    private readonly IRptTongHopBanHangTheoMatHangVaKhachHangRepository _rptTongHopBanHangTheoMatHangVaKhachHangRepository;

    public ReportQueryHandler(
        IReportRepository reportRepository,
        IRptSoChiTietBanHangRepository rptSoChiTietBanHangRepository,
        IRptTongHopBanHangTheoMatHangRepository rptTongHopBanHangTheoMatHangRepository,
        IRptTongHopBanHangTheoKhachHangRepository rptTongHopBanHangTheoKhachHangRepository,
        IRptTongHopBanHangTheoNhanVienRepository rptTongHopBanHangTheoNhanVienRepository,
        IRptTinhHinhThucHienDonBanHangRepository rptTinhHinhThucHienDonBanHangRepository,
        IRptTinhHinhThucHienHopDongBanRepository rptTinhHinhThucHienHopDongBanRepository,
        IRptTongHopBanHangTheoNhomKhachHangRepository rptTongHopBanHangTheoNhomKhachHangRepository,
        IRptTongHopBanHangTheoKhachHangVaDonBanHangRepository rptTongHopBanHangTheoKhachHangVaDonBanHangRepository,
        IRptTongHopBanHangTheoMatHangVaKhachHangRepository rptTongHopBanHangTheoMatHangVaKhachHangRepository
        )
    {
        _reportRepository = reportRepository;
        _rptSoChiTietBanHangRepository = rptSoChiTietBanHangRepository;
        _rptTongHopBanHangTheoMatHangRepository = rptTongHopBanHangTheoMatHangRepository;
        _rptTongHopBanHangTheoKhachHangRepository = rptTongHopBanHangTheoKhachHangRepository;
        _rptTongHopBanHangTheoNhanVienRepository = rptTongHopBanHangTheoNhanVienRepository;
        _rptTinhHinhThucHienDonBanHangRepository = rptTinhHinhThucHienDonBanHangRepository;
        _rptTinhHinhThucHienHopDongBanRepository = rptTinhHinhThucHienHopDongBanRepository;
        _rptTongHopBanHangTheoNhomKhachHangRepository = rptTongHopBanHangTheoNhomKhachHangRepository;
        _rptTongHopBanHangTheoKhachHangVaDonBanHangRepository = rptTongHopBanHangTheoKhachHangVaDonBanHangRepository;
        _rptTongHopBanHangTheoMatHangVaKhachHangRepository = rptTongHopBanHangTheoMatHangVaKhachHangRepository;
    }
    public async Task<PagedResult<List<ReportDto>>> Handle(ReportPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ReportDto>>();
        var fopRequest = FopExpressionBuilder<Report>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var (datas, count) = await _reportRepository.Filter(request.Keyword, fopRequest);
        var result = datas.Select(item =>
        {
            return new ReportDto()
            {
                Id = item.Id,
                Name = item.Name,
                ReportTypeId = item.ReportTypeId,
                ReportTypeCode = item.ReportTypeCode,
                FromDate = item.FromDate,
                ToDate = item.ToDate,
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                EmployeeId = item.EmployeeId,
                EmployeeCode = item.EmployeeCode,
                EmployeeName = item.EmployeeName,
                CategoryRootId = item.CategoryRootId,
                CategoryRootName = item.CategoryRootName,
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                CustomerGroupId = item.CustomerGroupId,
                CustomerGroupCode = item.CustomerGroupCode,
                CustomerGroupName = item.CustomerGroupName,
                CurrencyCode = item.CurrencyCode,
                Status = item.Status,
                LoadDate = item.LoadDate,
                CreatedBy = item.CreatedBy,
                CreatedByName = item.CreatedByName,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedByName = item.UpdatedByName,
                UpdatedDate = item.UpdatedDate
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<ReportDto> Handle(ReportQueryById request, CancellationToken cancellationToken)
    {
        var item = await _reportRepository.GetById(request.ReportId);
        var result = new ReportDto()
        {
            Id = item.Id,
            Name = item.Name,
            ReportTypeId = item.ReportTypeId,
            ReportTypeCode = item.ReportTypeCode,
            FromDate = item.FromDate,
            ToDate = item.ToDate,
            CustomerId = item.CustomerId,
            CustomerCode = item.CustomerCode,
            CustomerName = item.CustomerName,
            EmployeeId = item.EmployeeId,
            EmployeeCode = item.EmployeeCode,
            EmployeeName = item.EmployeeName,
            CategoryRootId = item.CategoryRootId,
            CategoryRootName = item.CategoryRootName,
            ProductId = item.ProductId,
            ProductCode = item.ProductCode,
            ProductName = item.ProductName,
            CustomerGroupId = item.CustomerGroupId,
            CustomerGroupCode = item.CustomerGroupCode,
            CustomerGroupName = item.CustomerGroupName,
            CurrencyCode = item.CurrencyCode,
            Status = item.Status,
            LoadDate = item.LoadDate,
            CreatedBy = item.CreatedBy,
            CreatedByName = item.CreatedByName,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedByName = item.UpdatedByName,
            UpdatedDate = item.UpdatedDate
        };
        return result;
    }
    public async Task<PagedResult<List<RptSoChiTietBanHangDto>>> Handle(RptSoChiTietBanHangPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptSoChiTietBanHangDto>>();
        var fopRequest = FopExpressionBuilder<RptSoChiTietBanHang>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptSoChiTietBanHangRepository.Filter(request.Keyword, fopRequest);
        var result = datas?.Select(item =>
        {
            return new RptSoChiTietBanHangDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                OrderCode = item.OrderCode,
                OrderDate = item.OrderDate,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                UnitCode = item.UnitCode,
                UnitName = item.UnitName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                AmountDiscount = item.AmountDiscount,
                TaxRate = item.TaxRate,
                ReturnQuantity = item.ReturnQuantity,
                ReturnAmount = item.ReturnAmount,
                SaleAmount = item.SaleAmount
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<RptTongHopBanHangTheoMatHangDto>>> Handle(RptTongHopBanTheoMatHangPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTongHopBanHangTheoMatHangDto>>();
        var fopRequest = FopExpressionBuilder<RptTongHopBanHangTheoMatHang>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTongHopBanHangTheoMatHangRepository.Filter(request.Keyword, fopRequest);
        var result = datas?.Select(item =>
        {
            return new RptTongHopBanHangTheoMatHangDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                UnitCode = item.UnitCode,
                UnitName = item.UnitName,
                Quantity = item.Quantity,
                AmountNoDiscount = item.AmountNoDiscount,
                AmountDiscount = item.AmountDiscount,
                ReturnQuantity = item.ReturnQuantity,
                ReturnAmount = item.ReturnAmount,
                SaleAmount = item.SaleAmount
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<RptTongHopBanHangTheoKhachHangDto>>> Handle(RptTongHopBanTheoKhachHangPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTongHopBanHangTheoKhachHangDto>>();
        var fopRequest = FopExpressionBuilder<RptTongHopBanHangTheoKhachHang>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTongHopBanHangTheoKhachHangRepository.Filter(request.Keyword, fopRequest);
        var result = datas?.Select(item =>
        {
            return new RptTongHopBanHangTheoKhachHangDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                AmountNoDiscount = item.AmountNoDiscount,
                AmountDiscount = item.AmountDiscount,
                AmountTax = item.AmountTax,
                ReturnAmount = item.ReturnAmount,
                SaleAmount = item.SaleAmount
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<RptTongHopBanHangTheoNhanVienDto>>> Handle(RptTongHopBanTheoNhanVienPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTongHopBanHangTheoNhanVienDto>>();
        var fopRequest = FopExpressionBuilder<RptTongHopBanHangTheoNhanVien>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTongHopBanHangTheoNhanVienRepository.Filter(request.Keyword, fopRequest);
        var result = datas?.Select(item =>
        {
            return new RptTongHopBanHangTheoNhanVienDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                EmployeeCode = item.EmployeeCode,
                EmployeeName = item.EmployeeName,
                AmountNoDiscount = item.AmountNoDiscount,
                AmountDiscount = item.AmountDiscount,
                AmountTax = item.AmountTax,
                ReturnAmount = item.ReturnAmount,
                SaleAmount = item.SaleAmount
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<RptTinhHinhThucHienDonBanHangDto>>> Handle(RptTinhHinhThucHienDonBanHangFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTinhHinhThucHienDonBanHangDto>>();
        var fopRequest = FopExpressionBuilder<RptTinhHinhThucHienDonBanHang>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTinhHinhThucHienDonBanHangRepository.Filter(request.Keyword, fopRequest);
        var result = datas?.Select(item =>
        {
            return new RptTinhHinhThucHienDonBanHangDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                OrderCode = item.OrderCode,
                OrderDate = item.OrderDate,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                UnitCode = item.UnitCode,
                UnitName = item.UnitName,
                Quantity = item.Quantity,
                QuantityDelivery = item.QuantityDelivery,
                SaleOrder = item.SaleOrder,
                SaleMade = item.SaleMade
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<RptTinhHinhThucHienHopDongBanDto>>> Handle(RptTinhHinhThucHienHopDongBanFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTinhHinhThucHienHopDongBanDto>>();
        var fopRequest = FopExpressionBuilder<RptTinhHinhThucHienHopDongBan>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTinhHinhThucHienHopDongBanRepository.Filter(request.Keyword, fopRequest);
        var result = datas?.Select(item =>
        {
            return new RptTinhHinhThucHienHopDongBanDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                Code = item.Code,
                Date = item.Date,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                UnitCode = item.UnitCode,
                UnitName = item.UnitName,
                Quantity = item.Quantity,
                QuantitySold = item.QuantitySold,
                SaleContract = item.SaleContract,
                SaleMade = item.SaleMade
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<RptTongHopBanHangTheoNhomKhachHangDto>>> Handle(RptTongHopBanTheoNhomKhachHangPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTongHopBanHangTheoNhomKhachHangDto>>();
        var fopRequest = FopExpressionBuilder<RptTongHopBanHangTheoNhomKhachHang>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTongHopBanHangTheoNhomKhachHangRepository.Filter(request.Keyword, fopRequest);
        var result = datas?.Select(item =>
        {
            return new RptTongHopBanHangTheoNhomKhachHangDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                Code = item.Code,
                Name = item.Name,
                AmountNoDiscount = item.AmountNoDiscount,
                AmountDiscount = item.AmountDiscount,
                AmountTax = item.AmountTax,
                ReturnAmount = item.ReturnAmount,
                SaleAmount = item.SaleAmount
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<PagedResult<List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>>> Handle(RptTongHopBanHangTheoKhachHangVaDonBanHangPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>>();
        var fopRequest = FopExpressionBuilder<RptTongHopBanHangTheoKhachHangVaDonBanHang>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTongHopBanHangTheoKhachHangVaDonBanHangRepository.Filter(request.Keyword, fopRequest);
        var dataTree = datas?.Select(item =>
        {
            return new RptTongHopBanHangTheoKhachHangVaDonBanHangDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                ParentId = item.ParentId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                OrderCode = item.OrderCode,
                OrderDate = item.OrderDate,
                TotalAmountTax = item.TotalAmountTax,
                PaymentAmount = item.PaymentAmount
            };
        }
        ).ToList();
        var result = dataTree.Any() ? BuildTreeKH(dataTree).ToList() : dataTree;
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<PagedResult<List<RptTongHopBanHangTheoMatHangVaKhachHangDto>>> Handle(RptTongHopBanHangTheoMatHangVaKhachHangPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<RptTongHopBanHangTheoMatHangVaKhachHangDto>>();
        var fopRequest = FopExpressionBuilder<RptTongHopBanHangTheoMatHangVaKhachHang>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _rptTongHopBanHangTheoMatHangVaKhachHangRepository.Filter(request.Keyword, fopRequest);
        var dataTree = datas?.Select(item =>
        {
            return new RptTongHopBanHangTheoMatHangVaKhachHangDto()
            {
                Id = item.Id,
                ReportId = item.ReportId,
                ParentId = item.ParentId,
                Code = item.Code,
                Name = item.Name,
                UnitCode = item.UnitCode,
                UnitName = item.UnitName,
                Quantity = item.Quantity,
                AmountNoDiscount = item.AmountNoDiscount,
                AmountDiscount = item.AmountDiscount,
                AmountTax = item.AmountTax,
                ReturnQuantity = item.ReturnQuantity,
                ReturnAmount = item.ReturnAmount,
                SaleAmount = item.SaleAmount
            };
        }
        ).ToList();
        var result = dataTree.Any() ? BuildTreeMH(dataTree).ToList() : dataTree;
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    #region BuildTreeKH
    private IEnumerable<RptTongHopBanHangTheoKhachHangVaDonBanHangDto> BuildTreeKH(IEnumerable<RptTongHopBanHangTheoKhachHangVaDonBanHangDto> resource)
    {
        var groups = resource.GroupBy(i => i.ParentId);
        List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto> result = new List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>();

        var roots = groups.FirstOrDefault(x => x.Key == null);
        if (roots is not null && roots.Count() > 0)
        {
            var dict = groups.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.OrderBy(x => x.OrderDate).ToList());

            foreach (var item in roots.OrderBy(x => x.OrderDate))
            {
                bool flag = AddChildrenKH(item, dict);
                if (flag)
                {
                    result.Add(item);
                }
            }
        }
        return result;
    }

    private bool AddChildrenKH(RptTongHopBanHangTheoKhachHangVaDonBanHangDto node, IDictionary<Guid?, List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>> source)
    {
        bool allowNode = false;
        if (source.ContainsKey(node.Id))
        {
            node.Children = new List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>();
            foreach (var item in source[node.Id])
            {
                bool flag = AddChildrenKH(item, source);
                if (flag)
                {
                    allowNode = true;
                    node.Children.Add(item);
                }
            }
        }
        else
        {
            node.Children = new List<RptTongHopBanHangTheoKhachHangVaDonBanHangDto>();
        }
        if (!allowNode)
        {
            return true;
        }
        return allowNode;
    }
    #endregion
    #region BuildTreeMH
    private IEnumerable<RptTongHopBanHangTheoMatHangVaKhachHangDto> BuildTreeMH(IEnumerable<RptTongHopBanHangTheoMatHangVaKhachHangDto> resource)
    {
        var groups = resource.GroupBy(i => i.ParentId);
        List<RptTongHopBanHangTheoMatHangVaKhachHangDto> result = new List<RptTongHopBanHangTheoMatHangVaKhachHangDto>();

        var roots = groups.FirstOrDefault(x => x.Key == null);
        if (roots is not null && roots.Count() > 0)
        {
            var dict = groups.Where(g => g.Key != null).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var item in roots)
            {
                bool flag = AddChildrenMH(item, dict);
                if (flag)
                {
                    result.Add(item);
                }
            }
        }
        return result;
    }

    private bool AddChildrenMH(RptTongHopBanHangTheoMatHangVaKhachHangDto node, IDictionary<Guid?, List<RptTongHopBanHangTheoMatHangVaKhachHangDto>> source)
    {
        bool allowNode = false;
        if (source.ContainsKey(node.Id))
        {
            node.Children = new List<RptTongHopBanHangTheoMatHangVaKhachHangDto>();
            foreach (var item in source[node.Id])
            {
                bool flag = AddChildrenMH(item, source);
                if (flag)
                {
                    allowNode = true;
                    node.Children.Add(item);
                }
            }
        }
        else
        {
            node.Children = new List<RptTongHopBanHangTheoMatHangVaKhachHangDto>();
        }
        if (!allowNode)
        {
            return true;
        }
        return allowNode;
    }
    #endregion
}
