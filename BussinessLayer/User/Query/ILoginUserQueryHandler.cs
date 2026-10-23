using ModelLayer.Model.User.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Query
{
    public interface ILoginUserQueryHandler
    {
        RegisterResponseModel<LoginResponseModel> Handle(LoginUserQuery query);
    }
}
