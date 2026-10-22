using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Command
{
    public class ArchiveNoteCommand
    {
        public int NoteId { get; }
        public int UserId { get; }

        public ArchiveNoteCommand(int noteId, int userId)
        {
            NoteId = noteId;
            UserId = userId;
        }
    }
}
