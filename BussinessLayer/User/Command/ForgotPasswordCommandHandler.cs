using BussinessLayer.Interface;
using Microsoft.Extensions.Logging;
using Middleware.Services.Email;
using ModelLayer.Model.User.ResponseModel;
using RepositoryLayer.Interface;

namespace BussinessLayer.User.Command
{
    public class ForgotPasswordCommandHandler : IForgotPasswordCommandHandler
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ILogger<ForgotPasswordCommandHandler> _logger;

        public ForgotPasswordCommandHandler(IUserRL userRL,IJwtService jwtService,IEmailService emailService, ILogger<ForgotPasswordCommandHandler> logger)
        {
            _userRL = userRL;
            _jwtService = jwtService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<ForgetPasswordResponseModel> Handle(ForgotPasswordCommand command)
        {
            _logger.LogInformation("Forgot password process started.");

            var model = command.Model;

            var user = _userRL.GetUserByEmail(model.Email);

            if (user == null)
            {
                _logger.LogInformation("Password reset requested for an unregistered email.");

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
                <p>Hello {System.Net.WebUtility.HtmlEncode(user.FirstName)},</p>
                <p>Your password reset token is:</p>
                <p><strong>{System.Net.WebUtility.HtmlEncode(token)}</strong></p>
                <p>This token is valid for 15 minutes.</p>
                <p>If you did not request a password reset, you can ignore this email.</p>
            ";

            await _emailService.SendEmailAsync(
                user.Email,
                subject,
                body
            );

            _logger.LogInformation("Password reset email sent successfully.");

            return new ForgetPasswordResponseModel
            {
                IsSuccess = true,
                Message = "If an account exists for this email, a password reset email has been sent."
            };
        }
    }
}

