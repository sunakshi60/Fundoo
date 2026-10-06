using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        ResponseModel<RegistrationModel> RegisterUserRL(RegistrationModel registrationModel);
        UserEntity LoginUserRL(LoginModel login);

        List<UserEntity> GetAllUsersRL();

        UserEntity GetUserByIdRL(int userId);

        UserEntity UpdateUserRL(int userId, RegistrationModel registrationModel);

        bool DeleteUserRL(int userId);

        UserEntity GetUserByEmail(string email);

        bool SaveResetToken(string email, string token, DateTime expiry);

        bool ResetPassword(string token, string newPassword);
    }
}
