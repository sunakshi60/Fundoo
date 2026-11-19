using Microsoft.Extensions.Logging;
using BussinessLayer.Interface;


namespace BussinessLayer.Label.Command
{
    public class DeleteLabelCommandHandler : IDeleteLabelCommandHandler
    {
        private readonly ILabelBL _labelBL;
        private readonly ILogger<DeleteLabelCommandHandler> _logger;

        public DeleteLabelCommandHandler(ILabelBL labelBL, ILogger<DeleteLabelCommandHandler> logger)
        {
            _labelBL = labelBL;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteLabelCommand command)
        {
            _logger.LogInformation(
                "Deleting label {LabelId} for user {UserId}",
                command.LabelId,
                command.UserId);

            try
            {
                await _labelBL.DeleteLabelAsync(
                    command.LabelId,
                    command.UserId);

                _logger.LogInformation(
                    "Label {LabelId} deleted successfully for user {UserId}",
                    command.LabelId,
                    command.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to delete label {LabelId} for user {UserId}",
                    command.LabelId,
                    command.UserId);

                throw;
            }
        }
    }
}
