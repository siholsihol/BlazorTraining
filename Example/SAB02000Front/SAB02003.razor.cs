using R_BlazorFrontEnd.Controls;
using SAB02000Front.DTO;
using System.Collections.ObjectModel;

namespace SAB02000Front
{
    public partial class SAB02003 : R_Page
    {
        private List<string> _stringList = new List<string>()
        {
            "Apple",
            "Banana",
            "Cherry",
            "Date",
            "Elderberry",
            "Fig",
            "Grape",
            "Honeydew"
        };
        private List<ItemDTO> _itemList = new List<ItemDTO>()
        {
            new ItemDTO("Item 1"),
            new ItemDTO("Item 2"),
            new ItemDTO("Item 3"),
            new ItemDTO("Item 4"),
            new ItemDTO("Item 5")
        };

        private ObservableCollection<ItemDTO> _itemObservableList = new ObservableCollection<ItemDTO>()
        {
            new ItemDTO("Item A"),
            new ItemDTO("Item B"),
            new ItemDTO("Item C"),
            new ItemDTO("Item D"),
            new ItemDTO("Item E")
        };

        private string _stringListValue = "";
        private bool _checkboxValue = false;
        private string _itemListValue = "";
        private List<string> _comboBoxMultiSelectValue = new List<string>();
        private DateTime _dateValue = DateTime.Now;
        private ItemDTO _listBoxValue = new ItemDTO();
        private int _numericValue = 0;
        private string _rtfValue = @"{\rtf1rtf-value}";
    }
}
