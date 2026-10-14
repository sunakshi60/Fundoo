using BussinessLayer.Interface;
using ModelLayer.Model.NotesRequest;
using ModelLayer.Model.NotesResponse;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL _noteRL;

        public NoteBL(INoteRL noteRL)
        {
            _noteRL = noteRL;
        }

        public async Task<NotesResponseModel<NoteEntity>> AddNoteBL(NoteCreationModel model, int userId)
        {
            var note = await _noteRL.AddNoteRL(model, userId);

            return new NotesResponseModel<NoteEntity>
            {
                IsSuccess = true,
                Message = "Note added successfully",
                Data = note
            };
        }

        public async Task<NotesResponseModel<List<NoteEntity>>> ViewNotesBL(int userId)
        {
           var notes= await _noteRL.ViewNotesRL(userId);

            return new NotesResponseModel<List<NoteEntity>>
            {
                IsSuccess = true,
                Message = "Notes retrieved successfully",
                Data = notes
            };
        }

        public async Task<NotesResponseModel<NoteEntity>> ViewNoteByIdBL(int noteId, int userId)
        {
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
