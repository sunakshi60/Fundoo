using RepositoryLayer.Entity;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.User.UserRequest;
using ModelLayer.Model.User.ResponseModel;

namespace BussinessLayer.Interface
{
    public interface IUserBL
    {
        RegisterResponseModel<RegistrationModel> RegisterUserBL(RegistrationModel registrationModel);

        RegisterResponseModel<LoginResponseModel> LoginUserBL(LoginModel model);

        Task<ForgetPasswordResponseModel> ForgotPassword(ForgotPasswordModel model);

        ResetPasswordResponseModel ResetPassword(ResetPasswordModel model);
    }
}
