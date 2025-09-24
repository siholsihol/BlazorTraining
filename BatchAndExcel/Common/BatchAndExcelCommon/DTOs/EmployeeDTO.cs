using System;

namespace BatchAndExcelCommon.DTOs
{
    public class EmployeeDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string HireDate { get; set; }
        public DateTime? DHIRE_DATE { get; set; }

        public bool WNI { get; set; }
    }
}
