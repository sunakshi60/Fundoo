using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Command
{
    public class AddNoteCommandHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;

        public AddNoteCommandHandler(INoteRL noteRL, ICacheService cache)
        {
            _noteRL = noteRL;
            _cache = cache;
        }

        public async Task<NotesResponseModel<NoteEntity>> Handle(AddNoteCommand command)
        {
            var note = await _noteRL.AddNoteRL(command.Model, command.UserId);

            if (note == null)
            {
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


            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note added successfully",
                Data = note
            };
        }
    }
}
