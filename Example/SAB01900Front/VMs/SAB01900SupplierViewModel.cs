using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace SAB01900Front.VMs
{
    public class SAB01900SupplierViewModel : R_ViewModel<SupplierDTO>
    {
        public ObservableCollection<SupplierDTO> Suppliers { get; set; } = new ObservableCollection<SupplierDTO>();
        public SupplierDTO Supplier { get; set; } = new SupplierDTO();

        public void GetSuppliersByCategory(int categoryId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = SupplierService.GetSuppliersByCategory(categoryId);
                Suppliers = new ObservableCollection<SupplierDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
