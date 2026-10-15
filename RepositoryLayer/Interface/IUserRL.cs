using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.User.ResponseModel;
using ModelLayer.Model.User.UserRequest;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        RegisterResponseModel<RegistrationModel> RegisterUserRL(RegistrationModel registrationModel);

        UserEntity LoginUserRL(LoginModel login);

        UserEntity GetUserByEmail(string email);

        bool UpdatePassword(int userId, string newPassword);

    }
}
