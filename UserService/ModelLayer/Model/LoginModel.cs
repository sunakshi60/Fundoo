using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter a valid email address")]
        [EmailAddress]
        public string email { get; set; }


        [Required(ErrorMessage = "Please enter a valid password")]
        public string password { get; set; }
    }
}
