using ModelLayer.Model.User.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Command
{
    public interface IResetPasswordCommandHandler
    {
        ResetPasswordResponseModel Handle(ResetPasswordCommand command);
    }
}
