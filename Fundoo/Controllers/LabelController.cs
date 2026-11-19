using BussinessLayer.Label.Command;
using BussinessLayer.Label.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.Label.RequestModel;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    [ApiController]
    [Route("api/labels")]
    [Authorize]
    public class LabelController :ControllerBase
    {
        private readonly ICreateLabelCommandHandler _createLabelHandler;
        private readonly IGetLabelQueryHandler _getLabelHandler;
        private readonly IRenameLabelCommandHandler _renameLabelHandler;
        private readonly IDeleteLabelCommandHandler _deleteLabelHandler;
        private readonly IAttachLabelToNoteCommandHandler _attachLabelHandler;
        private readonly IRemoveLabelFromNoteCommandHandler _removeLabelHandler;

        public LabelController(ICreateLabelCommandHandler createLabelHandler,
            IGetLabelQueryHandler getLabelHandler,
            IRenameLabelCommandHandler renameLabelHandler,
            IDeleteLabelCommandHandler deleteLabelHandler,
            IAttachLabelToNoteCommandHandler attachLabelHandler,
            IRemoveLabelFromNoteCommandHandler removeLabelHandler)
        {
            _createLabelHandler = createLabelHandler;
            _getLabelHandler = getLabelHandler;
            _renameLabelHandler = renameLabelHandler;
            _deleteLabelHandler = deleteLabelHandler;
            _attachLabelHandler = attachLabelHandler;
            _removeLabelHandler = removeLabelHandler;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("UserId")
                ?? User.FindFirst("userId");

            return int.TryParse(userIdClaim?.Value, out userId)
                   && userId > 0;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody] CreateLabelModel request)
        {
            if (!TryGetCurrentUserId(out int userId))
                return Unauthorized(new { message = "Invalid user identity." });

            var command = new CreateLabelCommand
            {
                Name = request.Name,
                UserId = userId
            };

            var result = await _createLabelHandler.HandleAsync(command);

            return Ok(new
            {
                message = "Label created successfully.",
                data = result
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyLabels()
        {
            if (!TryGetCurrentUserId(out int userId))
                return Unauthorized(new { message = "Invalid user identity." });

            var query = new GetLabelQuery
            {
                UserId = userId
            };

            var result = await _getLabelHandler.HandleAsync(query);

            return Ok(new
            {
                message = "Labels retrieved successfully.",
                data = result
            });
        }

        [HttpPut("{labelId:int}")]
        public async Task<IActionResult> RenameLabel(int labelId, [FromBody] RenameLabelModel request)
        {
            if (!TryGetCurrentUserId(out int userId))
                return Unauthorized(new { message = "Invalid user identity." });

            var command = new RenameLabelCommand
            {
                LabelId = labelId,
                Name = request.Name,
                UserId = userId
            };

            var result = await _renameLabelHandler.HandleAsync(command);

            return Ok(new
            {
                message = "Label renamed successfully.",
                data = result
            });
        }

        [HttpDelete("{labelId:int}")]
        public async Task<IActionResult> DeleteLabel(int labelId)
        {
            if (!TryGetCurrentUserId(out int userId))
                return Unauthorized(new { message = "Invalid user identity." });

            var command = new DeleteLabelCommand
            {
                LabelId = labelId,
                UserId = userId
            };

            await _deleteLabelHandler.HandleAsync(command);

            return Ok(new
            {
                message = "Label deleted successfully."
            });
        }

        [HttpPut("~/api/notes/{noteId:int}/labels/{labelId:int}")]
        public async Task<IActionResult> AttachLabelToNote(int noteId, int labelId)
        {
            if (!TryGetCurrentUserId(out int userId))
                return Unauthorized(new { message = "Invalid user identity." });

            var command = new AttachLabelToNoteCommand
            {
                NoteId = noteId,
                LabelId = labelId,
                UserId = userId
            };

            await _attachLabelHandler.HandleAsync(command);

            return Ok(new
            {
                message = "Label attached to note successfully."
            });
        }

        [HttpDelete("~/api/notes/{noteId:int}/labels/{labelId:int}")]
        public async Task<IActionResult> RemoveLabelFromNote(int noteId,
            int labelId)
        {
            if (!TryGetCurrentUserId(out int userId))
                return Unauthorized(new { message = "Invalid user identity." });

            var command = new RemoveLabelFromNoteCommand
            {
                NoteId = noteId,
                LabelId = labelId,
                UserId = userId
            };

            await _removeLabelHandler.HandleAsync(command);

            return Ok(new
            {
                message = "Label removed from note successfully."
            });
        }
    }
}
