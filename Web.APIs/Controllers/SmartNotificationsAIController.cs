using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Interfaces;
using Web.Domain.Enums;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmartNotificationsAIController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;

        [HttpGet("Generate_Daily_Notifications_By_AI")]
        public async Task<IActionResult> GenerateDailyNotificationsByAI()
        {
            var userid = User.GetUserId();
            var result = await _notificationService.GenerateDailyNotificationsAsync(userid);
            return result.Success ? Ok(result) : BadRequest(result);

        }

        [HttpGet("GetUser_Daily_Notifications_By_AI")]
        public async Task<IActionResult> GetUserNotificationsAsync()
        {
            var userid = User.GetUserId();
            var result = await _notificationService.GetUserNotificationsAsync(userid);
            return result.Success ? Ok(result) : BadRequest(result);

        }

    }
}
