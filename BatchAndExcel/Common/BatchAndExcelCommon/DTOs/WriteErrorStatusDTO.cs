namespace BatchAndExcelCommon.DTOs
{
    public class WriteErrorStatusDTO
    {
        public string CCOMPANY_ID { get; set; } = string.Empty;
        public string CUSER_ID { get; set; } = string.Empty;
        public string CKEY_GUID { get; set; } = string.Empty;
        public int ISEQ_NO { get; set; } = 0;
        public string CERROR_MESSAGE { get; set; } = string.Empty;
    }
}
