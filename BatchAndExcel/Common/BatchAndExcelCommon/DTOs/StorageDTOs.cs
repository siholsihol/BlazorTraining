using R_APICommonDTO;
using System;

namespace BatchAndExcelCommon.DTOs
{
    public class GetAttachmentParameterDTO
    {
        public string StorageId { get; set; }
    }

    public class GetAttachmentDTO
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Url { get; set; }
        public byte[] Data { get; set; }
    }

    public class AddAttachmentParameterDTO
    {
        public string CompanyId { get; set; }
        public string EmployeeId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public Byte[] Data { get; set; }
        public string UserId { get; set; }
    }

    public class UpdateAttachmentParameterDTO
    {
        public string StorageId { get; set; }
        public Byte[] Data { get; set; }
        public string UserId { get; set; }
    }

    public class DeleteAttachmentParameterDTO
    {
        public string StorageId { get; set; }
        public string UserId { get; set; }
    }

    public class StorageResultDTO<T> : StorageResultDTO
    {
        public T Data { get; set; }
    }

    public class StorageResultDTO : R_APIResultBaseDTO
    {
    }
}
