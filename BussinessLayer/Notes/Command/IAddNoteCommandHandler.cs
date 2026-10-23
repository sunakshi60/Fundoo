using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;

namespace BussinessLayer.Notes.Command
{
    public interface IAddNoteCommandHandler
    {
        Task<NotesResponseModel<NoteEntity>> Handle(AddNoteCommand command);
    }
}
