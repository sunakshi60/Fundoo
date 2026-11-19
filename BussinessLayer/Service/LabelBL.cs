using BussinessLayer.Interface;
using ModelLayer.Model.Label.ResponseModel;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL _labelRL;

        public LabelBL(ILabelRL labelRL)
        {
            _labelRL = labelRL;
        }

        public async Task<LabelResponseModel> CreateLabelAsync(string name, int userId)
        {
            name = NormalizeName(name);

            string normalizedName = name.ToUpperInvariant();

            var existingLabel = await _labelRL.GetByNormalizedNameAsync(userId, normalizedName);

            if (existingLabel != null)
            {
                throw new InvalidOperationException("A label with this name already exists.");
            }

            var label = new LabelEntity
            {
                Name = name,
                NormalizedName = normalizedName,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _labelRL.AddAsync(label);
            await _labelRL.SaveChangesAsync();

            return MapToResponse(label);
        }

        public async Task<List<LabelResponseModel>> GetMyLabelsAsync(int userId)
        {
            var labels = await _labelRL.GetAllByUserIdAsync(userId);

            return labels.Select(MapToResponse).ToList();
        }

        public async Task<LabelResponseModel> RenameLabelAsync(int labelId, string name, int userId)
        {
            name = NormalizeName(name);

            var label = await _labelRL.GetByIdAndUserIdAsync(labelId, userId);

            if (label == null)
            {
                throw new KeyNotFoundException("Label not found.");
            }

            string normalizedName = name.ToUpperInvariant();

            var existingLabel = await _labelRL.GetByNormalizedNameAsync(userId, normalizedName);

            if (existingLabel != null && existingLabel.LabelId != labelId)
            {
                throw new InvalidOperationException("A label with this name already exists.");
            }

            label.Name = name;
            label.NormalizedName = normalizedName;

            await _labelRL.SaveChangesAsync();

            return MapToResponse(label);
        }

        public async Task DeleteLabelAsync(int labelId, int userId)
        {
            var label = await _labelRL.GetByIdAndUserIdAsync(labelId, userId);

            if (label == null)
            {
                throw new KeyNotFoundException("Label not found.");
            }

            await _labelRL.DeleteAsync(label);
            await _labelRL.SaveChangesAsync();
        }

        public async Task AttachLabelToNoteAsync(int noteId, int labelId, int userId)
        {
            var noteExists = await _labelRL.NoteExistsForUserAsync(noteId, userId);

            if (!noteExists)
            {
                throw new KeyNotFoundException("Note not found.");
            }

            var label = await _labelRL.GetByIdAndUserIdAsync(labelId, userId);

            if (label == null)
            {
                throw new KeyNotFoundException("Label not found.");
            }

            var isTrashed = await _labelRL.IsNoteTrashedAsync(noteId);

            if (isTrashed)
            {
                throw new InvalidOperationException("Labels cannot be attached to trashed notes.");
            }

            var associationExists = await _labelRL.NoteLabelExistsAsync(noteId, labelId);

            if (associationExists)
            {
                throw new InvalidOperationException("This label is already attached to the note.");
            }

            var noteLabel = new NoteLabelEntity
            {
                NoteId = noteId,
                LabelId = labelId
            };

            await _labelRL.AddNoteLabelAsync(noteLabel);
            await _labelRL.SaveChangesAsync();
        }

        public async Task RemoveLabelFromNoteAsync(int noteId, int labelId, int userId)
        {
            var noteExists = await _labelRL.NoteExistsForUserAsync(noteId, userId);

            if (!noteExists)
            {
                throw new KeyNotFoundException("Note not found.");
            }

            var label = await _labelRL.GetByIdAndUserIdAsync(labelId, userId);

            if (label == null)
            {
                throw new KeyNotFoundException("Label not found.");
            }

            var associationExists = await _labelRL.NoteLabelExistsAsync(noteId, labelId);

            if (!associationExists)
            {
                throw new KeyNotFoundException("The label is not attached to this note.");
            }

            await _labelRL.RemoveNoteLabelAsync(noteId, labelId);

            await _labelRL.SaveChangesAsync();
        }

        private static string NormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Label name is required.");
            }

            name = name.Trim();

            if (name.Length > 100)
            {
                throw new ArgumentException("Label name cannot exceed 100 characters.");
            }

            return name;
        }

        private static LabelResponseModel MapToResponse(LabelEntity label)
        {
            return new LabelResponseModel
            {
                LabelId = label.LabelId,
                Name = label.Name,
                CreatedAt = label.CreatedAt
            };
        }
    }
}
