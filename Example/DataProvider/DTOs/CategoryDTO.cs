using System.Text.Json.Serialization;

namespace DataProvider.DTOs
{
    public class CategoryDTO
    {
        [JsonPropertyName("CategoryID")]
        public int Id { get; set; }
        [JsonPropertyName("CategoryName")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
