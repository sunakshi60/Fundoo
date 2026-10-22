using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Query
{
    public class ViewNoteByIdQueryHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;

        public ViewNoteByIdQueryHandler(
            INoteRL noteRL,
            ICacheService cache)
        {
            _noteRL = noteRL;
            _cache = cache;
        }

        public async Task<NotesResponseModel<NoteEntity>> Handle(
            ViewNoteByIdQuery query)
        {
            string cacheKey =
                $"user:{query.UserId}:note:{query.NoteId}";

            var cachedNote =
                await _cache.GetAsync<NoteEntity>(cacheKey);

            if (cachedNote != null)
            {
                return new NotesResponseModel<NoteEntity>
                {
                    IsSuccess = true,
                    Message = "Note retrieved successfully from cache",
                    Data = cachedNote
                };
            }

            var note = await _noteRL.ViewNoteByIdRL(
                query.NoteId,
                query.UserId);

            if (note == null)
            {
                return new NotesResponseModel<NoteEntity>
                {
                    IsSuccess = false,
                    Message = "Note not found",
                    Data = null
                };
            }

            await _cache.SetAsync(
                cacheKey,
                note,
                TimeSpan.FromMinutes(10));

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note retrieved successfully",
                Data = note
            };
        }
    }
}
