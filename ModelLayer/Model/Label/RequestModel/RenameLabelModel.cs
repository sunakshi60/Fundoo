using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.Label.RequestModel
{
    public class RenameLabelModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
    }
}
