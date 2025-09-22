using System.Text.Json.Serialization;

namespace DataProvider.DTOs
{
    public class SupplierDTO
    {
        [JsonPropertyName("SupplierID")]
        public int Id { get; set; }
        [JsonPropertyName("SupplierName")]
        public string Name { get; set; } = string.Empty;
    }
}
