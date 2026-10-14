using RepositoryLayer.Entity;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.UserResponse;
using ModelLayer.Model.UserRequest;

namespace BussinessLayer.Interface
{
    public interface IUserBL
    {
        RegisterResponseModel<RegistrationModel> RegisterUserBL(RegistrationModel registrationModel);

        RegisterResponseModel<LoginResponseModel> LoginUserBL(LoginModel model);

        UserEntity GetUserByIdBL(int userId);

        UserEntity UpdateUserBL(int userId, RegistrationModel registrationModel);

        bool DeleteUserBL(int userId);

        Task<ForgetPasswordResponseModel> ForgotPassword(ForgotPasswordModel model);

        ResetPasswordResponseModel ResetPassword(ResetPasswordModel model);
    }
}
