using BussinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model.User.ResponseModel;
using RepositoryLayer.Interface;

namespace BussinessLayer.User.Query
{
    public class LoginUserQueryHandler : ILoginUserQueryHandler
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginUserQueryHandler> _logger;

        public LoginUserQueryHandler(IUserRL userRL,IJwtService jwtService, ILogger<LoginUserQueryHandler> logger)
        {
            _userRL = userRL;
            _jwtService = jwtService;
            _logger = logger;
        }

        public RegisterResponseModel<LoginResponseModel> Handle(LoginUserQuery query)
        {
            _logger.LogInformation("Login process started.");

            var user = _userRL.LoginUserRL(query.LoginModel);

            if (user == null)
            {
                _logger.LogWarning("Login failed due to invalid credentials.");

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

            _logger.LogInformation(
                "Login successful for UserId: {UserId}",
                user.UserId);

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
