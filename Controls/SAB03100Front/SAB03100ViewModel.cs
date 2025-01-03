using R_BlazorFrontEnd;

namespace SAB03100Front
{
    public class SAB03100ViewModel : R_ViewModel<EmployeeDTO>
    {
        public EmployeeDTO Employee = new EmployeeDTO();
    }

    public class EmployeeDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public int Gender { get; set; }
    }
}
