using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Command
{
    public class EditNoteCommandHandler : IEditNoteCommandHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;
        private readonly ILogger<EditNoteCommandHandler> _logger;

        public EditNoteCommandHandler(INoteRL noteRL, ICacheService cache, ILogger<EditNoteCommandHandler> logger)
        {
            _noteRL = noteRL;
            _cache = cache;
            _logger = logger;
        }

        public async Task<NotesResponseModel<NoteEntity>> Handle(EditNoteCommand command)
        {
            _logger.LogInformation(
               "Edit note process started for UserId: {UserId}",
               command.UserId);

            var note = await _noteRL.EditNoteRL(command.Model, command.UserId);

            if (note == null)
            {
                _logger.LogWarning(
                    "Note could not be updated for UserId: {UserId}",
                    command.UserId);

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

            _logger.LogInformation(
                "Note updated successfully. NoteId: {NoteId}, UserId: {UserId}",
                note.NoteId,
                command.UserId);

            _logger.LogDebug(
                "Notes list and individual-note caches invalidated for UserId: {UserId}",
                command.UserId);

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note updated successfully",
                Data = note
            };
        }
    }
}
