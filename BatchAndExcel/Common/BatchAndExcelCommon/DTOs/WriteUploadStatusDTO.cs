namespace BatchAndExcelCommon.DTOs
{
    public class WriteUploadStatusDTO
    {
        public string CCOMPANY_ID { get; set; } = string.Empty;
        public string CUSER_ID { get; set; } = string.Empty;
        public string CKEY_GUID { get; set; } = string.Empty;
        public int ICOUNT { get; set; } = 0;
        public string CACTION { get; set; } = string.Empty;
        public int ISTATUS { get; set; } = 0;

        public void ChangeStatus(int piCount, string pcAction, int piStatus)
        {
            ICOUNT = piCount;
            CACTION = pcAction;
            ISTATUS = piStatus;
        }
    }
}
