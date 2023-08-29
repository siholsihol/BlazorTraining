using R_BlazorFrontEnd;
using System.Data;

namespace BatchAndExcel.Pages
{
    public class ReadFromExcelCls
    {
        public DataSet Read(R_IExcel excel, byte[] byteFile)
        {
            return excel.R_ReadFromExcel(byteFile);
        }
    }
}
