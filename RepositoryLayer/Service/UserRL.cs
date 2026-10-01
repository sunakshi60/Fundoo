using ModelLayer.Model;
using RepositoryLayer.Context;
using Microsoft.AspNetCore.Identity;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        FundooContext fundooContext;

        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public ResponseModel< RegistrationModel> RegisterUserRL(RegistrationModel registrationModel)
        {
            ResponseModel<RegistrationModel> response = new ResponseModel<RegistrationModel>();

            var existingUser = fundooContext.Users.FirstOrDefault(x => x.Email == registrationModel.email);

            if (existingUser != null)
            {
                response.IsSuccess = false;
                response.Message = "Email already registered";
                response.Data = null;

                return response;
            }

            UserEntity user = new UserEntity();

            user.UserId = registrationModel.userId;
            user.FirstName = registrationModel.firstName;
            user.LastName = registrationModel.lastName;
            user.Email = registrationModel.email;
            user.PhoneNumber = Convert.ToInt64(registrationModel.contactNo);

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();

            user.Password = passwordHasher.HashPassword(
                user,
                registrationModel.password
            );

            fundooContext.Users.Add(user);
            fundooContext.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Registration successful";

            response.Data = new RegistrationModel
            {
                userId = user.UserId,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                contactNo = user.PhoneNumber
            };

            return response;
        }

        public ResponseModel<LoginModel> LoginUserRL(LoginModel login)
        {
            ResponseModel<LoginModel> response = new ResponseModel<LoginModel>();

            var user = fundooContext.Users.FirstOrDefault(x => x.Email == login.email);

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Login failed due to invalid email or password.";
                response.Data = null;

                return response;
            }

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();

            var passwordResult = passwordHasher.VerifyHashedPassword(user, user.Password, login.password);

            if (passwordResult == PasswordVerificationResult.Success)
            {
                response.IsSuccess = true;
                response.Message = "Login Successful";
                response.Data = new LoginModel
                {
                    email = user.Email,
                    password = "hidden content"
                };
            }    
            else
            {
                response.IsSuccess = false;
                response.Message = "Login failed due to Invalid email or password";
                response.Data = null;
            }
            return response;
        }

        public List<UserEntity> GetAllUsersRL()
        {
            return fundooContext.Users.ToList();
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
            user.Password = passwordHasher.HashPassword(
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
    }
}
