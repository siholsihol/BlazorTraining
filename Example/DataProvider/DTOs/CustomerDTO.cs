using System.Text.Json.Serialization;

namespace DataProvider.DTOs
{
    public class CustomerDTO
    {
        [JsonPropertyName("CustomerID")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("CustomerName")]
        public string CompanyName { get; set; } = string.Empty;
        public string? ContactName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
