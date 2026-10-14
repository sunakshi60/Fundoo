using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model.UserRequest
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "First name must be between 2 and 50 characters")]
        [RegularExpression(@"^[A-Z][a-z]{1,49}$",
            ErrorMessage = "First name must start with a capital letter followed by lowercase letters")]
        public string firstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Last name must be between 2 and 50 characters")]
        [RegularExpression(@"^[A-Za-z]+(?:[ '-][A-Za-z]+)*$",
            ErrorMessage = "Last name can contain only letters, spaces, apostrophes or hyphens")]
        public string lastName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email is required")]
        [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(
            @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$",
            ErrorMessage = "Email must be lowercase and have a valid domain, such as example@gmail.com")]
        public string email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required")]
        [StringLength(128, MinimumLength = 12,
            ErrorMessage = "Password must be between 12 and 128 characters")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9\s])\S+$",
            ErrorMessage = "Password must contain an uppercase letter, lowercase letter, digit and special character, with no spaces")]
        public string password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^[6-9][0-9]{9}$",
            ErrorMessage = "Contact number must be exactly 10 digits and start with 6, 7, 8 or 9")]
        public long contactNo { get; set; }
    }
}
