using BussinessLayer.Interface;
using Microsoft.Extensions.Logging;
using Middleware.Services.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Label.Command
{
    public class RemoveLabelFromNoteCommandHandler : IRemoveLabelFromNoteCommandHandler
    {
        private readonly ILabelBL _labelBL;
        private readonly ICacheService _cache;
        private readonly ILogger<RemoveLabelFromNoteCommandHandler> _logger;

        public RemoveLabelFromNoteCommandHandler(ILabelBL labelBL, ICacheService cache, ILogger<RemoveLabelFromNoteCommandHandler> logger)
        {
            _labelBL = labelBL;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(RemoveLabelFromNoteCommand command)
        {
            _logger.LogInformation(
                "Removing label {LabelId} from note {NoteId} for user {UserId}",
                command.LabelId,
                command.NoteId,
                command.UserId);

            try
            {
                await _labelBL.RemoveLabelFromNoteAsync(
                    command.NoteId,
                    command.LabelId,
                    command.UserId);

                // Invalidate cached notes after successful removal.
                await _cache.RemoveAsync($"user:{command.UserId}:notes");

                _logger.LogInformation(
                    "Label {LabelId} removed from note {NoteId} successfully",
                    command.LabelId,
                    command.NoteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to remove label {LabelId} from note {NoteId} for user {UserId}",
                    command.LabelId,
                    command.NoteId,
                    command.UserId);

                throw;
            }
        }
    }
}
