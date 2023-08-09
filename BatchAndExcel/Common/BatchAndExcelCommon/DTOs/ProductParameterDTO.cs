using System.Collections.Generic;

namespace BatchAndExcelCommon.DTOs
{
    public class ProductParameterDTO
    {
        public int GenerateCountProduct { get; set; }
    }

    public class ProductResult
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public ProductColumnDTO ColumnProduct { get; set; }
        public string Footer { get; set; }
        public List<BaseProductDTO> Products { get; set; }
    }

    public static class GenerateDataModel
    {
        public static ProductResult DefaultData()
        {
            ProductResult loData = new ProductResult()
            {
                Title = "ProductTitle",
                Header = "Product Header",
                Footer = "Product Footer",
                ColumnProduct = new ProductColumnDTO()
            };
            List<BaseProductDTO> loCollection = new List<BaseProductDTO>();
            for (int i = 0; i < 5; i++)
            {
                loCollection.Add(new BaseProductDTO()
                {
                    Id = $"ID {i}",
                    Quantity = i + 1,
                    Price = 2.23m + i * 1.7m
                }
               );
            }
            loData.Products = loCollection;

            return loData;
        }

        public static ProductWithHeaderResult DefaultDataWithHeader()
        {
            ProductWithHeaderResult loRtn = new ProductWithHeaderResult();
            loRtn.BaseHeaderData = GenerateBaseModel.DefaultData().BaseHeaderData;
            loRtn.ProductObjectData = GenerateDataModel.DefaultData();

            return loRtn;
        }
    }

    public class ProductColumnDTO
    {
        public string ColId { get; set; } = "Product ID";
        public string ColQuantity { get; set; } = "Quantity";
        public string ColPrice { get; set; } = "Price";
    }

    public class BaseProductDTO
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductWithHeaderResult : BaseHeaderResult
    {
        public ProductResult ProductObjectData { get; set; }
    }
}
