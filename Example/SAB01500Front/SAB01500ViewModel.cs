﻿using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using SAB01500Front.DTOs;
using System.Collections.ObjectModel;

namespace SAB01500Front
{
    public class SAB01500ViewModel : R_ViewModel<CategoryDTO>
    {
        public ObservableCollection<CategoryGridDTO> CategoryList = new ObservableCollection<CategoryGridDTO>();

        public CategoryDTO Category = new CategoryDTO();

        #region Category
        public void GetCategoryList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = CategoryService.GetCategories();
                var loGridData = R_FrontUtility.ConvertCollectionToCollection<CategoryGridDTO>(loResult);
                CategoryList = new ObservableCollection<CategoryGridDTO>(loGridData);
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
        #endregion
    }
}
