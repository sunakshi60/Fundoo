using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class NoteLabelEntity
    {
        public int NoteId { get; set; }

        public int LabelId { get; set; }

        [ForeignKey("NoteId")]
        public NoteEntity Note { get; set; } = null!;

        [ForeignKey("LabelId")]
        public LabelEntity Label { get; set; } = null!;
    }
}
