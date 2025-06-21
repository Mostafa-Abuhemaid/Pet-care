using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Interfaces;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Lock")]
        public async Task<IActionResult> LockUser(string email)
        {
            var user = await _userService.LockUserByEmailAsync(email);
            return user.Success ? Ok(user) : BadRequest(user);
        }


        //  [Authorize(Roles = "Admin")]
        [HttpPost("Unlock")]
        public async Task<IActionResult> UnlockUser(string email)
        {
            var user = await _userService.UnlockUserByEmailAsync(email);
            return user.Success ? Ok(user) : BadRequest(user);
        }
        // [Authorize(Roles = "Admin")] 
        [HttpDelete("DeleteAccountByEmail")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var user = await _userService.DeleteUserByEmailAsync(email);
            return user.Success ? Ok(user) : BadRequest(user);
        }
        // [Authorize(Roles = "Admin")] 
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _userService.GetAllUsersAsync();
            return user.Success ? Ok(user) : BadRequest(user);
        }
    }
}
