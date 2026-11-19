using ModelLayer.Model.Label.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Label.Command
{
    public interface IRenameLabelCommandHandler
    {
        Task<LabelResponseModel> HandleAsync(RenameLabelCommand command);
    }
}
