using BussinessLayer.Interface;
using ModelLayer.Model.Notes.NotesRequest;
using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BussinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL _noteRL;
        private readonly IDistributedCache _cache;

        public NoteBL(INoteRL noteRL,IDistributedCache cache)
        {
            _noteRL = noteRL;
            _cache = cache;
        }

        private async Task InvalidateNotesCacheAsync(int userId)
        {
            string cacheKey = $"user:{userId}:notes";

            await _cache.RemoveAsync(cacheKey);
        }

        private async Task InvalidateNoteCacheAsync(int noteId, int userId)
        {
            string cacheKey = $"user:{userId}:note:{noteId}";
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task<NotesResponseModel<NoteEntity>> AddNoteBL(NoteCreationModel model, int userId)
        {
            var note = await _noteRL.AddNoteRL(model, userId);

            await InvalidateNotesCacheAsync(userId);

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note added successfully",
                Data = note
            };
        }

        public async Task<NotesResponseModel<List<NoteEntity>>> ViewNotesBL(int userId)
        {
            string cacheKey = $"user:{userId}:notes";

            //Check Redis
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                var cachedNotes = JsonSerializer.Deserialize <List<NoteEntity >> (cachedData);

                if (cachedNotes != null)
                {
                    return new NotesResponseModel<List<NoteEntity>>
                    {
                        IsSuccess = true,
                        Message="Notes retrieved successfully from cache",
                        Data=cachedNotes
                    };
                }
            }

            //Cache miss - fetch from PostgresSQL
            var notes = await _noteRL.ViewNotesRL(userId);

            //store the result in Redis for 10 minutes
            var jsonData = JsonSerializer.Serialize(notes);

            await _cache.SetStringAsync(cacheKey, jsonData,
              new DistributedCacheEntryOptions
              {
                  AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
              });

            //return notes
            return new NotesResponseModel<List<NoteEntity>>
            {
                IsSuccess = true,
                Message = "Notes retrieved successfully",
                Data = notes
            };
        }

        public async Task<NotesResponseModel<NoteEntity>> ViewNoteByIdBL(int noteId, int userId)
        {
            // Create a unique cache key for this user's specific note
            string cacheKey = $"user:{userId}:note:{noteId}";

            // Check Redis cache
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                var cachedNote = JsonSerializer.Deserialize<NoteEntity>(cachedData);

                if (cachedNote != null)
                {
                    return new NotesResponseModel<NoteEntity>
                    {
                        IsSuccess = true,
                        Message = "Note retrieved successfully from cache",
                        Data = cachedNote
                    };
                }
            }

            // Cache miss: fetch note from the database
            var note = await _noteRL.ViewNoteByIdRL(noteId, userId);

            if (note == null)
            {
                return new NotesResponseModel<NoteEntity>
                {
                    IsSuccess = false,
                    Message = "Note not found",
                    Data = null
                };
            }

            // Store the note in Redis for 10 minutes
            var jsonData = JsonSerializer.Serialize(note);

            await _cache.SetStringAsync(
                cacheKey,
                jsonData,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

            // Return the note
            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note retrieved successfully",
                Data = note
            };
        }

        public async Task<NotesResponseModel<NoteEntity>> EditNoteBL(EditNotesModel model, int userId)
        {
            var note = await _noteRL.EditNoteRL(model, userId);

            if (note == null)
            {
                return new NotesResponseModel<NoteEntity>
                {
                    IsSuccess = false,
                    Message = "Note not found",
                    Data = null
                };
            }

            await InvalidateNotesCacheAsync(userId);

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note updated successfully",
                Data = note
            };
        }

        public async Task<NotesResponseModel<bool>> DeleteNoteBL(int noteId, int userId)
        {
            var deleted = await _noteRL.DeleteNoteRL(noteId, userId);

            if (deleted)
            {
                await InvalidateNotesCacheAsync(userId);
                await InvalidateNoteCacheAsync(noteId, userId);
            }

            return new NotesResponseModel<bool>
            {
                IsSuccess = deleted,
                Message = deleted
                    ? "Note moved to trash successfully"
                    : "Note not found",
                Data = deleted
            };
        }

        public async Task<NotesResponseModel<bool>> ArchUnarchivedBL(int noteId, int userId)
        {
            var updated = await _noteRL.ArchUnarchivedRL(noteId,userId);

            if (updated)
            {
                await InvalidateNotesCacheAsync(userId);
                await InvalidateNoteCacheAsync(noteId, userId);
            }

            return new NotesResponseModel<bool>
            {
                IsSuccess = updated,
                Message = updated
                    ? "Note archive status updated successfully"
                    : "Note not found",
                Data = updated
            };
        }
    }
}
