using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Label.Command
{
    public class AttachLabelToNoteCommand
    {
        public int NoteId { get; set; }

        public int LabelId { get; set; }

        public int UserId { get; set; }
    }
}
