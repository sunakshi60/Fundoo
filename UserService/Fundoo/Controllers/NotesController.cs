using BussinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.NotesRequest;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController:ControllerBase
    {
        private readonly INoteBL _noteBL;

        public NotesController(INoteBL noteBL)
        {
            _noteBL = noteBL;
        }

        [Authorize]
        [HttpPost("AddNote")]
        public async Task <ActionResult> AddNote(NoteCreationModel model)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("Valid user ID claim not found");
            }
            var note = await _noteBL.AddNoteBL(model, userId);

            return Ok(note);
        }

        [HttpGet("ViewNotes")]
        public async Task<ActionResult> ViewNotes()
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized();
            }

            var notes = await _noteBL.ViewNotesBL(userId);

            return Ok(notes);
        }

        [HttpGet("ViewNoteById/{noteId:int}")]
        public async Task<ActionResult> ViewNoteById(int noteId)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("You must login first");
            }

            var note = await _noteBL.ViewNoteByIdBL(noteId, userId);

            if (!note.IsSuccess)
            {
                return NotFound(note);
            }

            return Ok(note);
        }

        [HttpPut("EditNote")]
        public async Task<ActionResult> EditNote([FromBody] EditNotesModel model)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("You must login to edit your notes");
            }

            var note = await _noteBL.EditNoteBL(model, userId);

            if (!note.IsSuccess)
            {
                return NotFound(note);
            }

            return Ok(note);
        }


        [HttpDelete("DeleteNote")]
        public async Task<ActionResult> DeleteNote(NotesIdModel model)
        {
            if(!TryGetUserId(out int userId))
            {
                return Unauthorized("You must login first.");
            }

            var result = await _noteBL.DeleteNoteBL(model.NoteId, userId);

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPatch("ArchUnarchieve")]
        public async Task<ActionResult> ArchUnarchived(NotesIdModel model)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized("You must login first");
            }

            var archieved = await _noteBL.ArchUnarchivedBL(model.NoteId, userId);

            if (!archieved.IsSuccess)
            {
                return NotFound(archieved);
            }

            return Ok("Note archive status updated successfully");
        }

        private bool TryGetUserId(out int userId)
        {
            return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }
    }
}
