using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using SAB01300Front.DTOs;
using System.Collections.ObjectModel;

namespace SAB01300Front
{
    public class SAB01300ViewModel : R_ViewModel<CategoryDTO>
    {
        public CategoryDTO Category = new();
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ObservableCollection<CategoryGridDTO> CategoryList { get; set; } = new ObservableCollection<CategoryGridDTO>();
        public ObservableCollection<ProductDTO> Products { get; set; } = new ObservableCollection<ProductDTO>();

        public SAB01300ViewModel() { }

        public SAB01300ViewModel(
            ICategoryService categoryService,
            IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task GetCategoryList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _categoryService.GetCategoriesAsync();
                var loGridData = R_FrontUtility.ConvertCollectionToCollection<CategoryGridDTO>(loResult);
                CategoryList = new ObservableCollection<CategoryGridDTO>(loGridData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetCategoryByIdAsync(int piCategoryId)
        {
            var loEx = new R_Exception();

            try
            {
                Category = await _categoryService.GetCategoryAsync(piCategoryId);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveCategoryAsync(CategoryDTO poEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (peCRUDMode == eCRUDMode.AddMode)
                {
                    await _categoryService.CreateCategoryAsync(poEntity);
                }
                else
                {
                    await _categoryService.UpdateCategoryAsync(poEntity);
                }

                Category = await _categoryService.GetCategoryAsync(poEntity.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteCategoryAsync(int piCategoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new CategoryDTO { Id = piCategoryId };
                await _categoryService.DeleteCategoryAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task ChangeCategoryNameAsync(int piCategoryId)
        {
            var loCategory = await _categoryService.GetCategoryAsync(piCategoryId);

            loCategory.Name = "ganti nama kategori";
        }

        public async Task<CategoryDTO> GetCategoryAsync(int piCategoryId)
        {
            return await _categoryService.GetCategoryAsync(piCategoryId);
        }

        public async Task GetProductsByCategoryAsync(int categoryId)
        {
            var loProducts = await _productService.GetProductsByCategoryAsync(categoryId);

            Products = new ObservableCollection<ProductDTO>(loProducts);
        }
    }
}
