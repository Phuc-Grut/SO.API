using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Consul.Filtering;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using MassTransit.Futures.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.SO.DTOs;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace VFi.Application.SO.Queries;

public class CustomerExcelQueryAll : IQuery<IEnumerable<CustomerDto>>
{
    public CustomerExcelQueryAll(string? keyword, int? type, int? status)
    {
        Keyword = keyword;
        Type = type;
        Status = status;
    }
    public string? Keyword { get; set; }
    public int? Type { get; set; }
    public int? Status { get; set; }
}
public class AccountEmailCheckExist : IQuery<bool>
{
    public AccountEmailCheckExist()
    {
    }

    public AccountEmailCheckExist(string email)
    {
        Email = email;
    }

    public string Email { get; set; }

}
public class CustomerQueryComboBox : IQuery<IEnumerable<CustomerCbxDto>>
{
    public CustomerQueryComboBox(int? status, string? keyword)
    {
        Status = status;
        Keyword = keyword;
    }
    public int? Status { get; set; }
    public string? Keyword { get; set; }
}
public class CustomerQueryCheckCode : IQuery<bool>
{
    public CustomerQueryCheckCode(string code)
    {
        Code = code;
    }
    public string Code { get; set; }
}
public class CustomerQueryById : IQuery<CustomerDto>
{
    public CustomerQueryById()
    {
    }

    public CustomerQueryById(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; set; }
}
public class CustomerQueryByCode : IQuery<CustomerDto>
{
    public CustomerQueryByCode([FromBody] string code)
    {
        Code = code;
    }
    public string Code { get; set; }
}
public class CustomerPagingQuery : FopQuery, IQuery<PagedResult<List<CustomerDto>>>
{
    public CustomerPagingQuery(string? keyword, CustomerParams queryParams, string filter, string order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        QueryParams = queryParams;
    }
    public CustomerParams? QueryParams { get; set; }
}

public class CustomerImportTemplateQuery : IQuery<MemoryStream>
{
    public CustomerImportTemplateQuery()
    {

    }

}
public class ValidateCustomerImportQuery : IQuery<List<CustomerValidateDto>>
{
    public ValidateCustomerImportQuery(IFormFile file, string sheetId, int headerRow, List<ValidateField> listField)
    {
        File = file;
        ListField = listField;
        SheetId = sheetId;
        HeaderRow = headerRow;
    }

    public string SheetId { get; set; } = null!;
    public IFormFile File { get; set; }
    public int HeaderRow { get; set; }
    public List<ValidateField> ListField { get; set; }
}
public partial class CustomerQueryHandler : IQueryHandler<CustomerQueryComboBox, IEnumerable<CustomerCbxDto>>,
                                         IQueryHandler<CustomerExcelQueryAll, IEnumerable<CustomerDto>>,
                                         IQueryHandler<CustomerQueryCheckCode, bool>,
                                         IQueryHandler<AccountEmailCheckExist, bool>,
                                         IQueryHandler<CustomerQueryById, CustomerDto>,
                                         IQueryHandler<CustomerQueryByCode, CustomerDto>,
                                         IQueryHandler<CustomerPagingQuery, PagedResult<List<CustomerDto>>>,
    IQueryHandler<CustomerImportTemplateQuery, MemoryStream>,
    IQueryHandler<ValidateCustomerImportQuery, List<CustomerValidateDto>>

