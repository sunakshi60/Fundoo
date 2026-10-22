using BussinessLayer.Interface;
using BussinessLayer.User.Command;
using BussinessLayer.User.Query;
using Middleware.Security;
using Middleware.Services.Email;
using ModelLayer.Model.User.ResponseModel;
using ModelLayer.Model.User.UserRequest;
using RepositoryLayer.Interface;

namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly CustomPasswordHasher _passwordHasher;
        private readonly RegisterUserCommandHandler _registerUserHandler;
        private readonly LoginUserQueryHandler _loginUserHandler;
        private readonly ForgotPasswordCommandHandler _forgotPasswordHandler;
        private readonly ResetPasswordCommandHandler _resetPasswordHandler;


        public UserBL(IUserRL _userRL, IJwtService jwtService, IEmailService emailService,
                      CustomPasswordHasher passwordHasher, RegisterUserCommandHandler registerUserHandler,
                      LoginUserQueryHandler loginUserHandler, ForgotPasswordCommandHandler forgotPasswordHandler,
                      ResetPasswordCommandHandler resetPasswordHandler)
        {
            this._userRL = _userRL;
            _jwtService = jwtService;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
            _registerUserHandler = registerUserHandler;
            _loginUserHandler = loginUserHandler;
            _forgotPasswordHandler = forgotPasswordHandler;
            _resetPasswordHandler = resetPasswordHandler;
        }

        public RegisterResponseModel<RegistrationModel> RegisterUserBL(RegistrationModel registrationModel)
        {
            var command = new RegisterUserCommand(registrationModel);

            return _registerUserHandler.Handle(command);
        }

        public RegisterResponseModel<LoginResponseModel> LoginUserBL(LoginModel model)
        {
            var query = new LoginUserQuery(model);

            return _loginUserHandler.Handle(query);
        }

        public async Task<ForgetPasswordResponseModel> ForgotPassword(ForgotPasswordModel model)
        {
            var command = new ForgotPasswordCommand(model);

            return await _forgotPasswordHandler.Handle(command);
        }

        public ResetPasswordResponseModel ResetPassword(ResetPasswordModel model)
        {
            var command = new ResetPasswordCommand(model);

            return _resetPasswordHandler.Handle(command);
        }
    }
}
