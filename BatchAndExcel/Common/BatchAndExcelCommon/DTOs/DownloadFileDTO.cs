using R_APICommonDTO;

namespace BatchAndExcelCommon.DTOs
{
    public class DownloadFileDTO : R_APIResultBaseDTO
    {
        public byte[] FileBytes { get; set; }
    }
}