{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICustomerGroupRepository _customerGroupRepository;
    private readonly IBusinessRepository _businessRepository;
    private readonly IGroupEmployeeRepository _groupEmployeeRepository;
    private readonly ICustomerGroupMappingRepository _groupMapping;
    private readonly ICustomerBusinessMappingRepository _businessMapping;
    private readonly IExportTemplateRepository _exportTemplateRepository;
    private readonly IPriceListRepository _priceListRepository;
    private readonly ICustomerSourceRepository _customerSourceRepository;
    private readonly IPIMRepository _pimRepository;
    public CustomerQueryHandler(
                                ICustomerRepository customerRespository,
                                IEmployeeRepository employeeRepository,
                                ICustomerGroupRepository customerGroupRepository,
                                IBusinessRepository businessRepository,
                                IGroupEmployeeRepository groupEmployeeRepository,
                                ICustomerGroupMappingRepository groupMapping,
                                ICustomerBusinessMappingRepository businessMapping,
                                IPriceListRepository priceListRepository,
                                IExportTemplateRepository exportTemplateRepository,
                                ICustomerSourceRepository customerSourceRepository,
                                IPIMRepository pimRepository
        )
    {
        _customerRepository = customerRespository;
        _employeeRepository = employeeRepository;
        _customerGroupRepository = customerGroupRepository;
        _businessRepository = businessRepository;
        _groupEmployeeRepository = groupEmployeeRepository;
        _groupMapping = groupMapping;
        _businessMapping = businessMapping;
        _priceListRepository = priceListRepository;
        _exportTemplateRepository = exportTemplateRepository;
        _customerSourceRepository = customerSourceRepository;
        _pimRepository = pimRepository;

    }
    public async Task<bool> Handle(CustomerQueryCheckCode request, CancellationToken cancellationToken)
    {
        var data = await _customerRepository.GetByCode(request.Code);
        if (data is null)
        {
            return false;
        }
        return true;
    }

    public async Task<CustomerDto> Handle(CustomerQueryById request, CancellationToken cancellationToken)
    {
        var obj = await _customerRepository.GetById(request.CustomerId);
        var priceListName = obj.PriceListId != null ? _priceListRepository.GetById((Guid)obj.PriceListId).Result?.Name : null;
        var empGroupName = obj.GroupEmployeeId != null ? _groupEmployeeRepository.GetById((Guid)obj.GroupEmployeeId).Result?.Name : null;
        var empName = obj.EmployeeId != null ? _employeeRepository.GetByAccountId((Guid)obj.EmployeeId).Result?.Name : null;
        var filter = new Dictionary<string, object>();
        filter.Add("customerId", request.CustomerId);

        var listGroup = await _groupMapping.GetListListBox(filter);
        var groups = await _customerGroupRepository.Filter(listGroup.Select(x => x.CustomerGroupId).ToList());

        var listBusiness = await _businessMapping.GetListListBox(filter);
        var businesses = await _businessRepository.GetById(listBusiness);

        var result = new CustomerDto()
        {
            Id = obj.Id,
            CustomerSourceId = obj.CustomerSourceId,
            CustomerSourceName = obj.CustomerSourceName,
            Image = obj.Image,
            Type = obj.Type,
            Code = obj.Code,
            Name = obj.Name,
            Phone = obj.Phone,
            Email = obj.Email,
            Fax = obj.Fax,
            Country = obj.Country,
            Province = obj.Province,
            District = obj.District,
            Ward = obj.Ward,
            ZipCode = obj.ZipCode,
            Address = obj.Address,
            Website = obj.Website,
            TaxCode = obj.TaxCode,
            BusinessSector = obj.BusinessSector,
            CompanyName = obj.CompanyName,
            CompanyPhone = obj.CompanyPhone,
            CompanySize = obj.CompanySize,
            Capital = obj.Capital,
            EstablishedDate = obj.EstablishedDate,
            Tags = obj.Tags,
            Note = obj.Note,
            Status = obj.Status,
            EmployeeId = obj.EmployeeId,
            EmployeeName = empName,
            GroupEmployeeId = obj.GroupEmployeeId,
            GroupEmployeeName = empGroupName,
            IsVendor = obj.IsVendor,
            IsAuto = obj.IsAuto,
            Gender = obj.Gender,
            Year = obj.Year,
            Month = obj.Month,
            Day = obj.Day,
            CurrencyId = obj.CurrencyId,
            CurrencyName = obj.CurrencyName,
            PriceListId = obj.PriceListId,
            PriceListName = priceListName,
            DebtLimit = obj.DebtLimit,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            CreatedByName = obj.CreatedByName,
            UpdatedByName = obj.UpdatedByName,
            IdName = obj.IdName,
            IdNumber = obj.IdNumber,
            IdDate = obj.IdDate,
            IdIssuer = obj.IdIssuer,
            IdImage1 = obj.IdImage1,
            IdImage2 = obj.IdImage2,
            IdStatus = obj.IdStatus,
            CccdNumber = obj.CccdNumber,
            DateRange = obj.DateRange,
            Birthday = obj.Birthday,
            IssuedBy = obj.IssuedBy,
            ListGroup = listGroup.Select(x => new CustomerMappingDto()
            {
                Value = x.CustomerGroupId
            }).ToList(),
            Groups = String.Join(", ", groups.ToList().Select(x => x.Name).ToArray()),
            ListBusiness = listBusiness.Select(x => new CustomerMappingDto()
            {
                Value = x.BusinessId
            }).ToList(),
            Businesses = String.Join(", ", businesses.ToList().Select(x => x.Name).ToArray()),
            ListAddress = obj.CustomerAddress.Select(x => new CustomerAddressDto()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Name = x.Name,
                Country = x.Country,
                Province = x.Province,
                District = x.District,
                Ward = x.Ward,
                Address = x.Address,
                Phone = x.Phone,
                Email = x.Email,
                ShippingDefault = x.ShippingDefault,
                BillingDefault = x.BillingDefault,
                Status = x.Status
            }).ToList(),
            ListContact = obj.CustomerContact.Select(x => new CustomerContactDto()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Name = x.Name,
                Gender = x.Gender,
                Phone = x.Phone,
                Email = x.Email,
                Facebook = x.Facebook,
                Tags = x.Tags,
                Address = x.Address,
                Status = x.Status
            }).ToList(),
            ListBank = obj.CustomerBank.Select(x => new CustomerBankDto()
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                Name = x.Name,
                BankCode = x.BankCode,
                BankName = x.BankName,
                BankBranch = x.BankBranch,
                AccountName = x.AccountName,
                AccountNumber = x.AccountNumber,
                Default = x.Default,
                Status = x.Status,
                SortOrder = x.SortOrder
            }).OrderBy(x => x.SortOrder).ToList(),
            AccountId = obj.AccountId,
            AccountEmail = obj.AccountEmail,
            AccountEmailVerified = obj.AccountEmailVerified,
            AccountUsername = obj.AccountUsername,
            AccountCreatedDate = obj.AccountCreatedDate,
            AccountPhone = obj.AccountPhone,
            AccountPhoneVerified = obj.AccountPhoneVerified,
            Representative = obj.Representative,
            Revenue = obj.Revenue,
            RevenueMonth = obj.RevenueMonth,
            BmapGroup = obj.BmapGroup
        };
        return result;
    }

    public async Task<PagedResult<List<CustomerDto>>> Handle(CustomerPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<CustomerDto>>();

        var fopRequest = FopExpressionBuilder<Customer>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
        var filter = new Dictionary<string, object>();
        var filterListBox = new Dictionary<string, object>();
        if (request.QueryParams?.Status != null)
            filterListBox.Add("status", request.QueryParams.Status.ToString());
        if (request.QueryParams?.Type != null)
            filterListBox.Add("type", request.QueryParams.Type.ToString());
        if (!string.IsNullOrEmpty(request.QueryParams?.CustomerGroupId))
            filterListBox.Add("customerGroupId", request.QueryParams.CustomerGroupId);
        if (!string.IsNullOrEmpty(request.QueryParams?.EmployeeId))
            filterListBox.Add("employeeId", request.QueryParams.EmployeeId);
        var (datas, count) = await _customerRepository.Filter(request.Keyword, filterListBox, fopRequest);
        var employee = await _employeeRepository.GetAll();
        var GroupEmployee = await _groupEmployeeRepository.GetAll();
        var data = datas.Select(obj => new CustomerDto()
        {
            Id = obj.Id,
            Code = obj.Code,
            Name = obj.Name,
            CustomerGroup = obj.CustomerGroup,
            Groups = String.Join(", ", obj.CustomerGroupMapping.Select(x => x.CustomerGroup.Name).ToList()),
            Image = obj.Image,
            CustomerSourceName = obj.CustomerSourceName,
            Phone = obj.Phone,
            Email = obj.Email,
            Website = obj.Website,
            TaxCode = obj.TaxCode,
            BusinessSector = obj.BusinessSector,
            CompanySize = obj.CompanySize,
            Capital = obj.Capital,
            EstablishedDate = obj.EstablishedDate,
            EmployeeId = obj.EmployeeId,
            EmployeeName = (obj.EmployeeId != null) ? employee.FirstOrDefault(x => x.AccountId == obj.EmployeeId)?.Name : null,
            GroupEmployeeId = obj.GroupEmployeeId,
            GroupEmployeeName = obj.GroupEmployeeId != null ? GroupEmployee.FirstOrDefault(x => x.Id == obj.GroupEmployeeId)?.Name : null,
            Address = obj.Address,
            Status = obj.Status,
            CreatedDate = obj.CreatedDate,
            Type = obj.Type,
            Year = obj.Year,
            Gender = obj.Gender,
            Country = obj.Country,
            Province = obj.Province,
            District = obj.District,
            Ward = obj.Ward,
            Tags = obj.Tags,
            Note = obj.Note,
            PriceListId = obj.PriceListId,
            PriceListName = obj.PriceListName,
            Currency = obj.Currency,
            CurrencyName = obj.CurrencyName,
            IdName = obj.IdName,
            IdNumber = obj.IdNumber,
            IdDate = obj.IdDate,
            IdIssuer = obj.IdIssuer,
            IdImage1 = obj.IdImage1,
            IdImage2 = obj.IdImage2,
            IdStatus = obj.IdStatus,
            CccdNumber = obj.CccdNumber,
            DateRange = obj.DateRange,
            Birthday = obj.Birthday,
            IssuedBy = obj.IssuedBy,
            Revenue = obj.Revenue,
            RevenueMonth = obj.RevenueMonth,
            BmapGroup = obj.BmapGroup
        }).ToList();
        response.Items = data;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }
    public async Task<IEnumerable<CustomerDto>> Handle(CustomerExcelQueryAll request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetAll();
        var groupemployee = await _groupEmployeeRepository.GetAll();
        var response = new PagedResult<List<CustomerDto>>();

        var filter = new Dictionary<string, object>();
        var filterListBox = new Dictionary<string, object>();
        if (request.Status != null)
            filterListBox.Add("status", request.Status.ToString());
        if (request.Type != null)
            filterListBox.Add("type", request.Type.ToString());

        var Customers = await _customerRepository.GetAll();
        var result = Customers.Where(x => (x.Type == request.Type || request.Type == null) && x.Status == request.Status).Select((item, i) => new CustomerDto()
        {
            STT = i + 1,
            Id = item.Id,
            Code = item.Code,
            TypeName = item.Type == 0 ? "Cá nhân" : item.Status == 1 ? "Doanh nghiệp" : "",
            Name = item.Name,
            CustomerGroup = item.CustomerGroup,
            Phone = item.Phone,
            Country = item.Country,
            Province = item.Province,
            District = item.District,
            Ward = item.Ward,
            Email = item.Email,
            Note = item.Note,
            Fax = item.Fax,
            TaxCode = item.TaxCode,
            EmployeeId = item.EmployeeId,
            EmployeeName = item.EmployeeId != null ? employee.FirstOrDefault(x => x.Id == item.EmployeeId)?.Name : null,
            GroupEmployeeId = item.GroupEmployeeId,
            GroupEmployeeName = item.GroupEmployeeId != null ? groupemployee.FirstOrDefault(x => x.Id == item.GroupEmployeeId)?.Name : null,
            Address = item.Address,
            Status = item.Status,
            StatusString = item.Status == 0 ? "Không sủ dụng" : item.Status == 1 ? "Sử dụng" : "",
            CreatedDate = item.CreatedDate,
        });
        return result;
    }

    public async Task<IEnumerable<CustomerCbxDto>> Handle(CustomerQueryComboBox request, CancellationToken cancellationToken)
    {

        var Customers = await _customerRepository.GetListCbx(request.Status, request.Keyword);
        var result = Customers.Select(x => new CustomerCbxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = !String.IsNullOrEmpty(x.Code) ? (x.Code + " - " + x.Name) : x.Name,
            Code = x.Code,
            Name = x.Name,
            Phone = x.Phone,
            Email = x.Email,
            Address = x.Address,
            Country = x.Country,
            Province = x.Province,
            District = x.District,
            Ward = x.Ward,
            ZipCode = x.ZipCode,
            TaxCode = x.TaxCode,
            PriceListId = x.PriceListId,
            DebtLimit = x.DebtLimit
        });
        return result;
    }

    public Task<bool> Handle(AccountEmailCheckExist request, CancellationToken cancellationToken)
    {
        var result = _customerRepository.AccountEmailExist(request.Email);
        return result;
    }
    public async Task<CustomerDto> Handle(CustomerQueryByCode request, CancellationToken cancellationToken)
    {
        var obj = await _customerRepository.GetByCode(request.Code);
        var result = new CustomerDto()
        {
            Id = obj.Id,
            CustomerSourceId = obj.CustomerSourceId,
            Image = obj.Image,
            Type = obj.Type,
            Code = obj.Code,
            Name = obj.Name,
            Phone = obj.Phone,
            Email = obj.Email,
            Country = obj.Country,
            Province = obj.Province,
            District = obj.District,
            Ward = obj.Ward,
            ZipCode = obj.ZipCode,
            Address = obj.Address,
            Website = obj.Website,
            TaxCode = obj.TaxCode,
            BusinessSector = obj.BusinessSector,
            CompanyName = obj.CompanyName,
            CompanyPhone = obj.CompanyPhone,
            CompanySize = obj.CompanySize,
            Capital = obj.Capital,
            EstablishedDate = obj.EstablishedDate,
            Tags = obj.Tags,
            Note = obj.Note,
            Status = obj.Status,
            EmployeeId = obj.EmployeeId,
            GroupEmployeeId = obj.GroupEmployeeId,
            IsVendor = obj.IsVendor,
            IsAuto = obj.IsAuto,
            Gender = obj.Gender,
            Year = obj.Year,
            Month = obj.Month,
            Day = obj.Day,
            CurrencyId = obj.CurrencyId,
            CurrencyName = obj.CurrencyName,
            PriceListId = obj.PriceListId,
            PriceListName = obj.PriceListName,
            DebtLimit = obj.DebtLimit,
            Revenue = obj.Revenue,
            RevenueMonth = obj.RevenueMonth,
            CreatedBy = obj.CreatedBy,
            CreatedDate = obj.CreatedDate,
            UpdatedBy = obj.UpdatedBy,
            UpdatedDate = obj.UpdatedDate,
            AccountId = obj.AccountId,
            AccountEmail = obj.AccountEmail,
            AccountEmailVerified = obj.AccountEmailVerified,
            AccountUsername = obj.AccountUsername,
            AccountCreatedDate = obj.AccountCreatedDate,
            AccountPhone = obj.AccountPhone,
            AccountPhoneVerified = obj.AccountPhoneVerified,
            BmapGroup = obj.BmapGroup
        };
        return result;
    }

    public async Task<MemoryStream> Handle(CustomerImportTemplateQuery request, CancellationToken cancellationToken)
    {
        var contentBytes = await _exportTemplateRepository.GetTemplate("MAU_IMPORT_KHACH_HANG");

        MemoryStream memoryStream = new();

        memoryStream.SetLength(0);

        memoryStream.Write(contentBytes, 0, contentBytes.Length);

        string[] CellReferenceArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(memoryStream, true))
        {
            SharedStringTablePart shareStringPart;
            if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }
        }
        return memoryStream;
    }

    public static int? GetColumnIndex(string cellRef)
    {
        if (string.IsNullOrEmpty(cellRef))
            return null;

        cellRef = cellRef.ToUpper();

        int columnIndex = -1;
        int mulitplier = 1;

        foreach (char c in cellRef.ToCharArray().Reverse())
        {
            if (char.IsLetter(c))
            {
                columnIndex += mulitplier * ((int)c - 64);
                mulitplier = mulitplier * 26;
            }
        }

        return columnIndex;
    }
    public async Task<List<CustomerValidateDto>> Handle(ValidateCustomerImportQuery request, CancellationToken cancellationToken)
    {
        List<CustomerValidateDto> result = new List<CustomerValidateDto>();
        var getListBusiness = await _businessRepository.GetAll();
        var getListCustomerGroup = await _customerGroupRepository.GetAll();
        var getListEmployee = await _employeeRepository.GetAll();
        var getListCustomerSource = await _customerSourceRepository.GetAll();
        var getLisstGroupEmployee = await _groupEmployeeRepository.GetAll();
        try
        {
            using (MemoryStream stream = new MemoryStream())
            {
                request.File.CopyTo(stream);
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(request.SheetId)).Worksheet;
                    SheetData? thesheetdata = (SheetData?)theWorksheet?.GetFirstChild<SheetData>();

                    for (int i = (request.HeaderRow); thesheetdata is not null && i < thesheetdata.ChildElements.Count; i++)
                    {
                        Row thecurrentRow = (Row)thesheetdata.ChildElements[i];
                        CustomerValidateDto customerValidateDto = new CustomerValidateDto()
                        {
                            Row = thecurrentRow.RowIndex
                        };


                        foreach (ValidateField field in request.ListField)
                        {
                            if (field.IndexColumn >= 0 && field.IndexColumn < thecurrentRow.ChildElements.Count)
                            {
                                Cell thecurrentCell = thecurrentRow.Elements<Cell>().FirstOrDefault(cell => GetColumnIndex(cell.CellReference) == field.IndexColumn);
                                if (thecurrentCell != null)
                                {
                                    string cellValue = "";
                                    if (thecurrentCell.DataType != null && thecurrentCell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentCell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item != null && item.Text != null)
                                            {
                                                cellValue = item.Text.Text;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        cellValue = (thecurrentCell.CellValue == null) ? thecurrentCell.InnerText : thecurrentCell.CellValue.Text;
                                    }

                                    switch (field.Field)
                                    {
                                        case "code":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Errors.Add($"CustomerCode {cellValue} invalid");
                                            }
                                            else
                                            {
                                                customerValidateDto.Code = cellValue;
                                            }

                                            break;
                                        case "name":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Name = cellValue;
                                            }
                                            break;
                                        case "type":
                                            if (String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Errors.Add($"type {cellValue} invalid");
                                            }
                                            else
                                            {
                                                customerValidateDto.Type = string.Equals(cellValue.ToLower(), "cá nhân", StringComparison.OrdinalIgnoreCase) ? 0 : string.Equals(cellValue.ToLower(), "doanh nghiệp", StringComparison.OrdinalIgnoreCase) ? 1 : 3;
                                            }
                                            break;
                                        case "isVendor":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.IsVendor = string.Equals(cellValue.ToLower(), "1", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

                                            }
                                            break;
                                        case "day":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Day = cellValue;
                                            }
                                            break;
                                        case "month":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Month = cellValue;
                                            }
                                            break;
                                        case "year":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Year = cellValue;
                                            }
                                            break;
                                        case "gender":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Gender = string.Equals(cellValue.ToLower(), "nữ", StringComparison.OrdinalIgnoreCase) ? 1 : string.Equals(cellValue.ToLower(), "nam", StringComparison.OrdinalIgnoreCase) ? 0 : 3;
                                            }
                                            break;
                                        case "phone":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                string pattern = @"^0\d{8,9}$";
                                                Regex regex = new Regex(pattern);

                                                if (regex.IsMatch(cellValue.Trim()))
                                                {
                                                    customerValidateDto.Phone = cellValue.Trim();
                                                }
                                                else
                                                {
                                                    customerValidateDto.Phone = cellValue.Trim();
                                                    customerValidateDto.Errors.Add($"Phone {cellValue} malformed");

                                                }
                                            }
                                            break;
                                        case "email":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                                                Regex regex = new Regex(pattern);
                                                if (regex.IsMatch(cellValue.Trim()))
                                                {
                                                    customerValidateDto.Email = cellValue.Trim();
                                                }
                                                else
                                                {
                                                    customerValidateDto.Email = cellValue.Trim();
                                                    customerValidateDto.Errors.Add("WorkEmail malformed");
                                                }
                                            }
                                            break;
                                        case "taxCode":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.TaxCode = cellValue;
                                            }
                                            break;
                                        case "zipCode":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.ZipCode = cellValue;
                                            }
                                            break;
                                        case "fax":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Fax = cellValue;
                                            }
                                            break;
                                        case "website":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Website = cellValue;
                                            }
                                            break;
                                        case "businessSector":
                                            if (!String.IsNullOrEmpty(cellValue))

                                            {
                                                customerValidateDto.BusinessSector = cellValue;
                                            }
                                            break;
                                        case "companySize":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.CompanySize = cellValue;
                                            }
                                            break;
                                        case "capital":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Capital = cellValue;
                                            }
                                            break;
                                        case "country":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Country = cellValue.ToLower();
                                            }
                                            break;
                                        case "province":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Province = cellValue;
                                            }
                                            break;
                                        case "district":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.District = cellValue;
                                            }
                                            break;
                                        case "ward":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Ward = cellValue;
                                            }
                                            break;
                                        case "address":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.Address = cellValue;
                                            }
                                            break;
                                        case "idNumber":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.IdNumber = cellValue;
                                            }
                                            break;
                                        case "idDate":
                                            try
                                            {
                                                double excelDateValue;
                                                DateTime excelDate;
                                                if (!String.IsNullOrEmpty(cellValue))
                                                {
                                                    if (Double.TryParse(cellValue, out excelDateValue))
                                                    {
                                                        customerValidateDto.IdDate = DateTime.FromOADate(excelDateValue);
                                                    }
                                                    else if (DateTime.TryParseExact(cellValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out excelDate))
                                                    {
                                                        customerValidateDto.IdDate = excelDate;
                                                    }
                                                    else
                                                    {
                                                        customerValidateDto.Errors.Add($"Date range {cellValue} notincorrectformat");
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                customerValidateDto.Errors.Add($"Date range {cellValue} notincorrectformat");

                                            }
                                            break;
                                        case "idIssuer":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.IdIssuer = cellValue;
                                            }
                                            break;
                                        case "customerGroup":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.CustomerGroup = cellValue;
                                            }
                                            break;
                                        case "groupEmployeeName":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.GroupEmployeeName = cellValue;
                                            }
                                            break;
                                        case "employeeName":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.EmployeeName = cellValue;
                                            }
                                            break;
                                        case "customerSource":
                                            if (!String.IsNullOrEmpty(cellValue))
                                            {
                                                customerValidateDto.CustomerSource = cellValue;
                                            }
                                            break;


                                        case "note":
                                            customerValidateDto.Note = cellValue;

                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        //check trung linh vuc KD
                        if (!String.IsNullOrEmpty(customerValidateDto.BusinessSector))
                        {
                            var checkRB = getListBusiness.FirstOrDefault(e => e.Name.Trim().ToLower().Equals(customerValidateDto.BusinessSector.Trim().ToLower()));

                            if (checkRB is not null)
                            {
                                customerValidateDto.BusinessSector = checkRB.Name;
                                customerValidateDto.BusinessSectorId = checkRB.Id.ToString();
                            }
                            else
                            {
                                customerValidateDto.Errors.Add($"LineOfBusinessVT {customerValidateDto.BusinessSector} invalid");
                            }
                        }
                        //check nhom KH
                        if (!String.IsNullOrEmpty(customerValidateDto.CustomerGroup))
                        {
                            var dataCG = customerValidateDto.CustomerGroup.Trim().ToLower().Split(',').ToArray();
                            if (dataCG.Length > 0)
                            {
                                foreach (var item in dataCG)
                                {
                                    var checkRB = getListCustomerGroup.FirstOrDefault(e => e.Name.Trim().ToLower().Equals(item.Trim().ToLower()));

                                    if (checkRB is not null)
                                    {
                                        customerValidateDto.CustomerGroupNames.Add(checkRB.Name);
                                        customerValidateDto.CustomerGroupIds.Add(checkRB.Id);
                                    }
                                    else
                                    {
                                        customerValidateDto.Errors.Add($"CustomerGroup {customerValidateDto.CustomerGroup} invalid");
                                    }
                                }
                            }
                        }
                        // NVKD
                        if (!String.IsNullOrEmpty(customerValidateDto.EmployeeName))
                        {
                            var checkRB = getListEmployee.FirstOrDefault(e => e.Name.Trim().ToLower().Equals(customerValidateDto.EmployeeName.Trim().ToLower()));

                            if (checkRB is not null)
                            {
                                customerValidateDto.EmployeeName = checkRB.Name;
                                customerValidateDto.EmployeeId = checkRB.Id;
                            }
                            else
                            {
                                customerValidateDto.Errors.Add($"Employee {customerValidateDto.EmployeeName} invalid");
                            }
                        }
                        //nguồn KH
                        if (!String.IsNullOrEmpty(customerValidateDto.CustomerSource))
                        {

                            var checkRB = getListCustomerSource.FirstOrDefault(e => e.Name.Trim().ToLower().Equals(customerValidateDto.CustomerSource.Trim().ToLower()));

                            if (checkRB is not null)
                            {
                                customerValidateDto.CustomerSource = checkRB.Name;
                                customerValidateDto.CustomerSourceId = checkRB.Id;
                            }
                            else
                            {
                                customerValidateDto.Errors.Add($"CustomerSource {customerValidateDto.CustomerSource} invalid");
                            }
                        }
                        // nhóm bán hàng
                        if (!String.IsNullOrEmpty(customerValidateDto.GroupEmployeeName))
                        {
                            var checkRB = getLisstGroupEmployee.FirstOrDefault(e => e.Name.Trim().ToLower().Equals(customerValidateDto.GroupEmployeeName.Trim().ToLower()));

                            if (checkRB is not null)
                            {
                                customerValidateDto.GroupEmployeeName = checkRB.Name;
                                customerValidateDto.GroupEmployeeId = checkRB.Id;
                            }
                            else
                            {
                                customerValidateDto.Errors.Add($"GroupEmployeeName {customerValidateDto.GroupEmployeeName} invalid");
                            }
                        }
                        if (!String.IsNullOrEmpty(customerValidateDto.Code) || !String.IsNullOrEmpty(customerValidateDto.Name))
                        {
                            result.Add(customerValidateDto);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        //check code trùng
        var resultCode = result.Where(x => !string.IsNullOrEmpty(x.Code)).Select(x => x.Code);
        if (resultCode is not null && resultCode.Count() > 0)
        {
            var employees = await _customerRepository.Filter(resultCode);
            var listEmployeeError = result.Where(x => employees.Any(y => y.Equals(x.Code))).ToList();

            if (listEmployeeError != null && listEmployeeError.Count() > 0)
            {
                foreach (var i in listEmployeeError)
                {
                    var aa = result.Where(x => x.Code == i.Code?.Trim()).FirstOrDefault();
                    if (aa != null)
                    {
                        aa.Errors.Add($"{aa.Code} AlreadyExists");
                    }
                }
            }

        }
        //Nguon KH
        /*var resultCustomerGroup = result.Where(x => !string.IsNullOrEmpty(x.CustomerGroup)).Select(x => x.CustomerGroup.Split(",")).SelectMany(x => x).Select(y => y.Trim()).Select(y => y.ToLower()).Distinct();
        if (resultCustomerGroup is not null && resultCustomerGroup.Count() > 0)
        {
            var customerGroup = await _customerGroupRepository.Filter(resultCustomerGroup);
            var ei = customerGroup.Select(x => x.Name.Trim().ToLower());
            var listCustomerGroupError = result.Where(x => !ei.Contains(x?.CustomerGroup?.ToLower())).ToList();
            var listCustomerGroup = result.Where(x => ei.Contains(x?.CustomerGroup?.ToLower())).ToList();
            if (listCustomerGroup is not null && listCustomerGroup.Count() > 0)
            {
                foreach (var r in result)
                {
                    var data = r.CustomerGroup?.Trim().Split(",").Select(s => s.Trim()).ToArray();
                    if (data is not null)
                    {
                        foreach (var a in data)
                        {
                            var cg = customerGroup.Where(x => x.Name.Contains(a)).FirstOrDefault();
                            if (cg is not null)
                            {
                                r.CustomerGroupIds.Add(cg.Id);
                                r.CustomerGroupNames.Add(cg.Name);
                            }
                        }
                    }

                }
            }

            if (listCustomerGroupError != null && listCustomerGroupError.Count() > 0)
            {
                foreach (var i in listCustomerGroupError)
                {
                    var cg = result.Where(x => x.CustomerGroup != null && i.CustomerGroup != null && x.CustomerGroup.Contains(i.CustomerGroup?.Trim())).FirstOrDefault();
                    if (cg != null)
                    {
                        cg.Errors.Add($"CustomerGroup {cg.CustomerGroup} invalid");
                    }
                }
            }

        }*/
        List<Country> dataCountry = new List<Country>();
        List<Province> dataProvince = new List<Province>();
        List<District> dataDistrict = new List<District>();
        List<Ward> dataWard = new List<Ward>();
        // check địa chỉ 
        var resultCountry = result.Where(x => !string.IsNullOrEmpty(x.Country.ToLower())).Select(x => x.Country.ToLower()).Distinct() ?? new List<string>();
        if (resultCountry is not null && resultCountry.Count() > 0)
        {
            dataCountry = await _pimRepository.GetCountryByName(resultCountry.ToList());

            foreach (var itemCountry in result.Where(x => !String.IsNullOrEmpty(x.Country)))
            {
                var check = dataCountry.FirstOrDefault(x => x.Name.Trim().ToLower().Equals(itemCountry.Country.Trim().ToLower()));
                if (check is not null)
                {
                    itemCountry.Country = check.Name;
                    itemCountry.CountryId = check.Id;
                }
                else
                {
                    itemCountry.Errors.Add($"Country {itemCountry.Country} incorrect");
                }
            }

        }
        var resultProvince = result.Where(x => !string.IsNullOrEmpty(x.Province)).Select(x => x.Province).Distinct() ?? new List<string>();
        if (resultProvince is not null && resultProvince.Count() > 0)
        {
            dataProvince = await _pimRepository.GetProvinceByName(resultProvince.ToList());
            foreach (var itemStateProvince in result.Where(x => !String.IsNullOrEmpty(x.Province)))
            {
                var check = dataProvince.FirstOrDefault(x => x.Name.Trim().ToLower().Equals(itemStateProvince.Province.Trim().ToLower()));
                if (check is not null)
                {
                    if (check.CountryId.Equals(itemStateProvince.CountryId))
                    {
                        itemStateProvince.Province = check.Name;
                        itemStateProvince.StateProvinceId = check.Id;
                    }
                    else
                    {
                        itemStateProvince.Errors.Add($"Province {itemStateProvince.Province} incorrect");
                    }
                }
                else
                {
                    itemStateProvince.Errors.Add($"Province {itemStateProvince.Province} incorrect");
                }
            }
        }

        var resultDistrict = result.Where(x => !string.IsNullOrEmpty(x.District)).Select(x => x.District).Distinct() ?? new List<string>();
        if (resultDistrict is not null && resultDistrict.Count() > 0)
        {
            dataDistrict = await _pimRepository.GetDistrictByName(resultDistrict.ToList());
            foreach (var itemDistrict in result.Where(x => !String.IsNullOrEmpty(x.District)))
            {
                var check = dataDistrict.FirstOrDefault(x => x.Name.Trim().ToLower().Equals(itemDistrict.District.Trim().ToLower()));
                if (check is not null)
                {
                    if (check.StateProvinceId.Equals(itemDistrict.StateProvinceId))
                    {
                        itemDistrict.District = check.Name;
                        itemDistrict.DistrictId = check.Id;
                    }
                    else
                    {
                        itemDistrict.Errors.Add($"District {itemDistrict.District} incorrect");
                    }

                }
                else
                {
                    itemDistrict.Errors.Add($"District {itemDistrict.District} incorrect");
                }

            }
        }

        var resultWard = result.Where(x => !string.IsNullOrEmpty(x.Ward)).Select(x => x.Ward ?? "").Distinct().ToList() ?? new List<string>();
        if (resultWard.Count() > 0)
        {
            dataWard = await _pimRepository.GetWardByName(resultWard.ToList());
            foreach (var item in result.Where(x => !String.IsNullOrEmpty(x.Ward)))
            {
                var check = dataWard.FirstOrDefault(x => x.Name.Trim().ToLower().Equals(item.Ward.Trim().ToLower()));
                if (check is not null)
                {
                    if (check.DistrictId.Equals(item.DistrictId))
                    {
                        item.Ward = check.Name;
                        item.WardId = check.Id;
                    }
                    else
                    {
                        item.Errors.Add($"Ward {item.Ward} incorrect");
                    }
                }
                else
                {
                    item.Errors.Add($"Ward {item.Ward} incorrect");
                }

            }

        }
        return result;
    }
}
