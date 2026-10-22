using ModelLayer.Model.Notes.NotesRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Command
{
    public class AddNoteCommand
    {
        public NoteCreationModel Model { get; }
        public int UserId { get; }

        public AddNoteCommand(NoteCreationModel model, int userId)
        {
            Model = model;
            UserId = userId;
        }
    }
}
