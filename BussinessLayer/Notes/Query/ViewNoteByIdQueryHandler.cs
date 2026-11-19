using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BussinessLayer.Notes.Query
{
    public class ViewNoteByIdQueryHandler : IViewNoteByIdQueryHandler
    {
        private readonly INoteRL _noteRL;
        private readonly ICacheService _cache;
        private readonly ILogger<ViewNoteByIdQueryHandler> _logger;

        public ViewNoteByIdQueryHandler(INoteRL noteRL, ICacheService cache, ILogger<ViewNoteByIdQueryHandler> logger)
        {
            _noteRL = noteRL;
            _cache = cache;
            _logger = logger;
        }

        public async Task<NotesResponseModel<NoteEntity>> Handle(ViewNoteByIdQuery query)
        {
            _logger.LogInformation(
                "Fetching note started. NoteId: {NoteId}, UserId: {UserId}",
                query.NoteId,
                query.UserId);

            string cacheKey = $"user:{query.UserId}:note:{query.NoteId}";

            var cachedNote = await _cache.GetAsync<NoteEntity>(cacheKey);

            if (cachedNote != null)
            {
                _logger.LogInformation(
                    "Note retrieved successfully from cache. NoteId: {NoteId}, UserId: {UserId}",
                    query.NoteId,
                    query.UserId);

                return new NotesResponseModel<NoteEntity>
                {
                    IsSuccess = true,
                    Message = "Note retrieved successfully from cache",
                    Data = cachedNote
                };
            }

            _logger.LogDebug(
                "Cache miss for NoteId: {NoteId}, UserId: {UserId}. Fetching from database.",
                query.NoteId,
                query.UserId);

            var note = await _noteRL.ViewNoteByIdRL(
                query.NoteId,
                query.UserId);

            if (note == null)
            {
                _logger.LogWarning(
                    "Note not found. NoteId: {NoteId}, UserId: {UserId}",
                    query.NoteId,
                    query.UserId);

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

            _logger.LogInformation(
                "Note retrieved from database and cached for 10 minutes. NoteId: {NoteId}, UserId: {UserId}",
                note.NoteId,
                query.UserId);

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note retrieved successfully",
                Data = note
            };
        }
    }
}
