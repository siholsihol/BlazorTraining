using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;

namespace BatchAndExcelBack
{
    public class ExcelBatchProcessWithResourcesCls : R_IBatchProcess
    {
        SaveBatchEmployeeWithResourcesResources.Resources_Dummy_Class _loRsp = new();

        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            var loEx = new R_Exception();

            try
            {
                _BatchProcess(poBatchProcessPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task _BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            var loEx = new R_Exception();
            var loDb = new R_Db();

            try
            {
                using DbConnection loConn = await loDb.GetConnectionAsync();

                //throw new Exception("Connection error yang disengaja");

                var loObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<EmployeeBatchProcessDTO>>(poBatchProcessPar.BigObject);

                var lcQuery = "select SeqNo=0,*,ErrorMsg = convert(varchar(1000),'') into #raw_data from dbo.TestEmployeeTable where 0=1";
                await loDb.SqlExecNonQueryAsync(lcQuery, loConn, false);

                // Prepare mapping for different column
                var loMapping = new Dictionary<string, string>();
                loMapping.Add("CompanyId", "CoId");
                loMapping.Add("EmployeeId", "EmpId");
                loMapping.Add("TotalChildren", "TotalChild");
                await loDb.R_BulkInsertAsync((System.Data.SqlClient.SqlConnection)loConn, "#raw_data", loObject, loMapping);

                lcQuery = "SaveBatchEmployeeWithResources";

                using DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CoId", DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@UserId", DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@KeyGUID", DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);

                await loDb.SqlExecNonQueryAsync(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.Haserror)
            {
                var lcQuery = "INSERT INTO GST_UPLOAD_ERROR_STATUS (CCOMPANY_ID, CUSER_ID, CKEY_GUID, ISEQ_NO, CERROR_MESSAGE) ";
                lcQuery += $"VALUES ('{poBatchProcessPar.Key.COMPANY_ID}', '{poBatchProcessPar.Key.USER_ID}', '{poBatchProcessPar.Key.KEY_GUID}', -1, '{loEx.ErrorList[0].ErrDescp}')";
                await loDb.SqlExecNonQueryAsync(lcQuery);

                lcQuery = $"EXEC RSP_WriteUploadProcessStatus '{poBatchProcessPar.Key.COMPANY_ID}', " +
                 $"'{poBatchProcessPar.Key.USER_ID}', " +
                 $"'{poBatchProcessPar.Key.KEY_GUID}', " +
                 $"100, '{loEx.ErrorList[0].ErrDescp}', 9";
                await loDb.SqlExecNonQueryAsync(lcQuery);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
