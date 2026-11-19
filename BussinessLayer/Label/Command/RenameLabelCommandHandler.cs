using FluentValidation;
using Microsoft.Extensions.Logging;
using ModelLayer.Model.Label.ResponseModel;
using BussinessLayer.Interface;

namespace BussinessLayer.Label.Command
{
    public class RenameLabelCommandHandler : IRenameLabelCommandHandler
    {
        private readonly ILabelBL _labelBL;
        private readonly IValidator<RenameLabelCommand> _validator;
        private readonly ILogger<RenameLabelCommandHandler> _logger;

        public RenameLabelCommandHandler(ILabelBL labelBL, IValidator<RenameLabelCommand> validator,
            ILogger<RenameLabelCommandHandler> logger)
        {
            _labelBL = labelBL;
            _validator = validator;
            _logger = logger;
        }

        public async Task<LabelResponseModel> HandleAsync(RenameLabelCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Rename label validation failed for LabelId {LabelId}, UserId {UserId}",
                    command.LabelId,
                    command.UserId);

                throw new ValidationException(validationResult.Errors);
            }

            _logger.LogInformation(
                "Renaming label {LabelId} for user {UserId}",
                command.LabelId,
                command.UserId);

            try
            {
                var label = await _labelBL.RenameLabelAsync(
                    command.LabelId,
                    command.Name,
                    command.UserId);

                _logger.LogInformation(
                    "Label {LabelId} renamed successfully for user {UserId}",
                    label.LabelId,
                    command.UserId);

                return label;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to rename label {LabelId} for user {UserId}",
                    command.LabelId,
                    command.UserId);

                throw;
            }
        }
    }
}
