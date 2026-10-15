using BussinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Middleware.Services.Email;
using ModelLayer.Model.User.UserRequest;
using ModelLayer.Model.User.ResponseModel;


namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public UserBL(IUserRL _userRL, IJwtService jwtService, IEmailService emailService)
        {
            this._userRL = _userRL;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public RegisterResponseModel<RegistrationModel> RegisterUserBL(RegistrationModel registrationModel)
        {
            return _userRL.RegisterUserRL(registrationModel);
        }

        public RegisterResponseModel<LoginResponseModel> LoginUserBL(LoginModel model)
        {
            var user = _userRL.LoginUserRL(model);

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

        public async Task<ForgetPasswordResponseModel> ForgotPassword(ForgotPasswordModel model)
        {
            var user = _userRL.GetUserByEmail(model.Email);

            if (user == null)
            {
                return new ForgetPasswordResponseModel
                {
                    IsSuccess = true,
                    Message = "If an account exists for this email, a password reset email will be sent."
                };
            }

            var token = _jwtService.GenerateResetToken(
                user.UserId,
                user.Email
            );

            var subject = "Fundoo Password Reset";

            var body = $@"
                <h2>Password Reset Request</h2>
                <p>Hello {user.FirstName},Your password reset token is:</p>

                <p>
                <strong>{token}</strong>
                </p>

                <p>This token is valid for 15 minutes to authorize.</p>
                <p>If you did not request a password reset, you can ignore this email.</p>      
            ";

            await _emailService.SendEmailAsync(
                user.Email,
                subject,
                body
            );

            return new ForgetPasswordResponseModel
            {
                IsSuccess = true,
                Message = "If an account exists for this email, a password reset email has been sent."
            };           
        }

        public ResetPasswordResponseModel ResetPassword(ResetPasswordModel model)
        {
            var principal = _jwtService.ValidateResetToken(model.Token);

            if (principal == null)
            {
                return new ResetPasswordResponseModel
                {
                    IsSuccess = false,
                    Message = "Invalid or expired reset token."
                };
            }

            var userIdClaim = principal.FindFirst("UserId")?.Value;
            var emailClaim = principal.FindFirst("Email")?.Value;

            if (!int.TryParse(userIdClaim, out var userId) ||
                string.IsNullOrWhiteSpace(emailClaim))
            {
                return new ResetPasswordResponseModel
                {
                    IsSuccess = false,
                    Message = "Invalid reset token claims."
                };
            }

            var isReset = _userRL.UpdatePassword(
                userId,
                model.NewPassword);

            return new ResetPasswordResponseModel
            {
                IsSuccess = isReset,
                Message = isReset
                    ? "Password reset successfully."
                    : "Unable to reset password."
            };
        }
    }
}
