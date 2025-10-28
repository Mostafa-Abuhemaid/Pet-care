namespace Web.Domain.Entites
{
    // Model
    public class PetAdvice
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string Advice { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
