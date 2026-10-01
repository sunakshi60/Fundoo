using BussinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;

        public UserBL(IUserRL _userRL)
        {
            this._userRL = _userRL;
        }

        public ResponseModel <RegistrationModel> RegisterUserBL(RegistrationModel registrationModel)
        {
            return _userRL.RegisterUserRL(registrationModel);
        }

        public ResponseModel<LoginModel>LoginUserBL(LoginModel login)
        {
            return _userRL.LoginUserRL(login);
        }

        public List<UserEntity> GetAllUsersBL()
        {
            return _userRL.GetAllUsersRL();
        }

        public UserEntity GetUserByIdBL(int userId)
        {
            return _userRL.GetUserByIdRL(userId);
        }

        public UserEntity UpdateUserBL(int userId, RegistrationModel registrationModel)
        {
            return _userRL.UpdateUserRL(userId, registrationModel);
        }

        public bool DeleteUserBL(int userId)
        {
            return _userRL.DeleteUserRL(userId);
        }
    }
}
