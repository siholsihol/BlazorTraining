namespace BatchAndExcelCommon.DTOs
{
    public class WriteUploadStatusDTO
    {
        public string CCOMPANY_ID { get; set; } = string.Empty;
        public string CUSER_ID { get; set; } = string.Empty;
        public string CKEY_GUID { get; set; } = string.Empty;
        public int ISTEP { get; set; } = 0;
        public string CACTION { get; set; } = string.Empty;
        public int ISTATUS { get; set; } = 0;

        public void ChangeStatus(int piStep, string pcAction, int piStatus)
        {
            ISTEP = piStep;
            CACTION = pcAction;
            ISTATUS = piStatus;
        }
    }
}
