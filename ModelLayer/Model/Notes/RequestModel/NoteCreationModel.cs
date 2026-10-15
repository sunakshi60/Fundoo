using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.Notes.NotesRequest
{
    public class NoteCreationModel
    {
        [Required]
        [StringLength(200,MinimumLength =1)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
    }
}
