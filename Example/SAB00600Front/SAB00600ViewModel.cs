using DataProvider.DTOs;
using DataProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System.Collections.ObjectModel;

namespace SAB00600Front
{
    public class SAB00600ViewModel : R_ViewModel<CustomerDTO>
    {
        public ObservableCollection<CustomerDTO> CustomerList { get; set; } = new ObservableCollection<CustomerDTO>();

        public CustomerDTO Customer = new();
        private readonly ICustomerService _customerService;

        //public List<GenderDTO> Genders { get; set; } = new List<GenderDTO>();

        public SAB00600ViewModel()
        {

        }

        public SAB00600ViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task GetCustomerList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _customerService.GetCustomersAsync();
                CustomerList = new ObservableCollection<CustomerDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetCustomerByIdAsync(string customerId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _customerService.GetCustomerAsync(customerId);

                Customer = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveCustomerAsync(CustomerDTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (peCRUDMode == eCRUDMode.AddMode)
                {
                    await _customerService.CreateCustomerAsync(poNewEntity);
                }
                else
                {
                    await _customerService.UpdateCustomerAsync(poNewEntity);
                }

                Customer = await _customerService.GetCustomerAsync(poNewEntity.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new CustomerDTO { Id = customerId };
                await _customerService.DeleteCustomerAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        //public async Task GetGendersAsync()
        //{
        //    var loGenders = await _customerService.GetGendersAsync();

        //    Genders = loGenders;
        //}
    }
}
