using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteAPPLibrary;

namespace NoteTakingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //GET : /api/Home
        [HttpGet]
        [Authorize]
        public async Task<Object> GetHome(UserManager<ApplicationUser> userManager)
        {
            string userName = User.Claims.First(c => c.Type == "UserName").Value;
            var user = await _userManager.FindByNameAsync(userName);
            return new
            {
                user.Name,
                user.Email,
                user.UserName
            };
        }
    }
}
