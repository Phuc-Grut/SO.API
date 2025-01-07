namespace VFi.Api.HRM.ViewModels;

public class ApiRpesponseModel
{
    /// <summary>
    /// Mã trả lời
    /// false là yêu cầu không được chấp nhận xử lý
    /// true là xử lý yêu cầu thành công
    /// </summary>
    public Boolean IsValid { get; set; } = false;

    /// <summary>
    /// Nội dung thông báo tương ứng với Code hoặc nội dung ngoại lệ xử lý
    /// </summary>
    public string Message { get; set; } = "Yêu cầu không được xử lý.";

    /// <summary>
    /// Dữ liệu trả lại, dạng kiểu đối tượng động. Với  mỗi api sẽ trả về các đối tượng dữ liệu khác nhau tương ứng
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Dữ liệu trả lại, lỗi của đói tượng
    /// </summary>
    public object? Errors { get; set; }
}
