using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.User.ResponseModel
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
