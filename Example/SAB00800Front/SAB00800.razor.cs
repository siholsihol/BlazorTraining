﻿using DataDummyProvider.DTOs;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using SAB00800Front.DTOs;

namespace SAB00800Front
{
    public partial class SAB00800
    {
        private SAB00800ViewModel _viewModel = new();
        private R_Conductor _conductorRef;
        private R_TreeView<TenantTreeDTO> _treeRef;

        //ObservableCollection<TreeDTO> FlatData { get; set; } = new ObservableCollection<TreeDTO>();
        //IEnumerable<object> ExpandedItems { get; set; } = new List<TreeDTO>();
        //public IEnumerable<object> SelectedItems { get; set; } = new List<object>();

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _treeRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Tree_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel.GetTenantList();

                eventArgs.ListEntityResult = _viewModel.TenantList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Conductor_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantDTO>(eventArgs.Data);
                _viewModel.GetTenantById(loParam.CCATEGORY_ID);

                eventArgs.Result = _viewModel.Tenant;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Conductor_Validation(R_ValidationEventArgs eventArgs)
        {
            //var loEx = new R_Exception();

            //try
            //{
            //    var loData = (CategoryDTO)eventArgs.Data;

            //    if (string.IsNullOrWhiteSpace(loData.Name))
            //        loEx.Add("", "Please fill Category Name.");
            //}
            //catch (Exception ex)
            //{
            //    loEx.Add(ex);
            //}

            //loEx.ThrowExceptionIfErrors();
        }

        private void Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (TenantDTO)eventArgs.Data;
                _viewModel.SaveCategory(loParam, (eCRUDMode)eventArgs.ConductorMode);

                eventArgs.Result = _viewModel.Tenant;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Conductor_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            //var loEx = new R_Exception();

            //try
            //{
            //    var loParam = (CategoryDTO)eventArgs.Data;
            //    _viewModel.DeleteCategory(loParam.Id);
            //}
            //catch (Exception ex)
            //{
            //    loEx.Add(ex);
            //}

            //loEx.ThrowExceptionIfErrors();
        }

        private void Conductor_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
        {
            var loConductorData = (TenantDTO)eventArgs.Data;
            eventArgs.GridData = new TenantTreeDTO
            {
                CPARENT = loConductorData.CPARENT,
                CCATEGORY_ID = loConductorData.CCATEGORY_ID,
                CCATEGORY_NAME_DISPLAY = $"[{loConductorData.ILEVEL}] {loConductorData.CCATEGORY_ID} - {loConductorData.CCATEGORY_NAME}",
                LHAS_CHILDREN = !string.IsNullOrWhiteSpace(loConductorData.CPARENT)
            };
        }
    }
}