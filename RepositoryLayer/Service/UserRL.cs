using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ModelLayer.Model.User.ResponseModel;
using ModelLayer.Model.User.UserRequest;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Middleware.Security;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        private readonly CustomPasswordHasher _passwordHasher;

        public UserRL(FundooContext fundooContext, CustomPasswordHasher passwordHasher)
        {
            this.fundooContext = fundooContext;
            _passwordHasher = passwordHasher;
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

            user.Password = _passwordHasher.HashPassword(registrationModel.password);


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

            bool isValid = _passwordHasher.VerifyPassword(model.password, user.Password);

            if (!isValid)
            {
                return null;
            }

            return user;
        }

        public UserEntity GetUserByEmail(string email)
        {
            return fundooContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            var user = fundooContext.Users
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return false;

            user.Password = _passwordHasher.HashPassword(newPassword);

            fundooContext.SaveChanges();

            return true;
        }
    }
}
