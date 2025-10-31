using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.NotificationDTO;
using Web.Application.Response;
using Web.Domain.Entites;

namespace Web.Application.Interfaces
{
    public interface INotificationService
    {
        Task<BaseResponse<List<NotificationResponseDTO>>> GenerateDailyNotificationsAsync(string userid);
        Task<BaseResponse<List<NotificationResponseDTO>>> GetUserNotificationsAsync(string userId);
    }

}
