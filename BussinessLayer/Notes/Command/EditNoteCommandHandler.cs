using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Command
{
    public class EditNoteCommandHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;

        public EditNoteCommandHandler(INoteRL noteRL, ICacheService cache)
        {
            _noteRL = noteRL;
            _cache = cache;
        }

        public async Task<NotesResponseModel<NoteEntity>> Handle(EditNoteCommand command)
        {
            var note = await _noteRL.EditNoteRL(command.Model, command.UserId);

            if (note == null)
            {
                return new NotesResponseModel<NoteEntity>
                {
                    IsSuccess = false,
                    Message = "Note not found or could not be updated",
                    Data = null
                };
            }

            // Invalidate both list and individual-note caches
            await _cache.RemoveAsync($"user:{command.UserId}:notes");

            await _cache.RemoveAsync($"user:{command.UserId}:note:{note.NoteId}");

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note updated successfully",
                Data = note
            };
        }
    }
}
