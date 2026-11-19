using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Command
{
    public class ArchiveNoteCommandHandler : IArchiveNoteCommandHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;
        private readonly ILogger<ArchiveNoteCommandHandler> _logger;

        public ArchiveNoteCommandHandler(INoteRL noteRL, ICacheService cache, ILogger<ArchiveNoteCommandHandler> logger)
        {
            _noteRL = noteRL;
            _cache = cache;
            _logger = logger;
        }

        public async Task<NotesResponseModel<bool>> Handle(ArchiveNoteCommand command)
        {
            _logger.LogInformation("Archive note process started. NoteId: {NoteId}, UserId: {UserId}",
                command.NoteId,
                command.UserId);

            var updated = await _noteRL.ArchUnarchivedRL(command.NoteId, command.UserId);

            if (!updated)
            {
                _logger.LogWarning(
                    "Note could not be archived. NoteId: {NoteId}, UserId: {UserId}",
                    command.NoteId,
                    command.UserId);

                return new NotesResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Note not found or could not be updated",
                    Data = false
                };
            }

            // Invalidate list and individual-note caches
            await _cache.RemoveAsync($"user:{command.UserId}:notes");

            await _cache.RemoveAsync($"user:{command.UserId}:note:{command.NoteId}");

            _logger.LogInformation(
                "Note archive status updated successfully. NoteId: {NoteId}, UserId: {UserId}",
                command.NoteId,
                command.UserId);

            _logger.LogDebug(
                "Notes list and individual-note caches invalidated for UserId: {UserId}",
                command.UserId);

            return new NotesResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Note archive status updated successfully",
                Data = true
            };
        }
    }
}
