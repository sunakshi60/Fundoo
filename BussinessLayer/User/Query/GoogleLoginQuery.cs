using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.User.Query
{
    public class GoogleLoginQuery
    {
        public string Email { get; }
        public string GoogleId { get; }

        public GoogleLoginQuery(string email, string googleId)
        {
            Email = email;
            GoogleId = googleId;
        }
    }
}
