using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB00900Front
{
    public class ProductPageViewModel
    {
        public ObservableCollection<ProductDTO> ProductList = new ObservableCollection<ProductDTO>();

        public void GetProductList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = ProductService.GenerateProduct(10);
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
