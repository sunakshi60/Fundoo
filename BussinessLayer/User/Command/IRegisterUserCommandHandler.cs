using ModelLayer.Model.User.ResponseModel;
using ModelLayer.Model.User.UserRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public interface IRegisterUserCommandHandler
    {
        RegisterResponseModel<RegistrationModel> Handle(RegisterUserCommand command);
    }
}
