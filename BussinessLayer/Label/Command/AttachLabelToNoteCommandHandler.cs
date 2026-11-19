using BussinessLayer.Interface;
using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;

namespace BussinessLayer.Label.Command
{
    public class AttachLabelToNoteCommandHandler : IAttachLabelToNoteCommandHandler
    {
        private readonly ILabelBL _labelBL;
        private readonly ICacheService _cache;
        private readonly ILogger<AttachLabelToNoteCommandHandler> _logger;

        public AttachLabelToNoteCommandHandler(ILabelBL labelBL, ICacheService cache, ILogger<AttachLabelToNoteCommandHandler> logger)
        {
            _labelBL = labelBL;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(AttachLabelToNoteCommand command)
        {
            _logger.LogInformation(
                "Attaching label {LabelId} to note {NoteId} for user {UserId}",
                command.LabelId,
                command.NoteId,
                command.UserId);

            try
            {
                await _labelBL.AttachLabelToNoteAsync(
                    command.NoteId,
                    command.LabelId,
                    command.UserId);

                await _cache.RemoveAsync($"user:{command.UserId}:notes");

                _logger.LogInformation(
                    "Label {LabelId} attached to note {NoteId} successfully",
                    command.LabelId,
                    command.NoteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to attach label {LabelId} to note {NoteId} for user {UserId}",
                    command.LabelId,
                    command.NoteId,
                    command.UserId);

                throw;
            }
        }
    }
}
