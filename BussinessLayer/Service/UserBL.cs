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
        private readonly IJwtService _jwtService;

        public UserBL(IUserRL _userRL, IJwtService jwtService)
        {
            this._userRL = _userRL;
            _jwtService = jwtService;
        }

        public ResponseModel <RegistrationModel> RegisterUserBL(RegistrationModel registrationModel)
        {
            return _userRL.RegisterUserRL(registrationModel);
        }

        public ResponseModel<LoginResponseModel> LoginUserBL(LoginModel model)
        {
            var user = _userRL.LoginUserRL(model);

            if (user == null)
            {
                return new ResponseModel<LoginResponseModel>
                {
                    IsSuccess = false,
                    Message = "Invalid email or password",
                    Data = null
                };
            }

            var token = _jwtService.GenerateToken(
                user.UserId,
                user.Email
            );

            return new ResponseModel<LoginResponseModel>
            {
                IsSuccess = true,
                Message = "Login successful",
                Data = new LoginResponseModel
                {
                    Token = token,
                    UserId = user.UserId,
                    Email = user.Email
                }
            };
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
