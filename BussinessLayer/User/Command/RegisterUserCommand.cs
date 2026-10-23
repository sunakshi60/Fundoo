using ModelLayer.Model.User.UserRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public class RegisterUserCommand 
    {
        public RegistrationModel RegistrationModel { get; set; }

        public RegisterUserCommand(RegistrationModel registrationModel)
        {
            RegistrationModel = registrationModel;
        }
    }
}
