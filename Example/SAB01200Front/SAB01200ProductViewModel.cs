using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB01200Front
{
    public class SAB01200ProductViewModel : R_ViewModel<ProductDTO>
    {
        public ObservableCollection<ProductDTO> Products { get; set; } = new ObservableCollection<ProductDTO>();
        public ProductDTO Product { get; set; } = new ProductDTO();

        public void GetProductList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = ProductService.GenerateProduct(10);
                Products = new ObservableCollection<ProductDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void GetProductById(int productId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = ProductService.GetProduct(productId);

                Product = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void SaveProduct(ProductDTO poNewEntity, R_eConductorMode conductorMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (conductorMode == R_eConductorMode.Add)
                {
                    ProductService.CreateProduct(poNewEntity);
                }
                else
                {
                    ProductService.UpdateProduct(poNewEntity);
                }

                Product = ProductService.GetProduct(poNewEntity.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void DeleteProduct(int productId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new ProductDTO { Id = productId };
                ProductService.DeleteProduct(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
