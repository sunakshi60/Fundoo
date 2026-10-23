using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Command
{
    public class ArchiveNoteCommandHandler : IArchiveNoteCommandHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;

        public ArchiveNoteCommandHandler(INoteRL noteRL, ICacheService cache)
        {
            _noteRL = noteRL;
            _cache = cache;
        }

        public async Task<NotesResponseModel<bool>> Handle(ArchiveNoteCommand command)
        {
            var updated = await _noteRL.ArchUnarchivedRL(
                command.NoteId,
                command.UserId);

            if (!updated)
            {
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

            return new NotesResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Note archive status updated successfully",
                Data = true
            };
        }
    }
}
