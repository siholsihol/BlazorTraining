using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB01900Front.VMs
{
    public class SAB01900ProductViewModel : R_ViewModel<ProductDTO>
    {
        private readonly IProductService _productService;

        public ObservableCollection<ProductDTO> Products { get; set; } = new ObservableCollection<ProductDTO>();
        public ProductDTO Product { get; set; } = new ProductDTO();

        public SAB01900ProductViewModel() { }

        public SAB01900ProductViewModel(IProductService productService)
        {
            _productService = productService;
        }

        public async Task GetProductListByCategoryAsync(int categoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _productService.GetProductsByCategoryAsync(categoryId);
                Products = new ObservableCollection<ProductDTO>(loResult);
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
    }
}
