using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System.Collections.ObjectModel;

namespace SAB01200Front
{
    public class SAB01200CategoryViewModel : R_ViewModel<CategoryDTO>
    {
        public ObservableCollection<CategoryDTO> Categories { get; set; } = new ObservableCollection<CategoryDTO>();

        public CategoryDTO Category { get; set; } = new CategoryDTO();

        public int CurrentCategoryId { get; set; }

        public void GetCategoryList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = CategoryService.GetCategories();
                Categories = new ObservableCollection<CategoryDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void GetCategoryById(int piCategoryId)
        {
            var loEx = new R_Exception();

            try
            {
                Category = CategoryService.GetCategory(piCategoryId);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void SaveCategory(CategoryDTO poEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (peCRUDMode == eCRUDMode.AddMode)
                {
                    CategoryService.CreateCategory(poEntity);
                }
                else
                {
                    CategoryService.UpdateCategory(poEntity);
                }

                Category = CategoryService.GetCategory(poEntity.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void DeleteCategory(int piCategoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new CategoryDTO { Id = piCategoryId };
                CategoryService.DeleteCategory(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
