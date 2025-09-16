using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System.Collections.ObjectModel;

namespace SAB01200Front
{
    public class SAB01200CategoryViewModel : R_ViewModel<CategoryDTO>
    {
        private readonly ICategoryService _categoryService;

        public ObservableCollection<CategoryDTO> Categories { get; set; } = new ObservableCollection<CategoryDTO>();
        public CategoryDTO Category { get; set; } = new CategoryDTO();
        public int CurrentCategoryId { get; set; }

        public SAB01200CategoryViewModel()
        {

        }

        public SAB01200CategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task GetCategoryListAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _categoryService.GetCategoriesAsync();
                Categories = new ObservableCollection<CategoryDTO>(loResult);
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
    }
}
