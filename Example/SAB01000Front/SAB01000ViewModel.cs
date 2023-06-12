using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using System.Collections.ObjectModel;

namespace SAB01000Front
{
    public class SAB01000ViewModel : R_ViewModel<ProductDTO>
    {
        //private ProductService _productService = new ProductService();
        //private CategoryService _categoryService = new CategoryService();

        public ObservableCollection<ProductDTO> Products { get; set; } = new ObservableCollection<ProductDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public void GenerateProduct(int piCount)
        {
            var loProducts = ProductService.GenerateProduct(piCount);

            Products = new ObservableCollection<ProductDTO>(loProducts);
        }

        public void GenerateCategory()
        {
            var loCategories = CategoryService.GenerateCategory();

            Categories = loCategories;
        }
    }
}
