using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System.Collections.ObjectModel;

namespace SAB01900Front.VMs
{
    public class SAB01900CategoryViewModel : R_ViewModel<CategoryDTO>
    {
        private readonly ICategoryService _categoryService;

        public ObservableCollection<CategoryDTO> Categories { get; set; } = new ObservableCollection<CategoryDTO>();
        public CategoryDTO Category { get; set; } = new CategoryDTO();
        public List<CategoryDTO> ComboCategory { get; set; } = new List<CategoryDTO>();
        public int CurrentComboboxValue { get; set; } = 0;

        public SAB01900CategoryViewModel() { }

        public SAB01900CategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task GetComboCategoryListAsync()
        {
            var loEx = new R_Exception();

            try
            {
                ComboCategory = await _categoryService.GetCategoriesAsync();

                CurrentComboboxValue = ComboCategory.FirstOrDefault().Id;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
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

        public void DeleteCategoryAsync(int piCategoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new CategoryDTO { Id = piCategoryId };
                _categoryService.DeleteCategoryAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
