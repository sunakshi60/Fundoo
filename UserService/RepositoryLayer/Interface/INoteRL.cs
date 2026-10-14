using ModelLayer.Model.NotesRequest;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        Task<NoteEntity> AddNoteRL(NoteCreationModel model, int userId);

        Task<List<NoteEntity>> ViewNotesRL(int userId);

        Task<NoteEntity> ViewNoteByIdRL(int noteId,int userId);

        Task<NoteEntity> EditNoteRL(EditNotesModel model, int userId);

        Task<bool> DeleteNoteRL(int noteId, int userId);

        Task<bool> ArchUnarchivedRL(int noteId, int userId);

    }
}
