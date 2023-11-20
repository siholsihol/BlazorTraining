using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;

namespace BatchAndExcelBack
{
    public class SaveBatchWithBulkCopyCls : R_IBatchProcess
    {
        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = null;
            DbCommand loCommand = null;
            DbConnection loConn = null;

            try
            {
                var loObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<EmployeeBatchProcessDTO>>(poBatchProcessPar.BigObject);

                var lcCmd = "select SeqNo=0,*,ErrorMsg = convert(varchar(1000),'') into #raw_data from dbo.TestEmployeeTable where 0=1";

                loDb = new R_Db();
                loConn = loDb.GetConnection();

                loDb.SqlExecNonQuery(lcCmd, loConn, false);

                // Prepare mapping for different column
                var loMapping = new Dictionary<string, string>();
                loMapping.Add("CompanyId", "CoId");
                loMapping.Add("EmployeeId", "EmpId");
                loMapping.Add("TotalChildren", "TotalChild");
                loDb.R_BulkInsert((System.Data.SqlClient.SqlConnection)loConn, "#raw_data", loObject, loMapping);

                lcCmd = "SaveBatchEmployee";

                loCommand = loDb.GetCommand();
                loCommand.CommandText = lcCmd;
                loCommand.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCommand, "@CoId", DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@UserId", DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCommand, "@KeyGUID", DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);

                loDb.SqlExecNonQuery(loConn, loCommand, true);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (!(loConn.State == ConnectionState.Closed))
                        loConn.Close();
                    loConn.Dispose();
                    loConn = null;
                }
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}
