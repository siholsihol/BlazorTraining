using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB00900Front
{
    public class ProductPageViewModel
    {
        public ObservableCollection<ProductDTO> ProductList = new ObservableCollection<ProductDTO>();
        public ProductDTO Product = new ProductDTO();

        public void GetProductList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = ProductService.GetProducts();
                ProductList = new ObservableCollection<ProductDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public ProductDTO? GetProductById(int productId)
        {
            var loEx = new R_Exception();
            ProductDTO? loReturn = null;
            try
            {
                loReturn = ProductService.GetProduct(productId);
                Product = loReturn;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

            return loReturn;
        }
    }
}
