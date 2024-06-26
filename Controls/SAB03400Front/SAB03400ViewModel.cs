﻿using DataDummyProvider.DTOs;
using DataDummyProvider.Services;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Helpers;
using System.Collections.ObjectModel;

namespace SAB03400Front
{
    public class SAB03400ViewModel : R_ViewModel<ProductDTO>
    {
        public ObservableCollection<ProductDTO> Products { get; set; } = new ObservableCollection<ProductDTO>();
        public ObservableCollection<ProductDTO> Products2 { get; set; } = new ObservableCollection<ProductDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        public SAB03400ViewModel()
        {

        }
        public void GetProducts()
        {
            var loProducts = ProductService.GetProducts().Take(5).ToList();
            var loSelectedProduct = R_FrontUtility.ConvertCollectionToCollection<ProductDTO>(loProducts);

            Products = new ObservableCollection<ProductDTO>(loSelectedProduct);

            //var loProductsGrid1 = ProductService.GetNewProducts();
            //var loSelectedProductGrid1 = R_FrontUtility.ConvertCollectionToCollection<ProductDTO>(loProductsGrid1);

            Products2 = new ObservableCollection<ProductDTO>();
        }

        public void GetCategories()
        {
            var loCategories = CategoryService.GetCategories();

            Categories = loCategories;
        }
    }
}
