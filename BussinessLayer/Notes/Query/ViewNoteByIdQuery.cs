using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Query
{
    public class ViewNoteByIdQuery
    {
        public int NoteId { get; }
        public int UserId { get; }

        public ViewNoteByIdQuery(int noteId, int userId)
        {
            NoteId = noteId;
            UserId = userId;
        }
    }
}
