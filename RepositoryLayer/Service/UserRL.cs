using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ModelLayer.Model.User.ResponseModel;
using ModelLayer.Model.User.UserRequest;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
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

            user.Password = _passwordHasher.HashPassword(
                user,
                newPassword);

            fundooContext.SaveChanges();

            return true;
        }
    }
}
