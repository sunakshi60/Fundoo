using ModelLayer.Model.User.UserRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public class ResetPasswordCommand
    {
        public ResetPasswordModel Model { get; set; }

        public ResetPasswordCommand(ResetPasswordModel model)
        {
            Model = model;
        }
    }
}
