using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB01500Front
{
    public class ProductViewModel : R_ViewModel<ProductDTO>
    {
        public ProductDTO Product { get; set; } = new ProductDTO();
        public List<CategoryDTO> CategoryList { get; set; } = new List<CategoryDTO>();
        public ObservableCollection<ProductDTO> ProductList = new ObservableCollection<ProductDTO>();
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public DateTime? ReleaseDate { get; set; }

        public ProductViewModel()
        {

        }

        public ProductViewModel(IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task GetProductListAsync(int categoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _productService.GetProductsByCategoryAsync(categoryId);
                ProductList = new ObservableCollection<ProductDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetProductByIdAsync(int productId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _productService.GetProductAsync(productId);

                Product = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveProductAsync(ProductDTO poNewEntity, R_eConductorMode conductorMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (conductorMode == R_eConductorMode.Add)
                {
                    await _productService.CreateProductAsync(poNewEntity);
                }
                else
                {
                    await _productService.UpdateProductAsync(poNewEntity);
                }

                Product = await _productService.GetProductAsync(poNewEntity.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new ProductDTO { Id = productId };
                await _productService.DeleteProductAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetCategoriesAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _categoryService.GetCategoriesAsync();

                CategoryList = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
