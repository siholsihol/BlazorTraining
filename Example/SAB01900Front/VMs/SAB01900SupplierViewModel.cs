using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using System.Collections.ObjectModel;

namespace SAB01900Front.VMs
{
    public class SAB01900SupplierViewModel : R_ViewModel<SupplierDTO>
    {
        private readonly ISupplierService _supplierService;

        public ObservableCollection<SupplierDTO> Suppliers { get; set; } = new ObservableCollection<SupplierDTO>();
        public SupplierDTO Supplier { get; set; } = new SupplierDTO();

        public SAB01900SupplierViewModel() { }

        public SAB01900SupplierViewModel(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        //public async Task GetSuppliersByCategoryAsync(int categoryId)
        //{
        //    var loEx = new R_Exception();

        //    try
        //    {
        //        var loResult = await _supplierService.GetSuppliersByCategory(categoryId);
        //        Suppliers = new ObservableCollection<SupplierDTO>(loResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }

        //    loEx.ThrowExceptionIfErrors();
        //}
    }
}
