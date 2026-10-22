using ModelLayer.Model.User.UserRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Query
{
    public class LoginUserQuery
    {
        public LoginModel LoginModel { get; set; }

        public LoginUserQuery(LoginModel loginModel)
        {
            LoginModel = loginModel;
        }
    }
}
