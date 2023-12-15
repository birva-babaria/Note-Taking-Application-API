using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteAPPLibrary;
using NoteTakingAppAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NoteTakingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        private readonly NoteAppContext _context;

        public NotesController(NoteAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("AddingNote")]
        [Authorize]
        //POST method : api/Notes/AddingNote
        public async Task<Object> AddingNote(NoteModel model)
        {
            var userName = User.FindFirstValue("UserName");
            if (ModelState.IsValid)
            {
                var n = new Note();
                n.Title = model.Title;
                n.Data = model.Data;
                n.UserName = userName;
                n.Createddate = DateTime.Now;
                n.Lastmodifieddate = DateTime.Now;
                n.Trashstatus = false;
                _context.Notes.Add(n);
                await _context.SaveChangesAsync();
                return Ok(new {message = "New Note Created :)"});
            }
            else
            {
                return BadRequest(new { error = "Error Occured :(" });
            }
        }

        [HttpGet]
        [Route("UserNotes")]
        [Authorize]
        //GET method : api/Notes/UserNotes
        public async Task<IActionResult> GetUserNotes()
        {
            var userName = User.FindFirstValue("UserName");

            var userNotesDTO = await _context.Notes
                .Where(note => note.UserName == userName && !note.Trashstatus)
                .OrderByDescending(note => note.Createddate)
                .Select(note => new Notes
                {
                    Id = note.Id,
                    Title = note.Title,
                    Data = note.Data,
                    Createddate = note.Createddate,
                    Lastmodifieddate = note.Lastmodifieddate,
                    UserName = note.UserName,
                    Trashstatus = note.Trashstatus
                }).ToListAsync();

            var trashNotesDTO = await _context.Trashes
                .Where(t => t.Note.UserName == userName && t.Note.Trashstatus)
                .OrderByDescending(t => t.Deleteddate)
                .Select(trash => new Notes
                {
                    Id = trash.Note.Id,
                    Title = trash.Note.Title,
                    Data = trash.Note.Data,
                    Createddate = trash.Note.Createddate,
                    Lastmodifieddate = trash.Note.Lastmodifieddate,
                    UserName = trash.Note.UserName,
                    Deleteddate = trash.Deleteddate
                }).ToListAsync();

            return Ok(new { notes = userNotesDTO, trash = trashNotesDTO });
        }

        [HttpGet]
        [Route("AllNotes")]
        [Authorize]
        //GET method : api/Notes/AllNotes
        public async Task<List<Notes>> GetAllNotes()
        {
            var notesDTO = await _context.Notes
                .Where(note => !note.Trashstatus)
                .OrderByDescending(note => note.Createddate)
                .Select(n => new Notes()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Data = n.Data,
                    Createddate = n.Createddate,
                    Lastmodifieddate = n.Lastmodifieddate,
                    UserName = n.UserName,
                }).ToListAsync();
            return notesDTO;
        }

        [HttpGet("{id}")]
        [Authorize]
        //GET method : api/Notes/{id}
        public async Task<ActionResult<Notes>> GetNoteById(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            var noteDTO = new Notes
            {
                Id = note.Id,
                Title = note.Title,
                Data = note.Data,
                Createddate = note.Createddate,
                Lastmodifieddate = note.Lastmodifieddate,
                Trashstatus = note.Trashstatus,
                UserName = note.UserName
            };
            return noteDTO;
        }

        [HttpPut("editNote/{id}")]
        [Authorize]
        //PUT method : api/Notes/editNote/{id}
        public async Task<Object> EditNote(int id, NoteModel model)
        {
            var existingNote = await _context.Notes.FindAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }
            // Update the existingNote properties with values from updatedNote
            existingNote.Title = model.Title;
            existingNote.Data = model.Data;
            existingNote.Lastmodifieddate = DateTime.Now;
            _context.Entry(existingNote).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Note Updated Successfully :)" });
        }

        [HttpPost("trashNote/{id}")]
        [Authorize]
        //POST method : api/Notes/trashNote/{id}
        public async Task<Object> DeleteNote(int id)
        {
            var curr_note = await _context.Notes.FindAsync(id);
            if (curr_note == null)
            {
                return NotFound();
            }
            curr_note.Trashstatus = true;
            var t = new Trash();
            t.NoteId = id;
            t.Deleteddate = DateTime.Now;
            _context.Trashes.Add(t);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("recoverNote/{id}")]
        [Authorize]
        //POST method : api/Notes/recoverNote/{id}
        public async Task<Object> RecoverNote(int id)
        {
            var curr_note = await _context.Notes.FindAsync(id);
            if (curr_note == null)
            {
                return NotFound();
            }
            curr_note.Trashstatus = false;
            var trash_note = await _context.Trashes
                .Where(t => t.NoteId == id)
                .OrderByDescending(t => t.Deleteddate)
                .FirstOrDefaultAsync();
            if (trash_note != null)
            {
                _context.Trashes.Remove(trash_note);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
