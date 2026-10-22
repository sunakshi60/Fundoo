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
    public class ViewNotesQueryHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;

        public ViewNotesQueryHandler(INoteRL noteRL, ICacheService cache)
        {
            _noteRL = noteRL;
            _cache = cache;
        }

        public async Task<NotesResponseModel<List<NoteEntity>>> Handle(ViewNotesQuery query)
        {
            string cacheKey = $"user:{query.UserId}:notes";

            var cachedNotes = await _cache.GetAsync<List<NoteEntity>>(cacheKey);

            if (cachedNotes != null)
            {
                return new NotesResponseModel<List<NoteEntity>>
                {
                    IsSuccess = true,
                    Message = "Notes retrieved successfully from cache",
                    Data = cachedNotes
                };
            }

            var notes = await _noteRL.ViewNotesRL(query.UserId);

            await _cache.SetAsync(
                cacheKey,
                notes,
                TimeSpan.FromMinutes(10));

            return new NotesResponseModel<List<NoteEntity>>
            {
                IsSuccess = true,
                Message = "Notes retrieved successfully",
                Data = notes
            };
        }
    }
}
