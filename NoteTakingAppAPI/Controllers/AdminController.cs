using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteAPPLibrary;

namespace NoteTakingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        private readonly NoteAppContext _context;

        public AdminController(NoteAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteNote/{id}")]
        //DELETE method : api/Admin/DeleteNote/{id}
        public async Task<IActionResult> AdminDeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            // Delete associated comments records
            var comments = _context.Comments.Where(c => c.NoteId == id);
            _context.Comments.RemoveRange(comments);

            // Delete associated trash records
            var trashRecords = _context.Trashes.Where(t => t.NoteId == id);
            _context.Trashes.RemoveRange(trashRecords);

            // Delete the note itself
            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteComment/{id}")]
        //DELETE method : api/Admin/DeleteComment/{id}
        public async Task<IActionResult> AdminDeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            // Delete the comment itself
            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
