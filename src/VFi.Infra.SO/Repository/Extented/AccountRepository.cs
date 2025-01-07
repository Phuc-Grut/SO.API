
using FluentValidation.Results;
using Flurl.Http;
using VFi.Domain.SO.Interfaces;
using VFi.Domain.SO.Models;
using VFi.Domain.SO.Models;
using VFi.Infra.SO.Context;
using VFi.NetDevPack.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Infra.SO.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly MasterApiContext _apiMasterContext;
    private readonly IDApiContext _apiContext;
    private const string PATH_GET_USER_BY_USERNAME = "/api/user/get-by-username";
    private const string PATH_GET_USER_BY_ID = "/api/user/get-by-id/{0}";
    private const string PATH_GET_USER_BY_EMAIL = "/api/user/get-by-email";
    private const string PATH_GET_USER_EXIST_USERNAME = "/api/user/exist-username/{username}";
    private const string PATH_GET_USER_EXIST_EMAIL = "/api/user/exist-email/{email}";
    private const string PATH_USER_SET_PASSWORD = "/api/user/set-password";
    private const string PATH_USER_CHANGE_PASSWORD = "/api/user/change-password";
    private const string PATH_USER_EDIT_PICTURE = "/api/user/edit-picture";
    private const string PATH_USER_EDIT_PROFILE = "/api/user/edit-profile";
    private const string PATH_ADD_USER = "/api/user/add-simple";
    private const string PATH_SEND_EMAIL = "/api/notify/email";

    private const string PATH_GET_CUSTOMER_BY_ACCOUNT = "/api/customer/get-by-account/{0}";
    private const string PATH_GET_BID_BY_ACCOUNT = "/api/customer/get-bid-by-account/{0}";
    private const string PATH_SIGNUP_CUSTOMER = "/api/customer/signup";
    private const string PATH_EDIT_CUSTOMER = "/api/customer/edit-info-by-account";//
    private const string PATH_EDIT_IMAGE_CUSTOMER = "/api/customer/edit-image-by-account";//
    private const string PATH_DELETE_IMAGE_CUSTOMER = "/api/customer/delete-image-by-account/{0}";//
    private const string PATH_EDIT_IDENTITY_CUSTOMER = "/api/customer/edit-identity-by-account"; //

    private const string PATH_GET_CUSTOMER_ADDRESS_BY_ACCOUNT = "/api/customeraddress/get-by-accountid?accountid={0}";
    private const string PATH_ADD_CUSTOMER_ADDRESS_BY_ACCOUNT = "/api/customeraddress/add-by-account";//
    private const string PATH_EDIT_CUSTOMER_ADDRESS_BY_ACCOUNT = "/api/customeraddress/edit-by-account";//
    private const string PATH_DELETE_CUSTOMER_ADDRESS_BY_ACCOUNT = "/api/customeraddress/delete-by-account/{0}?accountid={1}";//
    private const string PATH_GET_TRANSACTION_BY_ACCOUNT = "/api/wallettransaction/get-by-account?account={0}&wallet={1}&keyword={2}&type={3}&size={4}&page={5}";
    private const string PATH_GET_WALLET_BY_ACCOUNT = "/api/wallet/get-by-account?accountid={0}&code={1}";

    private const string PATH_GET_CUSTOMER_BANK_BY_ACCOUNT = "/api/customerbank/get-by-accountid?accountid={0}";
    private const string PATH_ADD_CUSTOMER_BANK_BY_ACCOUNT = "/api/customerbank/add-by-account";//
    private const string PATH_EDIT_CUSTOMER_BANK_BY_ACCOUNT = "/api/customerbank/edit-by-account";//
    private const string PATH_DELETE_CUSTOMER_BANK_BY_ACCOUNT = "/api/customerbank/delete-by-account/{0}?accountid={1}";//
    private const string PATH_ACTIVE_BID = "/api/customer/active-bid-hold-by-account";//
    private const string PATH_DEACTIVE_BID = "/api/customer/deactive-bid-hold-by-account";//
    private const string PATH_PRICE_PUCHASE_BY_ACCOUNT_ID = "/api/customer/price-puchase-by-account-id";

    private const string PATH_GET_MY_INFO_BY_ACCOUNT_ID = "/api/customer/get-my-info?accountid={0}";
    private const string PATH_GET_TOP_PAYMENT = "/api/paymentinvoice/top-by-accountid?accountid={0}";


    public AccountRepository(IDApiContext apiContext, MasterApiContext apiMasterContext)
    {
        _apiContext = apiContext;
        _apiMasterContext = apiMasterContext;
    }
    public async Task<bool> SendEmail(EmailNotify emailNotify)
    {
        _ = await _apiMasterContext.Client.Request(PATH_SEND_EMAIL)
            .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json").PostJsonAsync(emailNotify);
        //.ReceiveJson<dynamic>();
        return true;
    }

    public bool ForgotPassword(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUser(string username)
    {
        var item = await _apiContext.Client.Request(PATH_GET_USER_BY_USERNAME + "/" + username)
                            .GetJsonAsync<User>();
        return item;
    }
    public async Task<User> GetUserById(string userId)
    {
        var item = await _apiContext.Client.Request(string.Format(PATH_GET_USER_BY_ID, userId))
                            .GetJsonAsync<User>();
        return item;
    }
    public async Task<User> GetUserByEmail(string email)
    {
        try
        {
            var item = await _apiContext.Client.Request(PATH_GET_USER_BY_EMAIL + "/" + email)
                               .GetJsonAsync<User>();
            return item;
        }
        catch (Exception)
        {

            return null;
        }
    }


    public bool IsExist(string username)
    {
        dynamic t = _apiContext.Client.Request(PATH_GET_USER_BY_USERNAME + "/" + username).AllowAnyHttpStatus()
                             .GetJsonAsync<User>().Result;

        if (t.ErrorMessage != null && t.ErrorMessage.Equals("Username not exist"))
            return false;
        else
        {
            return true;
        }
    }
    public async Task<bool> SetPassword(string userId, string password)
    {
        try
        {
            var result = await _apiContext.Client.Request(PATH_USER_SET_PASSWORD)
              .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
              .PutJsonAsync(new
              {
                  UserId = userId,
                  Password = password
              })
            .ReceiveJson<ValidationResult>();
            return result.IsValid;
        }
        catch (Exception)
        {

            throw;
        }
    }
    public async Task<ValidationResult> ChangePassword(string username, string password, string newpassword)
    {
        try
        {
            var result = await _apiContext.Client.Request(PATH_USER_CHANGE_PASSWORD)
              .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
              .PutJsonAsync(new
              {
                  Username = username,
                  Password = password,
                  NewPassword = newpassword
              })
            .ReceiveJson<ValidationResult>();
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public bool IsExistEmail(string email)
    {
        dynamic t = _apiContext.Client.Request($"{PATH_GET_USER_BY_EMAIL}/{email}").AllowAnyHttpStatus()
        .GetJsonAsync().Result;

        try
        {
            if (t.isValid != null && t.isValid is bool? && t.isValid == false)
                return false;
            else
            {
                return true;
            }
        }
        catch
        {
            if (t == null || t?.email == null || t?.email != email)
                return false;
            else
            {
                return true;
            }
        }
    }
    public async Task<dynamic> Signup(string name, string email, string phone, string password)
    {
        dynamic result = await _apiContext.Client.Request(PATH_ADD_USER)
          .WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
          .PostJsonAsync(new
          {
              Name = name,
              Email = email,
              Phone = phone,
              Password = password,
              Enabled = true,
              EmailVerified = true,
              PhoneVerified = false
          })
        .ReceiveJson<dynamic>();
        return result;
    }

    public Task<bool> ValidPassword(string email, string password)
    {
        throw new NotImplementedException();
    }



}
