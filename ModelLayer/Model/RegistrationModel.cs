using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "UserId is required")]
        public int userId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string firstName { get; set; }


        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string lastName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Password must contain at least 6 characters")]
        public string password { get; set; }


        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Contact number must be a valid 10-digit mobile number")]
        public long contactNo { get; set; }
    }
}
