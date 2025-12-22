namespace Web.Application.DTOs.BaseDTO
{
    public class ListItemFavoriteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string LogoUrl { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
