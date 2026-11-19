using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Query
{
    public class ViewNotesQueryHandler : IViewNotesQueryHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;
        private readonly ILogger<ViewNotesQueryHandler> _logger;

        public ViewNotesQueryHandler(INoteRL noteRL, ICacheService cache, ILogger<ViewNotesQueryHandler> logger)
        {
            _noteRL = noteRL;
            _cache = cache;
            _logger = logger;
        }

        public async Task<NotesResponseModel<List<NoteEntity>>> Handle(ViewNotesQuery query)
        {
            _logger.LogInformation(
                "Fetching notes started for UserId: {UserId}",
                query.UserId);

            string cacheKey = $"user:{query.UserId}:notes";

            var cachedNotes = await _cache.GetAsync<List<NoteEntity>>(cacheKey);

            if (cachedNotes != null)
            {
                _logger.LogInformation(
                    "Notes retrieved successfully from cache for UserId: {UserId}",
                    query.UserId);

                return new NotesResponseModel<List<NoteEntity>>
                {
                    IsSuccess = true,
                    Message = "Notes retrieved successfully from cache",
                    Data = cachedNotes
                };
            }

            _logger.LogDebug(
                "Cache miss for UserId: {UserId}. Fetching notes from database.",
                query.UserId);

            var notes = await _noteRL.ViewNotesRL(query.UserId);

            await _cache.SetAsync(
                cacheKey,
                notes,
                TimeSpan.FromMinutes(10));

            _logger.LogInformation(
                "Notes retrieved from database and cached for 10 minutes. UserId: {UserId}, NoteCount: {NoteCount}",
                query.UserId,
                notes?.Count ?? 0);

            return new NotesResponseModel<List<NoteEntity>>
            {
                IsSuccess = true,
                Message = "Notes retrieved successfully",
                Data = notes
            };
        }
    }
}
