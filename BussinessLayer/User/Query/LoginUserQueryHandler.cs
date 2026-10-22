using BussinessLayer.Interface;
using ModelLayer.Model.User.ResponseModel;
using RepositoryLayer.Interface;

namespace BussinessLayer.User.Query
{
    public class LoginUserQueryHandler
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;

        public LoginUserQueryHandler(IUserRL userRL,IJwtService jwtService)
        {
            _userRL = userRL;
            _jwtService = jwtService;
        }

        public RegisterResponseModel<LoginResponseModel> Handle(LoginUserQuery query)
        {
            var user = _userRL.LoginUserRL(query.LoginModel);

            if (user == null)
            {
                return new RegisterResponseModel<LoginResponseModel>
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

            return new RegisterResponseModel<LoginResponseModel>
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
    }
}
