using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.User.RequestModel
{
    public class GoogleTokenModel
    {
        [Required]
        public string TokenId { get; set; } = string.Empty;
    }
}
