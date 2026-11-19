using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Command
{
    public class AddNoteCommandHandler : IAddNoteCommandHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;
        private readonly ILogger<AddNoteCommandHandler> _logger;

        public AddNoteCommandHandler(INoteRL noteRL, ICacheService cache, ILogger<AddNoteCommandHandler> logger)
        {
            _noteRL = noteRL;
            _cache = cache;
            _logger = logger;
        }

        public async Task<NotesResponseModel<NoteEntity>> Handle(AddNoteCommand command)
        {
            _logger.LogInformation( "Add note process started for UserId: {UserId}", command.UserId);

            var note = await _noteRL.AddNoteRL(command.Model, command.UserId);

            if (note == null)
            {
                _logger.LogWarning("Note could not be added for UserId: {UserId}", command.UserId);

                return new NotesResponseModel<NoteEntity>
                {
                    IsSuccess = false,
                    Message = "Note could not be added",
                    Data = null
                };
            }

            // Invalidate cached notes list for this user
            string cacheKey = $"user:{command.UserId}:notes";

            await _cache.RemoveAsync(cacheKey);

            _logger.LogInformation( "Note added successfully. NoteId: {NoteId}, UserId: {UserId}",
                note.NoteId,
                command.UserId);

            _logger.LogDebug("Notes cache invalidated for UserId: {UserId}", command.UserId);

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note added successfully",
                Data = note
            };
        }
    }
}
