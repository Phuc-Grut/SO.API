using FluentValidation.Results;
using VFi.Domain.SO.Models;
using VFi.NetDevPack.Models;

namespace VFi.Domain.SO.Interfaces;

public interface IAccountRepository
{
    Task<bool> SendEmail(EmailNotify emailNotify);
    Task<User> GetUser(string username);
    Task<User> GetUserById(string userid);
    Task<User> GetUserByEmail(string email);
    bool IsExist(string userName);
    bool IsExistEmail(string email);
    Task<dynamic> Signup(string name, string email, string phone, string password);
    Task<ValidationResult> ChangePassword(string username, string password, string newpassword);
    Task<bool> ValidPassword(string username, string password);
    Task<bool> SetPassword(string userId, string password);

}
