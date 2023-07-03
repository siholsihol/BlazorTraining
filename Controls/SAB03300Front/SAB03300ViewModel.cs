using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;

namespace SAB03300Front
{
    public class SAB03300ViewModel : R_ViewModel<ProductDTO>
    {
        public ObservableCollection<ProductDTO> Products { get; set; } = new ObservableCollection<ProductDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public SAB03300ViewModel()
        {

        }
        public void GetProducts()
        {
            var loProducts = ProductService.GetProducts();
            var loSelectedProduct = R_FrontUtility.ConvertCollectionToCollection<ProductDTO>(loProducts);

            Products = new ObservableCollection<ProductDTO>(loSelectedProduct);
        }

        public void GetCategories()
        {
            var loCategories = CategoryService.GetCategories();

            Categories = loCategories;
        }
    }
}
