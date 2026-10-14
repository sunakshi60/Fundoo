using BussinessLayer.Interface;
using Microsoft.AspNetCore.Identity;
using ModelLayer.Model.UserRequest;
using ModelLayer.Model.UserResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IJwtService _jwtService;
        private readonly IMessageClient _messageClient;

        public UserBL(IUserRL _userRL, IJwtService jwtService, IMessageClient messageClient)
        {
            this._userRL = _userRL;
            _jwtService = jwtService;
            _messageClient = messageClient;
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

            var expiry = DateTime.UtcNow.AddMinutes(15);

            var saved = _userRL.SaveResetToken(
                model.Email,
                token,
                expiry
            );

            if (!saved)
            {
                return new ForgetPasswordResponseModel
                {
                    IsSuccess = false,
                    Message = "Unable to process the password reset request."
                };
            }

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

            await _messageClient.SendEmailAsync(
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
            var isReset = _userRL.ResetPassword(model);

            var response = new ResetPasswordResponseModel
            {
                IsSuccess = isReset,
                Message = isReset
                ? "Password reset successfully."
                : "Invalid or expired reset token."
            };

            return response;
        }
    }
}
