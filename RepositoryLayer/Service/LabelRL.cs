using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        private readonly FundooContext _context;

        public LabelRL(FundooContext context)
        {
            _context = context;
        }

        public async Task<LabelEntity?> GetByIdAndUserIdAsync(int labelId, int userId)
        {
            return await _context.Labels.FirstOrDefaultAsync(l =>
                    l.LabelId == labelId &&
                    l.UserId == userId);
        }

        public async Task<LabelEntity?> GetByNormalizedNameAsync(int userId, string normalizedName)
        {
            return await _context.Labels.FirstOrDefaultAsync(l =>
                    l.UserId == userId &&
                    l.NormalizedName == normalizedName);
        }

        public async Task<List<LabelEntity>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Labels
                .Where(l => l.UserId == userId)
                .OrderBy(l => l.Name)
                .ToListAsync();
        }

        public async Task AddAsync(LabelEntity label)
        {
            await _context.Labels.AddAsync(label);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(LabelEntity label)
        {
            _context.Labels.Remove(label);

            return Task.CompletedTask;
        }

        public async Task<bool> NoteExistsForUserAsync(int noteId, int userId)
        {
            return await _context.Notes
                .AnyAsync(n =>
                    n.NoteId == noteId &&
                    n.UserId == userId);
        }

        public async Task<bool> NoteLabelExistsAsync(int noteId, int labelId)
        {
            return await _context.NoteLabels
                .AnyAsync(nl =>
                    nl.NoteId == noteId &&
                    nl.LabelId == labelId);
        }

        public async Task AddNoteLabelAsync(NoteLabelEntity noteLabel)
        {
            await _context.NoteLabels.AddAsync(noteLabel);
        }

        public async Task RemoveNoteLabelAsync(int noteId, int labelId)
        {
            var noteLabel = await _context.NoteLabels
                .FirstOrDefaultAsync(nl =>
                    nl.NoteId == noteId &&
                    nl.LabelId == labelId);

            if (noteLabel != null)
            {
                _context.NoteLabels.Remove(noteLabel);
            }
        }
        public async Task<bool> IsNoteTrashedAsync(int noteId)
        {
            return await _context.Notes
                .AnyAsync(n => n.NoteId == noteId && n.IsDeleted);
        }
    }
}
