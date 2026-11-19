using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Label.Command
{
    public class RenameLabelCommand
    {
        public int LabelId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
    }
}
