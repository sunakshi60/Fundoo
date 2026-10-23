using ModelLayer.Model.Notes.NotesResponse;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Notes.Query
{
    public interface IViewNotesQueryHandler
    {
        Task<NotesResponseModel<List<NoteEntity>>> Handle(ViewNotesQuery query);
    }
}
