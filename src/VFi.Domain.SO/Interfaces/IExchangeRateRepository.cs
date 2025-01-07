namespace VFi.Domain.SO.Interfaces;

public interface IExchangeRateRepository
{
    Task<double> GetRate(string currency);
}
