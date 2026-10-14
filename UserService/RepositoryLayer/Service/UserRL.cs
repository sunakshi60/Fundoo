using RepositoryLayer.Context;
using Microsoft.AspNetCore.Identity;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.UserRequest;
using ModelLayer.Model.UserResponse;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        FundooContext fundooContext;
        private readonly PasswordHasher<UserEntity> _passwordHasher;

        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
            _passwordHasher = new PasswordHasher<UserEntity>();
        }

        public RegisterResponseModel< RegistrationModel> RegisterUserRL(RegistrationModel registrationModel)
        {
            RegisterResponseModel<RegistrationModel> response = new RegisterResponseModel<RegistrationModel>();

            var existingUser = fundooContext.Users.FirstOrDefault(x => x.Email == registrationModel.email);

            if (existingUser != null)
            {
                response.IsSuccess = false;
                response.Message = "Email already registered";
                response.Data = null;

                return response;
            }

            UserEntity user = new UserEntity();

            user.FirstName = registrationModel.firstName;
            user.LastName = registrationModel.lastName;
            user.Email = registrationModel.email;
            user.PhoneNumber = Convert.ToInt64(registrationModel.contactNo);

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();

            user.Password = _passwordHasher.HashPassword(
                user,
                registrationModel.password
            );

            fundooContext.Users.Add(user);
            fundooContext.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Registration successful";

            response.Data = new RegistrationModel
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                contactNo = user.PhoneNumber
            };

            return response;
        }

        public UserEntity LoginUserRL(LoginModel model)
        {
            var user = fundooContext.Users.FirstOrDefault(x => x.Email == model.email);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                model.password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }


        public UserEntity GetUserByIdRL(int userId)
        {
            return fundooContext.Users.FirstOrDefault(x => x.UserId == userId);
        }

        public UserEntity UpdateUserRL(int userId, RegistrationModel registrationModel)
        {
            var user = fundooContext.Users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                return null;
            }

            user.FirstName = registrationModel.firstName;
            user.LastName = registrationModel.lastName;
            user.Email = registrationModel.email;
            user.PhoneNumber = Convert.ToInt64(registrationModel.contactNo);

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
            user.Password = _passwordHasher.HashPassword(
                user,
                registrationModel.password
            );

            fundooContext.SaveChanges();

            return user;
        }

        public bool DeleteUserRL(int userId)
        {
            var user = fundooContext.Users
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
            {
                return false;
            }

            fundooContext.Users.Remove(user);

            fundooContext.SaveChanges();

            return true;
        }

        public UserEntity GetUserByEmail(string email)
        {
            return fundooContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public bool SaveResetToken(string email, string token, DateTime expiry)
        {
            var user = fundooContext.Users
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
                return false;

            user.ResetToken = token;
            user.ResetTokenExpiry = expiry;

            fundooContext.SaveChanges();

            return true;
        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            var user = fundooContext.Users
                .FirstOrDefault(x =>
                    x.ResetToken == model.Token &&
                    x.ResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                return false;
            }

            user.Password = _passwordHasher.HashPassword(
                    user,
                    model.NewPassword);

            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            fundooContext.SaveChanges();

            return true;
        }
    }
}
