using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Label.Command
{
    public interface IRemoveLabelFromNoteCommandHandler
    {
        Task HandleAsync(RemoveLabelFromNoteCommand command);
    }
}
