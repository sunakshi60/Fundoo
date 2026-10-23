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

        public DeleteNoteCommandHandler(INoteRL noteRL, ICacheService cache)
        {
            _noteRL = noteRL;
            _cache = cache;
        }

        public async Task<NotesResponseModel<bool>> Handle(DeleteNoteCommand command)
        {
            var deleted = await _noteRL.DeleteNoteRL(command.NoteId, command.UserId);

            if (!deleted)
            {
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

            return new NotesResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Note deleted successfully",
                Data = true
            };
        }
    }
}
