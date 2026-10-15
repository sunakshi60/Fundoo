using ModelLayer.Model.Notes.NotesRequest;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
    public interface INoteBL
    {
        Task<NotesResponseModel<NoteEntity>> AddNoteBL(NoteCreationModel model, int userId);

        Task<NotesResponseModel<List<NoteEntity>>> ViewNotesBL(int userId);

        Task<NotesResponseModel<NoteEntity>> ViewNoteByIdBL(int noteId, int userId);

        Task<NotesResponseModel<NoteEntity>> EditNoteBL(EditNotesModel model, int userId);

        Task<NotesResponseModel<bool>> DeleteNoteBL(int noteId, int userId);

        Task<NotesResponseModel<bool>> ArchUnarchivedBL(int noteId, int userId);

    }
}
