using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.Notes.NotesRequest
{
    public class EditNotesModel
    {
        [Required]
        public int NoteId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Colour { get; set; } = string.Empty;
    }
}
