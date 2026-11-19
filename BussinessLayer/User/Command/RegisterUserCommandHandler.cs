using Microsoft.Extensions.Logging;
using ModelLayer.Model.User.ResponseModel;
using ModelLayer.Model.User.UserRequest;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public class RegisterUserCommandHandler : IRegisterUserCommandHandler
    {
        private readonly IUserRL _userRL;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IUserRL userRL, ILogger<RegisterUserCommandHandler> logger)
        {
            _userRL = userRL;
            _logger = logger;
        }

        public RegisterResponseModel<RegistrationModel> Handle(RegisterUserCommand command)
        {
            _logger.LogInformation("User registration process completed.");

            try
            {
                var response = _userRL.RegisterUserRL(command.RegistrationModel);

                _logger.LogInformation("User registration process completed.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred during user registration.");

                throw;
            }
        }
    }
}