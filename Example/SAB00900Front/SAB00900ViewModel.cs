using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace SAB00900Front
{
    public class SAB00900ViewModel : R_ViewModel<ProductDTO>
    {
        public ProductDTO Product { get; set; } = new ProductDTO();
        public List<CategoryDTO> CategoryList { get; set; } = new List<CategoryDTO>();
        public List<ProductDTO> ProductList = new List<ProductDTO>();

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

        public void GetCategories()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = CategoryService.GetCategories();

                CategoryList = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void GetProductList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = ProductService.GetProducts();
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
