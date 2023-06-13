using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Helpers;
using SAB01000Front.DTOs;
using System.Collections.ObjectModel;

namespace SAB01000Front
{
    public class SAB01000ViewModel : R_ViewModel<SelectedProductDTO>
    {
        public ObservableCollection<SelectedProductDTO> Products { get; set; } = new ObservableCollection<SelectedProductDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public SAB01000ViewModel()
        {

        }
        public void GenerateProduct(int piCount)
        {
            var loProducts = ProductService.GenerateProduct(piCount);
            var loSelectedProduct = R_FrontUtility.ConvertCollectionToCollection<SelectedProductDTO>(loProducts);

            Products = new ObservableCollection<SelectedProductDTO>(loSelectedProduct);
        }

        public void GenerateCategory()
        {
            var loCategories = CategoryService.GenerateCategory();

            Categories = loCategories;
        }
    }
}
