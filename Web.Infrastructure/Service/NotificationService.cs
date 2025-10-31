using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.AccountDTO;
using Web.Application.DTOs.NotificationDTO;
using Web.Application.DTOs.ProductDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Infrastructure.Service
{

    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotificationService> _logger;
        private readonly IGeminiService _geminiService;
        private readonly ICacheService _cacheService;
        private const string CachePrefix = "AvailableNotifications";

        public NotificationService(
            AppDbContext context,
            ILogger<NotificationService> logger,
            IGeminiService geminiService,
            ICacheService cacheService)
        {
            _context = context;
            _logger = logger;
            _geminiService = geminiService;
            _cacheService = cacheService;
        }

        public async Task<BaseResponse<List<NotificationResponseDTO>>> GenerateDailyNotificationsAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Pets)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new BaseResponse<List<NotificationResponseDTO>>(false, "User not found");

            if (user.Pets == null || user.Pets.Count == 0)
                return new BaseResponse<List<NotificationResponseDTO>>(false, "No pets found for this user");

            var notifications = new List<NotificationResponseDTO>();

            foreach (var pet in user.Pets)
            {
                try
                {
                    var advice = await _geminiService.GeneratePetAdvice(pet.petType, pet.Breed, pet.Name);
                    var notification = new Notification
                    {
                        UserId = userId,
                        PetId = pet.Id,
                        IconURL = pet.PhotoUrl ?? string.Empty,
                        Message = advice,
                        CreatedAt = DateTime.UtcNow
                    };

                    notifications.Add(notification.Adapt<NotificationResponseDTO>());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to generate notification for Pet ID {pet.Id}");
                }
            }

            var cacheKey = $"{CachePrefix}:{userId}";

            var existingNotifications = await _cacheService.GetAsync<List<NotificationResponseDTO>>(cacheKey)
                                 ?? new List<NotificationResponseDTO>();

            existingNotifications.AddRange(notifications);

            await _cacheService.SetAsync(cacheKey, existingNotifications, TimeSpan.FromHours(24));


            return new BaseResponse<List<NotificationResponseDTO>>(true, "Notifications generated successfully", notifications);
        }

        public async Task<BaseResponse<List<NotificationResponseDTO>>> GetUserNotificationsAsync(string userId)
        {
            var cacheKey = $"{CachePrefix}:{userId}";
            var cached = await _cacheService.GetAsync<List<NotificationResponseDTO>>(cacheKey);

            if (cached != null)
                return new BaseResponse<List<NotificationResponseDTO>>(true,"",cached);

            return new BaseResponse<List<NotificationResponseDTO>>(false,"No Notifications avaliable Now!");
        }
    }
}
