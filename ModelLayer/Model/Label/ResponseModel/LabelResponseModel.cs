using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.Label.ResponseModel
{
    public class LabelResponseModel
    {
        public int LabelId { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
