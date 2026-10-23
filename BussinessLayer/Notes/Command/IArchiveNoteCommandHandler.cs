using ModelLayer.Model.Notes.NotesResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Command
{
    public interface IArchiveNoteCommandHandler
    {
        Task<NotesResponseModel<bool>> Handle(ArchiveNoteCommand command);
    }
}
