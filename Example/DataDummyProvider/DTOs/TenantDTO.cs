namespace DataDummyProvider.DTOs
{
    public class TenantDTO
    {
        public string CPARENT { get; set; }
        public string CCATEGORY_ID { get; set; }
        public string CCATEGORY_NAME { get; set; }
        public int ILEVEL { get; set; }
        public bool LHAS_CHILDREN { get; set; }
        public string CNOTE { get; set; }
    }
}
