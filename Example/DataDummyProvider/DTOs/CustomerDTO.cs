namespace DataDummyProvider.DTOs
{
    public class CustomerDTO
    {
        public bool Selected { get; set; } = true;
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string GenderId { get; set; }
    }

    public class GenderDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
