using BussinessLayer.Interface;
using BussinessLayer.Notes.Command;
using BussinessLayer.Notes.Query;
using ModelLayer.Model.Notes.NotesRequest;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;

namespace BussinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly IAddNoteCommandHandler _addNoteHandler;
        private readonly IViewNotesQueryHandler _viewNotesHandler;
        private readonly IViewNoteByIdQueryHandler _viewNoteByIdHandler;
        private readonly IEditNoteCommandHandler _editNoteHandler;
        private readonly IDeleteNoteCommandHandler _deleteNoteHandler;
        private readonly IArchiveNoteCommandHandler _archiveNoteHandler;

        public NoteBL(IAddNoteCommandHandler addNoteHandler,
            IViewNotesQueryHandler viewNotesHandler, IViewNoteByIdQueryHandler viewNoteByIdHandler,
            IEditNoteCommandHandler editNoteHandler, IDeleteNoteCommandHandler deleteNoteHandler,
            IArchiveNoteCommandHandler archiveNoteHandler)
        {
            _addNoteHandler = addNoteHandler;
            _viewNotesHandler = viewNotesHandler;
            _viewNoteByIdHandler = viewNoteByIdHandler;
            _editNoteHandler = editNoteHandler;
            _deleteNoteHandler = deleteNoteHandler;
            _archiveNoteHandler = archiveNoteHandler;
        }

        public async Task<NotesResponseModel<NoteEntity>> AddNoteBL(NoteCreationModel model, int userId)
        {
            var command = new AddNoteCommand(model, userId);

            return await _addNoteHandler.Handle(command);
        }

        public async Task<NotesResponseModel<List<NoteEntity>>> ViewNotesBL(int userId)
        {
            var query = new ViewNotesQuery(userId);

            return await _viewNotesHandler.Handle(query);
        }

        public async Task<NotesResponseModel<NoteEntity>> ViewNoteByIdBL(int noteId, int userId)
        {
            var query = new ViewNoteByIdQuery(noteId, userId);

            return await _viewNoteByIdHandler.Handle(query);
        }

        public async Task<NotesResponseModel<NoteEntity>> EditNoteBL(EditNotesModel model, int userId)
        {
            var command = new EditNoteCommand(model, userId);

            return await _editNoteHandler.Handle(command);
        }

        public async Task<NotesResponseModel<bool>> DeleteNoteBL(int noteId, int userId)
        {
            var command = new DeleteNoteCommand(noteId, userId);

            return await _deleteNoteHandler.Handle(command);
        }

        public async Task<NotesResponseModel<bool>> ArchUnarchivedBL(int noteId, int userId)
        {
            var command = new ArchiveNoteCommand(noteId, userId);

            return await _archiveNoteHandler.Handle(command);
        }
    }
}
