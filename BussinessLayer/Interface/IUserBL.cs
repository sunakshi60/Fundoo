using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
    public interface IUserBL
    {
        ResponseModel<RegistrationModel> RegisterUserBL(RegistrationModel registrationModel);

        ResponseModel<LoginResponseModel> LoginUserBL(LoginModel model);

        List<UserEntity> GetAllUsersBL();

        UserEntity GetUserByIdBL(int userId);

        UserEntity UpdateUserBL(int userId, RegistrationModel registrationModel);

        bool DeleteUserBL(int userId);

    }
}
