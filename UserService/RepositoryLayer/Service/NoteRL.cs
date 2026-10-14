using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.NotesRequest;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        FundooContext _context;

        public NoteRL(FundooContext fundooContext)
        {
            _context = fundooContext;
        }

        public async Task<NoteEntity> AddNoteRL(NoteCreationModel model, int userId)
        {
            var note = new NoteEntity
            {
                Title = model.Title,
                Description = model.Description,
                Colour = model.Colour,
                UserId = userId
            };

            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task<List<NoteEntity>> ViewNotesRL(int userId)
        {
            var notes = await _context.Notes
                .Where(n => n.UserId == userId && n.IsDeleted == false)
                .Select(n => new NoteEntity
                {
                    NoteId = n.NoteId,
                    Title = n.Title,
                    Description = n.Description,
                    Colour = n.Colour,
                    IsArchived = n.IsArchived,
                    IsDeleted = n.IsDeleted,
                    UserId = n.UserId
                }).ToListAsync();

            return notes;
        }

        public async Task<NoteEntity> ViewNoteByIdRL(int noteId, int userId)
        {
            var note = await _context.Notes
            .Where(n => n.NoteId == noteId
                  && n.UserId == userId
                  && n.IsDeleted == false)
            .Select(n => new NoteEntity
            {
                 NoteId = n.NoteId,
                Title = n.Title,
                Description = n.Description,
                Colour = n.Colour,
                IsArchived = n.IsArchived,
                IsDeleted = n.IsDeleted,
                UserId = n.UserId
            }).FirstOrDefaultAsync();

            return note;
        }

        public async Task<NoteEntity> EditNoteRL(EditNotesModel model, int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                n.NoteId == model.NoteId &&
                n.UserId == userId &&
                !n.IsDeleted);

            if (note == null)
            {
                return null;
            }

            if (model.Title != null)
            {
                note.Title = model.Title;
            }

            if (model.Description != null)
            {
                note.Description = model.Description;
            }

            if (model.Colour != null)
            {
                note.Colour = model.Colour;
            }

            note.Title = model.Title;
            note.Description = model.Description;
            note.Colour = model.Colour;

            await _context.SaveChangesAsync();

            return note;
        }

        public async Task<bool> DeleteNoteRL(int noteId, int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                n.NoteId == noteId &&
                n.UserId == userId &&
                !n.IsDeleted);

            if (note == null)
            {
                return false;
            }

            note.IsDeleted = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ArchUnarchivedRL(int noteId, int userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId &&
                    !n.IsDeleted);

            if (note == null)
            {
                return false;
            }

            note.IsArchived = !note.IsArchived;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
