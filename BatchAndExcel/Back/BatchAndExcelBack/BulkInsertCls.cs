using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.SqlClient;
using System.Transactions;

namespace BatchAndExcelBack
{
    public class BulkInsertCls : R_IBatchProcess
    {
        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            var loEx = new R_Exception();

            try
            {
                var liFinishFlag = 1; //0=Process, 1=Success, 9=Fail
                var loObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<EmployeeDTO>>(poBatchProcessPar.BigObject);

                var loDb = new R_Db();

                using (var transScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    var loConn = loDb.GetConnection();

                    var lcQuery = "EXEC RSP_WriteUploadProcessStatus @COMPANY_ID, @USER_ID, @KEY_GUID, 0, 'Process Start', 0";
                    var loCmd = loDb.GetCommand();
                    loCmd.CommandText = lcQuery;

                    loDb.R_AddCommandParameter(loCmd, "@COMPANY_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCmd, "@USER_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                    loDb.R_AddCommandParameter(loCmd, "@KEY_GUID", System.Data.DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);
                    loDb.SqlExecQuery(loConn, loCmd, false);

                    lcQuery = "CREATE TABLE #Employee (Id varchar(50), FirstName varchar(255), Gender int)";
                    loDb.SqlExecNonQuery(lcQuery, loConn, false);

                    lcQuery = "SELECT * FROM #Employee";
                    var loResult = loDb.SqlExecQuery(lcQuery, loConn, false);
                    var loDto = R_Utility.R_ConvertTo<EmployeeDTO>(loResult);

                    loDb.R_BulkInsert<EmployeeDTO>((SqlConnection)loConn, "#Employee", loObject);

                    lcQuery = "SELECT * FROM #Employee";
                    loResult = loDb.SqlExecQuery(lcQuery, loConn, false);
                    loDto = R_Utility.R_ConvertTo<EmployeeDTO>(loResult);

                    lcQuery = "EXEC RSP_WriteUploadProcessStatus @COMPANY_ID, @USER_ID, @KEY_GUID, @Count, 'Process Finish', @Finish";
                    loCmd = loDb.GetCommand();
                    loCmd.CommandText = lcQuery;

                    loDb.R_AddCommandParameter(loCmd, "@COMPANY_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCmd, "@USER_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                    loDb.R_AddCommandParameter(loCmd, "@KEY_GUID", System.Data.DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);

                    loDb.R_AddCommandParameter(loCmd, "@Count", System.Data.DbType.Int32, 50, loObject.Count);
                    loDb.R_AddCommandParameter(loCmd, "@Finish", System.Data.DbType.Int32, 50, liFinishFlag);
                    loDb.SqlExecQuery(loConn, loCmd, false);

                    transScope.Complete();
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}
