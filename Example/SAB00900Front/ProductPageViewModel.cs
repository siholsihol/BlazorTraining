using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB00900Front
{
    public class ProductPageViewModel
    {
        private readonly IProductService _productService;
        public ObservableCollection<ProductDTO> ProductList = new ObservableCollection<ProductDTO>();

        public ProductPageViewModel()
        {

        }

        public ProductPageViewModel(IProductService productService)
        {
            _productService = productService;
        }

        public async Task GetProductList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _productService.GetProductsAsync();

                ProductList = new ObservableCollection<ProductDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
