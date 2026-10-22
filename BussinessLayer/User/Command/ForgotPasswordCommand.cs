using ModelLayer.Model.User.UserRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public class ForgotPasswordCommand
    {
        public ForgotPasswordModel Model { get; set; }

        public ForgotPasswordCommand(ForgotPasswordModel model)
        {
            Model = model;
        }
    }
}
