using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Helpers;
using SAB01000Front.DTOs;
using System.Collections.ObjectModel;

namespace SAB01000Front
{
    public class SAB01000ViewModel : R_ViewModel<SelectedProductDTO>
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ObservableCollection<SelectedProductDTO> Products { get; set; } = new ObservableCollection<SelectedProductDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public SAB01000ViewModel()
        {

        }

        public SAB01000ViewModel(IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task GetProductsAsync()
        {
            var loProducts = await _productService.GetProductsAsync();
            var loSelectedProduct = R_FrontUtility.ConvertCollectionToCollection<SelectedProductDTO>(loProducts);

            Products = new ObservableCollection<SelectedProductDTO>(loSelectedProduct);
        }

        public async Task GetCategoriesAsync()
        {
            var loCategories = await _categoryService.GetCategoriesAsync();

            Categories = loCategories;
        }
    }
}
