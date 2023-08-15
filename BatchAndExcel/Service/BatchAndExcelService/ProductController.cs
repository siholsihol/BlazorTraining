using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Cache;
using R_Common;
using R_CommonFrontBackAPI;
using R_ReportFastReportBack;
using System.Collections;

namespace BatchAndExcelService
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private R_ReportFastReportBackClass _reportCls;
        private ProductParameterDTO _productParameter;

        #region Instantiate
        public ProductController()
        {
            _reportCls = new R_ReportFastReportBackClass();
            _reportCls.R_InstantiateMainReportWithFileName += _reportCls_R_InstantiateMainReportWithFileName;
            _reportCls.R_GetMainDataAndName += _reportCls_R_GetMainDataAndName;
            _reportCls.R_SetNumberAndDateFormat += _reportCls_R_SetNumberAndDateFormat;
        }

        private void _reportCls_R_InstantiateMainReportWithFileName(ref string pcFileTemplate)
        {
            pcFileTemplate = "Reports\\ProductObject.frx";
        }

        private void _reportCls_R_GetMainDataAndName(ref ArrayList poData, ref string pcDataSourceName)
        {
            poData.Add(GenerateData(_productParameter.GenerateCountProduct));
            pcDataSourceName = "ResponseDataModel";
        }

        private void _reportCls_R_SetNumberAndDateFormat(ref R_ReportFormatDTO poReportFormat)
        {
            poReportFormat.DecimalSeparator = R_BackGlobalVar.REPORT_FORMAT_DECIMAL_SEPARATOR;
            poReportFormat.GroupSeparator = R_BackGlobalVar.REPORT_FORMAT_GROUP_SEPARATOR;
            poReportFormat.DecimalPlaces = R_BackGlobalVar.REPORT_FORMAT_DECIMAL_PLACES;
            poReportFormat.ShortDate = R_BackGlobalVar.REPORT_FORMAT_SHORT_DATE;
            poReportFormat.ShortTime = R_BackGlobalVar.REPORT_FORMAT_SHORT_TIME;
        }

        private ProductResult GenerateData(int pnCount)
        {
            ProductResult loRtn = new ProductResult()
            {
                Header = "Product Header",
                Footer = "Product Footer",
                ColumnProduct = new ProductColumnDTO()
            };
            List<BaseProductDTO> loCollection = new List<BaseProductDTO>();
            for (int i = 1; i <= pnCount; i++)
            {
                loCollection.Add(new BaseProductDTO()
                {
                    Id = $"ID {i}",
                    Quantity = i + 1,
                    Price = 2.23m + i * 1.7m
                }
               );
            }
            loRtn.Products = loCollection;

            return loRtn;
        }
        #endregion

        [HttpPost]
        public R_DownloadFileResultDTO AllProductPostWithoutParameter()
        {
            R_Exception loException = new R_Exception();
            R_DownloadFileResultDTO loRtn = null;
            try
            {
                var loParam = new ProductParameterDTO()
                {
                    GenerateCountProduct = 5
                };

                loRtn = new R_DownloadFileResultDTO();
                R_DistributedCache.R_Set(loRtn.GuidResult, R_NetCoreUtility.R_SerializeObjectToByte<ProductParameterDTO>(loParam));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public R_DownloadFileResultDTO AllProductPost(ProductParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            R_DownloadFileResultDTO loRtn = null;
            try
            {
                loRtn = new R_DownloadFileResultDTO();
                R_DistributedCache.R_Set(loRtn.GuidResult, R_NetCoreUtility.R_SerializeObjectToByte<ProductParameterDTO>(poParameter));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpGet]
        public FileStreamResult AllStreamProductGet(string pcGuid)
        {
            R_Exception loException = new R_Exception();
            FileStreamResult loRtn = null;
            try
            {
                //Get Parameter
                _productParameter = R_NetCoreUtility.R_DeserializeObjectFromByte<ProductParameterDTO>(R_DistributedCache.Cache.Get(pcGuid));
                loRtn = new FileStreamResult(_reportCls.R_GetStreamReport(), R_ReportUtility.GetMimeType(R_FileType.PDF));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}
