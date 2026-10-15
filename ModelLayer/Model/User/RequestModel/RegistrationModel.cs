using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model.User.UserRequest
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string firstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2,ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string lastName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required")]
        [StringLength(128, MinimumLength = 12,
            ErrorMessage = "Password must be between 12 and 128 characters")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9\s])\S+$",
            ErrorMessage = "Password must contain an uppercase letter, lowercase letter, digit and special character, with no spaces")]
        public string password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Enter a valid 10-digit Indian mobile number")]
        public long contactNo { get; set; }
    }
}
