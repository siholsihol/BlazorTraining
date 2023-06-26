using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
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

        public void GetCustomerList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = CustomerService.GetCustomers();
                CustomerList = new ObservableCollection<CustomerDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void GetCustomerById(string customerId)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = CustomerService.GetCustomer(customerId);

                Customer = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void SaveCustomer(CustomerDTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (peCRUDMode == eCRUDMode.AddMode)
                {
                    CustomerService.CreateCustomer(poNewEntity);
                }
                else
                {
                    CustomerService.UpdateCustomer(poNewEntity);
                }

                Customer = CustomerService.GetCustomer(poNewEntity.Id);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void DeleteCustomer(string customerId)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new CustomerDTO { Id = customerId };
                CustomerService.DeleteCustomer(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
