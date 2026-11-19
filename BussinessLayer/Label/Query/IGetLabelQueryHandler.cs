using ModelLayer.Model.Label.ResponseModel;

namespace BussinessLayer.Label.Query
{
    public interface IGetLabelQueryHandler
    {
        Task<List<LabelResponseModel>> HandleAsync(GetLabelQuery query);
    }
}
