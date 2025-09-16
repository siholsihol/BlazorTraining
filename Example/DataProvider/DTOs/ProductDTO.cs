using System;

namespace DataProvider.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool Active { get; set; } = true;
        public int CategoryId { get; set; }
        //public int SupplierId { get; set; }
    }
}
