namespace SAB00800Front.DTOs
{
    public class TenantTreeDTO
    {
        public string CPARENT { get; set; }
        public string CCATEGORY_ID { get; set; }
        //public string CCATEGORY_NAME { get; set; }
        public string CCATEGORY_NAME_DISPLAY { get; set; }
        //public int ILEVEL { get; set; }
        public bool LHAS_CHILDREN { get; set; }
    }
}
