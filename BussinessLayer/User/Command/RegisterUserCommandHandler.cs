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
    public class RegisterUserCommandHandler
    {
        private readonly IUserRL _userRL;

        public RegisterUserCommandHandler(IUserRL userRL)
        {
            _userRL = userRL;
        }

        public RegisterResponseModel<RegistrationModel> Handle(RegisterUserCommand command)
        {
            return _userRL.RegisterUserRL(command.RegistrationModel);
        }
    }
}