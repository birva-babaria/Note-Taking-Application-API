using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteAPPLibrary;
using NoteTakingAppAPI.DTOs;
using System.Security.Claims;

namespace NoteTakingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        private readonly NoteAppContext _context;
        public CommentsController(NoteAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("AllComments/{id}")]
        [Authorize]
        //GET method : api/Comments/AllComments/{id}
        public async Task<List<Comments>> GetUserComments(int id)
        {
            var CommentsDTO = await _context.Comments
                .Where(com => com.NoteId == id)
                .OrderByDescending(com => com.Posteddate)
                .Select(com => new Comments
                {
                    Id = com.Id,
                    Content = com.Content,
                    Posteddate = com.Posteddate,
                    NoteId = com.NoteId,
                    UserName = com.UserName,
                })
                .ToListAsync();

            return CommentsDTO;
        }

        [HttpPost]
        [Route("AddComment/{id}")]
        [Authorize]
        //POST : api/Comments/AddComment/{id}
        public async Task<IActionResult> PostAddComment(int id, CommentModel model)
        {
            var userName = User.FindFirstValue("UserName");

            var com = new Comment();
            com.Content = model.Content;
            com.NoteId = id;
            com.Posteddate = DateTime.Now;
            com.UserName = userName;
            _context.Comments.Add(com);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment Added :)" });
        }
    }
}
