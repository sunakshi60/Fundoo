using ModelLayer.Model.Notes.NotesRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Command
{
    public class EditNoteCommand
    {
        public EditNotesModel Model { get; }
        public int UserId { get; }

        public EditNoteCommand(EditNotesModel model, int userId)
        {
            Model = model;
            UserId = userId;
        }
    }
}
