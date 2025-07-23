using R_BlazorFrontEnd;

namespace SAB03100Front.TextBox
{
    public class TextBoxViewModel : R_ViewModel<TextBoxDTO>
    {
        public TextBoxDTO GetData()
        {
            return new TextBoxDTO()
            {
                Id = "emp01",
                FirstName = "Employee 1",
                Age = 1,
                DateOfBirth = DateTime.MinValue,
            };
        }
    }
}
