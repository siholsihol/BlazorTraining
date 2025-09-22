namespace DataProvider.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Discontinued { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}
