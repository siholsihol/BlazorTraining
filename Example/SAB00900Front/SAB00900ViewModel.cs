using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace SAB00900Front
{
    public class SAB00900ViewModel : R_ViewModel<ProductDTO>
    {
        public ProductDTO Product { get; set; } = new ProductDTO();
        public List<CategoryDTO> CategoryList { get; set; } = new List<CategoryDTO>();
        public List<ProductDTO> ProductList { get; set; } = new List<ProductDTO>();

        private readonly IProductService _productService = default!;
        private readonly ICategoryService _categoryService = default!;

        public SAB00900ViewModel() { }

        public SAB00900ViewModel(
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task GetProductById(int productId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _productService.GetProductAsync(productId);

                Product = loResult ?? new ProductDTO();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveProduct(ProductDTO poNewEntity, R_eConductorMode conductorMode)
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

                Product = await _productService.GetProductAsync(poNewEntity.Id) ?? new ProductDTO();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteProduct(int productId)
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

        public async Task GetCategories()
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

        public async Task GetProductList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _productService.GetProductsAsync();
                ProductList = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
