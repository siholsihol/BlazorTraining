using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
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
        public DateTime? ReleaseDate { get; set; }

        public void GetProductList(int categoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = ProductService.GetProductsByCategory(categoryId);
                ProductList = new ObservableCollection<ProductDTO>(loResult);
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
    }
}
