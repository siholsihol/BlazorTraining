using R_APICommonDTO;
using System;

namespace BatchAndExcelCommon.DTOs
{
    public class StorageDTO
    {
        public string CompanyId { get; set; }
        public string EmployeeId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string StorageId { get; set; }
    }

    public class EmployeeAttachmentDTO
    {
        public string CompanyId { get; set; }
        public string EmployeeId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public Byte[] Data { get; set; }
    }

    public class StorageResultDTO : R_APIResultBaseDTO
    {
    }
}
