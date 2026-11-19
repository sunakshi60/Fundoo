using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        Task<LabelEntity?> GetByIdAndUserIdAsync(int labelId, int userId);

        Task<LabelEntity?> GetByNormalizedNameAsync(int userId,string normalizedName);

        Task<List<LabelEntity>> GetAllByUserIdAsync(int userId);

        Task AddAsync(LabelEntity label);

        Task SaveChangesAsync();

        Task DeleteAsync(LabelEntity label);

        Task<bool> NoteExistsForUserAsync(int noteId, int userId);

        Task<bool> NoteLabelExistsAsync(int noteId, int labelId);

        Task AddNoteLabelAsync(NoteLabelEntity noteLabel);

        Task RemoveNoteLabelAsync(int noteId, int labelId);

        Task<bool> IsNoteTrashedAsync(int noteId);
    }
}
