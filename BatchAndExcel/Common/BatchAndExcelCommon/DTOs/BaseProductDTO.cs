namespace BatchAndExcelCommon.DTOs
{
    public class BaseHeaderResult
    {
        public BaseHeaderDTO BaseHeaderData { get; set; }
    }

    public static class GenerateBaseModel
    {
        public static BaseHeaderResult DefaultData()
        {
            BaseHeaderResult loRtn = new BaseHeaderResult()
            {
                BaseHeaderData = new BaseHeaderDTO()
                {
                    CompanyId = "C-01",
                    CompanyName = "Company 01",
                    UserId = "U-01",
                    UserName = "User 01"
                }
            };
            return loRtn;
        }
    }

    public class BaseHeaderDTO
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
