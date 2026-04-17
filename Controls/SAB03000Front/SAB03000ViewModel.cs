using R_BlazorFrontEnd;

namespace SAB03000Front
{
    public class SAB03000ViewModel : R_ViewModel<DTO>
    {
        public DTO dto { get; set; } = null;
        public void GetData()
        {
            dto = new DTO();
            dto.Name = "nameeeeeee";
        }
    }

    public class DTO
    {
        public string Name { get; set; }
    }

}
