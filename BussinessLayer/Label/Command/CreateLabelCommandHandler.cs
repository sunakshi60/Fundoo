using FluentValidation;
using Microsoft.Extensions.Logging;
using ModelLayer.Model.Label.ResponseModel;
using BussinessLayer.Interface;

namespace BussinessLayer.Label.Command
{
    public class CreateLabelCommandHandler : ICreateLabelCommandHandler
    {
        private readonly ILabelBL _labelBL;
        private readonly IValidator<CreateLabelCommand> _validator;
        private readonly ILogger<CreateLabelCommandHandler> _logger;

        public CreateLabelCommandHandler(ILabelBL labelBL, IValidator<CreateLabelCommand> validator, ILogger<CreateLabelCommandHandler> logger)
        {
            _labelBL = labelBL;
            _validator = validator;
            _logger = logger;
        }

        public async Task<LabelResponseModel> HandleAsync(CreateLabelCommand command)
        {
            _logger.LogInformation("Creating label for user {UserId}", command.UserId);

            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning(
                    "Label creation validation failed for user {UserId}",
                    command.UserId);

                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var label = await _labelBL.CreateLabelAsync(command.Name, command.UserId);

                _logger.LogInformation(
                    "Label {LabelId} created successfully for user {UserId}",
                    label.LabelId,
                    command.UserId);

                return label;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to create label for user {UserId}",
                    command.UserId);

                throw;
            }
        }
    }
}
