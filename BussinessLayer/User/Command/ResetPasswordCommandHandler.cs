using BussinessLayer.Interface;
using ModelLayer.Model.User.ResponseModel;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public class ResetPasswordCommandHandler
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRL _userRL;

        public ResetPasswordCommandHandler(IJwtService jwtService, IUserRL userRL)
        {
            _jwtService = jwtService;
            _userRL = userRL;
        }

        public ResetPasswordResponseModel Handle(ResetPasswordCommand command)
        {
            var model = command.Model;

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

            var user = _userRL.GetUserByEmail(emailClaim);

            if (user == null || user.UserId != userId)
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
