using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB01000Front
{
    public class CategoryPageViewModel
    {
        public ObservableCollection<CategoryDTO> CategoryList = new ObservableCollection<CategoryDTO>();

        public void GetCategoryList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = CategoryService.GetCategories();
                CategoryList = new ObservableCollection<CategoryDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
