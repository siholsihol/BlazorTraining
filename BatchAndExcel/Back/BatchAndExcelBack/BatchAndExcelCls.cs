using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;
using System.Text.Json;
using System.Transactions;

namespace BatchAndExcelBack
{
    public class BatchAndExcelCls : R_IBatchProcessAsync
    {
        public Task R_BatchProcessAsync(R_BatchProcessPar poBatchProcessPar)
        {
            var loEx = new R_Exception();

            try
            {
                var loDb = new R_Db();

                if (loDb.R_TestConnection() == false)
                {
                    loEx.Add("01", "Database Connection Failed");
                    goto END;
                }

                R_BatchProcessTasksync(poBatchProcessPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

        END:
            loEx.ThrowExceptionIfErrors();

            return Task.CompletedTask;
        }

        private async Task R_BatchProcessTasksync(R_BatchProcessPar poBatchProcessPar)
        {
            var loEx = new R_Exception();
            var llIsError = false;
            int[] liErrorEmployeeIds = Array.Empty<int>();
            var loDb = new R_Db();

            try
            {
                var liFinishFlag = 0; //0=Process, 1=Success, 9=Fail
                var loObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<EmployeeDTO>>(poBatchProcessPar.BigObject);
                var loUserParameters = poBatchProcessPar.UserParameters;
                var loIsError = loUserParameters.Where(x => x.Key == "IsError").FirstOrDefault();
                if (loIsError is not null)
                    llIsError = ((JsonElement)loIsError.Value).GetBoolean();

                if (llIsError)
                {
                    var loErrorEmployeeIds = loUserParameters.Where(x => x.Key == "ErrorEmployeeId").FirstOrDefault();
                    if (loErrorEmployeeIds is not null)
                        liErrorEmployeeIds = ((JsonElement)loErrorEmployeeIds.Value).EnumerateArray().Select(x => x.GetInt32()).ToArray();
                }

                using var loTransScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

                using DbConnection loConn = await loDb.GetConnectionAsync();
                using DbCommand loCmd = loDb.GetCommand();

                var loWriteUploadStatus = new WriteUploadStatusDTO
                {
                    CCOMPANY_ID = poBatchProcessPar.Key.COMPANY_ID,
                    CUSER_ID = poBatchProcessPar.Key.USER_ID,
                    CKEY_GUID = poBatchProcessPar.Key.KEY_GUID,
                    ICOUNT = 0,
                    CACTION = "Process Start",
                    ISTATUS = liFinishFlag
                };

                await WriteUploadStatusAsync(loConn, loWriteUploadStatus);

                //TODO Save to Database
                for (int i = 0; i < loObject.Count - 1; i++)
                {
                    loWriteUploadStatus.ChangeStatus(i, $"Process {loObject[i].FirstName}", liFinishFlag);
                    await WriteUploadStatusAsync(loConn, loWriteUploadStatus);

                    //simulate insert to database
                    await Task.Delay(100);

                    if (llIsError && liErrorEmployeeIds is not null && liErrorEmployeeIds.Any(x => x == i))
                    {
                        //simulate error with validation
                        var loErrorStatus = new WriteErrorStatusDTO
                        {
                            CCOMPANY_ID = poBatchProcessPar.Key.COMPANY_ID,
                            CUSER_ID = poBatchProcessPar.Key.USER_ID,
                            CKEY_GUID = poBatchProcessPar.Key.KEY_GUID,
                            ISEQ_NO = i,
                            CERROR_MESSAGE = $"EmployeeId {i} is already exist."
                        };

                        await WriteErrorStatusAsync(loConn, loErrorStatus);
                    }
                }

                //simulate error with unhandled exception
                //throw new Exception("error nya disengaja");

                if (llIsError && liErrorEmployeeIds is not null)
                {
                    //simulate error with validation
                    liFinishFlag = 9; //0=Process, 1=Success, 9=Fail
                    loWriteUploadStatus.ChangeStatus(loObject.Count, $"Error Validation", liFinishFlag);
                    await WriteUploadStatusAsync(loConn, loWriteUploadStatus);

                    loTransScope.Complete();

                    return;
                }

                liFinishFlag = 1; //0=Process, 1=Success, 9=Fail
                loWriteUploadStatus.ChangeStatus(loObject.Count, $"Process Finish", liFinishFlag);
                await WriteUploadStatusAsync(loConn, loWriteUploadStatus);

                loTransScope.Complete();
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

        private async Task WriteUploadStatusAsync(DbConnection poDbConnection, WriteUploadStatusDTO poParameter)
        {
            var loEx = new R_Exception();
            var loDb = new R_Db();

            try
            {
                using DbCommand loCmd = loDb.GetCommand();

                var lcQuery = "EXEC RSP_WriteUploadProcessStatus @CCOMPANY_ID, @CUSER_ID, @CKEY_GUID, @ICOUNT, @CACTION, @ISTATUS";
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CKEY_GUID", DbType.String, 50, poParameter.CKEY_GUID);
                loDb.R_AddCommandParameter(loCmd, "@ICOUNT", DbType.Int32, 50, poParameter.ICOUNT);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 50, poParameter.CACTION);
                loDb.R_AddCommandParameter(loCmd, "@ISTATUS", DbType.Int32, 50, poParameter.ISTATUS);

                await loDb.SqlExecNonQueryAsync(poDbConnection, loCmd, false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task WriteErrorStatusAsync(DbConnection poDbConnection, WriteErrorStatusDTO poParameter)
        {
            var loEx = new R_Exception();
            var loDb = new R_Db();

            try
            {
                using DbCommand loCmd = loDb.GetCommand();

                var lcQuery = "INSERT INTO GST_UPLOAD_ERROR_STATUS (CCOMPANY_ID, CUSER_ID, CKEY_GUID, ISEQ_NO, CERROR_MESSAGE) ";
                lcQuery += "VALUES (@CCOMPANY_ID, @CUSER_ID, @CKEY_GUID, @ISEQ_NO, @CERROR_MESSAGE)";
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CKEY_GUID", DbType.String, 50, poParameter.CKEY_GUID);
                loDb.R_AddCommandParameter(loCmd, "@ISEQ_NO", DbType.Int32, 50, poParameter.ISEQ_NO);
                loDb.R_AddCommandParameter(loCmd, "@CERROR_MESSAGE", DbType.String, 50, poParameter.CERROR_MESSAGE);

                await loDb.SqlExecNonQueryAsync(poDbConnection, loCmd, false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}