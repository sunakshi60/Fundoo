using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.Notes.NotesRequest
{
    public class NotesIdModel
    {
        [Required]
        public int NoteId { get; set; }
    }
}
