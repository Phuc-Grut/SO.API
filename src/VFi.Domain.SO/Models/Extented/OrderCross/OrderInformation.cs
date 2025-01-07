namespace VFi.Domain.SO.Models.Extented.OrderCross;
public class OrderInformation
{
    private DateTime? _paymentExpiryDate;

    public string Code { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? PaymentExpiryDate
    {
        get
        {
            if (_paymentExpiryDate.HasValue)
            {
                return _paymentExpiryDate;
            }
            if (CreatedDate.HasValue)
            {
                return CreatedDate.Value.AddDays(2);
            }
            return null;
        }
        set => _paymentExpiryDate = value;
    }
}
