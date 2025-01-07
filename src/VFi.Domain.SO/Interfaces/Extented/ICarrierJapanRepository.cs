namespace VFi.Domain.SO.Interfaces.Extented;

public interface ICarrierJapanRepository
{
    Task<string> GetJapanPost(string tracking);
    Task<string> GetSagawa(string tracking);
    Task<string> GetYamato(string tracking);
}
