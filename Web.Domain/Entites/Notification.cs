using PetCare.Api.Entities;

namespace Web.Domain.Entites
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }=string.Empty;
        public string  IconURL { get; set; }=string.Empty;
        public int PetId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        // Navigation
        public AppUser User { get; set; } = default!;
        public Pet Pet { get; set; } = default!;
    }



}
