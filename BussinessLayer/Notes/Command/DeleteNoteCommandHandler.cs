using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Command
{
    public class DeleteNoteCommandHandler : IDeleteNoteCommandHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;
        private readonly ILogger<DeleteNoteCommandHandler> _logger;

        public DeleteNoteCommandHandler(INoteRL noteRL, ICacheService cache, ILogger<DeleteNoteCommandHandler> logger)
        {
            _noteRL = noteRL;
            _cache = cache;
            _logger = logger;
        }

        public async Task<NotesResponseModel<bool>> Handle(DeleteNoteCommand command)
        {
            _logger.LogInformation( "Delete note process started. NoteId: {NoteId}, UserId: {UserId}",
                command.NoteId,
                command.UserId);

            var deleted = await _noteRL.DeleteNoteRL(command.NoteId,command.UserId);

            if (!deleted)
            {
                _logger.LogWarning(
                    "Note could not be deleted. NoteId: {NoteId}, UserId: {UserId}",
                    command.NoteId,
                    command.UserId);

                return new NotesResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Note not found or could not be deleted",
                    Data = false
                };
            }

            // Invalidate list and individual-note caches
            await _cache.RemoveAsync($"user:{command.UserId}:notes");

            await _cache.RemoveAsync($"user:{command.UserId}:note:{command.NoteId}");

            _logger.LogInformation( 
                "Note deleted successfully. NoteId: {NoteId}, UserId: {UserId}",
                command.NoteId,
                command.UserId);

            _logger.LogDebug(
                "Notes list and individual-note caches invalidated for UserId: {UserId}",
                command.UserId);

            return new NotesResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Note deleted successfully",
                Data = true
            };
        }
    }
}
