using ModelLayer.Model.Label.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Interface
{
    public interface ILabelBL
    {
        Task<LabelResponseModel> CreateLabelAsync(string name, int userId);

        Task<List<LabelResponseModel>> GetMyLabelsAsync(int userId);

        Task<LabelResponseModel> RenameLabelAsync(int labelId, string name, int userId);

        Task DeleteLabelAsync(int labelId, int userId);

        Task AttachLabelToNoteAsync(int noteId, int labelId, int userId);

        Task RemoveLabelFromNoteAsync(int noteId, int labelId, int userId);
    }
}
