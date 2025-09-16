using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using SAB00700Front.DTOs;
using System.Collections.ObjectModel;

namespace SAB00700Front
{
    public class SAB00700ViewModel : R_ViewModel<CategoryDTO>
    {
        public ObservableCollection<CategoryGridDTO> CategoryList = new ObservableCollection<CategoryGridDTO>();

        public CategoryDTO Category = new CategoryDTO();
        private readonly ICategoryService _categoryService;

        public SAB00700ViewModel()
        {

        }

        public SAB00700ViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task GetCategoryListAsync()
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
    }
}
