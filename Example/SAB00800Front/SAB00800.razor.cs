namespace SAB00800Front
{
    public partial class SAB00800
    {
        IEnumerable<TreeDTO> FlatData { get; set; }
        //IEnumerable<object> ExpandedItems { get; set; } = new List<TreeDTO>();
        public IEnumerable<object> SelectedItems { get; set; } = new List<object>();

        protected override Task R_Init_From_Master(object poParameter)
        {
            FlatData = GetFlatData();

            //ExpandedItems = FlatData.Where(x => x.HasChildren == true).ToList();

            SelectedItems = new List<object>() { FlatData.FirstOrDefault() };

            return Task.CompletedTask;
        }

        List<TreeDTO> GetFlatData()
        {
            List<TreeDTO> items = new List<TreeDTO>();

            items.Add(new TreeDTO()
            {
                CPARENT = null,
                CCATEGORY_ID = "C2001",
                CCATEGORY_NAME = "Metro Park",
                ILEVEL = 0
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2011",
                CCATEGORY_NAME = "Tower 1",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2012",
                CCATEGORY_NAME = "Tower 2",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "C2013",
                CCATEGORY_NAME = "Tower 3",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = "C2001",
                CCATEGORY_ID = "CTG01",
                CCATEGORY_NAME = "Tenant",
                ILEVEL = 1
            });
            items.Add(new TreeDTO()
            {
                CPARENT = null,
                CCATEGORY_ID = "C2002",
                CCATEGORY_NAME = "Parent 2",
                ILEVEL = 0
            });

            items.Where(x => string.IsNullOrWhiteSpace(x.CPARENT) && items.Where(y => y.CPARENT == x.CCATEGORY_ID).Count() > 0).ToList().ForEach(x => x.LHAS_CHILDREN = true);

            items.ForEach(x => x.CCATEGORY_NAME = $"[{x.ILEVEL}] {x.CCATEGORY_ID} - {x.CCATEGORY_NAME}");

            return items;
        }

        public class TreeDTO
        {
            public string CPARENT { get; set; }
            public string CCATEGORY_ID { get; set; }
            public string CCATEGORY_NAME { get; set; }
            public int ILEVEL { get; set; }
            public bool LHAS_CHILDREN { get; set; }
        }

        public class TreeDetailDTO
        {
            public string CPARENT { get; set; }
            public string CCATEGORY_ID { get; set; }
            public string CCATEGORY_NAME { get; set; }
            public int ILEVEL { get; set; }
            public string CNOTE { get; set; }
        }
    }
}
