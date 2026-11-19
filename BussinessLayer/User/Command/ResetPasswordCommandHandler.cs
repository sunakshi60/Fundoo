using BussinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model.User.ResponseModel;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public class ResetPasswordCommandHandler : IResetPasswordCommandHandler
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRL _userRL;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(IJwtService jwtService, IUserRL userRL, ILogger<ResetPasswordCommandHandler> logger)
        {
            _jwtService = jwtService;
            _userRL = userRL;
            _logger = logger;
        }

        public ResetPasswordResponseModel Handle(ResetPasswordCommand command)
        {
            _logger.LogInformation("Password reset process started.");

            var model = command.Model;

            var principal = _jwtService.ValidateResetToken(model.Token);

            if (principal == null)
            {
                _logger.LogWarning("Password reset failed because the token is invalid or expired.");

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
                _logger.LogWarning("Password reset failed because token claims are invalid.");

                return new ResetPasswordResponseModel
                {
                    IsSuccess = false,
                    Message = "Invalid reset token claims."
                };
            }

            var user = _userRL.GetUserByEmail(emailClaim);

            if (user == null || user.UserId != userId)
            {
                _logger.LogWarning("Password reset failed because the token does not match a valid user.");

                return new ResetPasswordResponseModel
                {
                    IsSuccess = false,
                    Message = "Invalid reset token claims."
                };
            }

            var isReset = _userRL.UpdatePassword(
                userId,
                model.NewPassword);

            if (isReset)
            {
                _logger.LogInformation("Password reset completed successfully for UserId: {UserId}", userId);
            }
            else
            {
                _logger.LogWarning("Password reset operation failed for UserId: {UserId}", userId);
            }

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
