using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace BatchAndExcelBack
{
    public class BulkInsertCls : R_IBatchProcessAsync
    {
        public async Task R_BatchProcessAsync(R_BatchProcessPar poBatchProcessPar)
        {
            var loEx = new R_Exception();

            try
            {
                var liFinishFlag = 1; //0=Process, 1=Success, 9=Fail
                var loObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<EmployeeDTO>>(poBatchProcessPar.BigObject);

                var loDb = new R_Db();

                using var transScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

                using DbConnection loConn = await loDb.GetConnectionAsync();
                using DbCommand loCmd = loDb.GetCommand();

                var lcQuery = "EXEC RSP_WriteUploadProcessStatus @COMPANY_ID, @USER_ID, @KEY_GUID, 0, 'Process Start', 0";
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@COMPANY_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@USER_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@KEY_GUID", System.Data.DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);
                await loDb.SqlExecQueryAsync(loConn, loCmd, false);

                lcQuery = "CREATE TABLE #Employee (Id varchar(50), FirstName varchar(255), Gender varchar(1))";
                await loDb.SqlExecNonQueryAsync(lcQuery, loConn, false);

                lcQuery = "SELECT * FROM #Employee";
                var loResult = loDb.SqlExecQuery(lcQuery, loConn, false);
                var loDto = R_Utility.R_ConvertTo<EmployeeDTO>(loResult);

                var loConvertedObject = loObject.Select(x => new Employee(x.Id, x.FirstName, x.Gender)).ToList();

                await loDb.R_BulkInsertAsync<Employee>((SqlConnection)loConn, "#Employee", loConvertedObject);

                lcQuery = "SELECT * FROM #Employee";
                loResult = await loDb.SqlExecQueryAsync(lcQuery, loConn, false);
                loDto = R_Utility.R_ConvertTo<EmployeeDTO>(loResult);

                lcQuery = "EXEC RSP_WriteUploadProcessStatus @COMPANY_ID, @USER_ID, @KEY_GUID, @Count, 'Process Finish', @Finish";
                loCmd.CommandText = lcQuery;

                loCmd.Parameters.Clear();
                loDb.R_AddCommandParameter(loCmd, "@COMPANY_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@USER_ID", System.Data.DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@KEY_GUID", System.Data.DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);
                loDb.R_AddCommandParameter(loCmd, "@Count", System.Data.DbType.Int32, 50, loObject.Count);
                loDb.R_AddCommandParameter(loCmd, "@Finish", System.Data.DbType.Int32, 50, liFinishFlag);
                await loDb.SqlExecQueryAsync(loConn, loCmd, false);

                transScope.Complete();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public record Employee(string Id, string FirstName, string Gender);
    }
}
