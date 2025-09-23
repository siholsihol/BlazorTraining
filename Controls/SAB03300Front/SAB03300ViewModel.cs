using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;

namespace SAB03300Front
{
    public class SAB03300ViewModel : R_ViewModel<ProductDTO>
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ObservableCollection<ProductDTO> Products { get; set; } = new ObservableCollection<ProductDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public SAB03300ViewModel() { }

        public SAB03300ViewModel(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task GetProductsAsync()
        {
            var loProducts = await _productService.GetProductsAsync();
            loProducts = loProducts.Take(5).ToList();

            var loSelectedProduct = R_FrontUtility.ConvertCollectionToCollection<ProductDTO>(loProducts);

            Products = new ObservableCollection<ProductDTO>(loSelectedProduct);
        }

        public async Task GetCategoriesAsync()
        {
            var loCategories = await _categoryService.GetCategoriesAsync();

            Categories = loCategories;
        }
    }
}
