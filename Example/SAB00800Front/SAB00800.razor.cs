using DataDummyProvider.DTOs;
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

        private void Conductor_Saving(R_SavingEventArgs eventArgs)
        {
            var loData = (TenantDTO)eventArgs.Data;
            loData.CPARENT = string.IsNullOrWhiteSpace(loData.CPARENT) ? null : loData.CPARENT;
        }

        private void Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (TenantDTO)eventArgs.Data;
                _viewModel.SaveTenant(loParam, (eCRUDMode)eventArgs.ConductorMode);

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
            var loEx = new R_Exception();

            try
            {
                var loParam = (TenantDTO)eventArgs.Data;
                _viewModel.DeleteTenant(loParam.CCATEGORY_ID);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void Conductor_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
        {
            var loConductorData = (TenantDTO)eventArgs.Data;
            eventArgs.GridData = new TenantTreeDTO
            {
                CPARENT = loConductorData.CPARENT,
                CCATEGORY_ID = loConductorData.CCATEGORY_ID,
                CCATEGORY_NAME_DISPLAY = $"[{loConductorData.ILEVEL}] {loConductorData.CCATEGORY_ID} - {loConductorData.CCATEGORY_NAME}"
            };
        }

        private void Tree_R_RefreshTreeViewState(R_RefreshTreeViewStateEventArgs eventArgs)
        {
            var loTreeList = (List<TenantTreeDTO>)eventArgs.TreeViewList;

            loTreeList.ForEach(x => x.LHAS_CHILDREN = string.IsNullOrWhiteSpace(x.CPARENT) && loTreeList.Where(y => y.CPARENT == x.CCATEGORY_ID).Count() > 0 ? true : false);

            //eventArgs.ExpandedList = loTreeList.Where(x => x.LHAS_CHILDREN == true).ToList();
        }
    }
}
