using BatchAndExcelCommon.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace BatchAndExcelBack
{
    public class BatchAndExcelCls : R_IBatchProcess
    {
        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            var loEx = new R_Exception();

            try
            {
                var liFinishFlag = 1; //0=Process, 1=Success, 9=Fail
                var loObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<EmployeeDTO>>(poBatchProcessPar.BigObject);

                var loDb = new R_Db();
                var loConn = loDb.GetConnection();

                var lcQuery = $"EXEC RSP_WriteUploadProcessStatus '{poBatchProcessPar.Key.COMPANY_ID}', " +
                    $"'{poBatchProcessPar.Key.USER_ID}', " +
                    $"'{poBatchProcessPar.Key.KEY_GUID}', " +
                    $"{0}, 'Process Start', 0";
                loDb.SqlExecNonQuery(lcQuery, loConn, false);

                //TODO Save to Database
                for (int i = 0; i < loObject.Count - 1; i++)
                {
                    lcQuery = $"EXEC RSP_WriteUploadProcessStatus '{poBatchProcessPar.Key.COMPANY_ID}', " +
                    $"'{poBatchProcessPar.Key.USER_ID}', " +
                    $"'{poBatchProcessPar.Key.KEY_GUID}', " +
                    $"{i}, 'Process {loObject[i].FirstName}', {0}";
                    loDb.SqlExecNonQuery(lcQuery, loConn, false);

                    var t = Task.Run(async () =>
                    {
                        await Task.Delay(100);
                    });

                    t.Wait();
                }

                lcQuery = $"EXEC RSP_WriteUploadProcessStatus '{poBatchProcessPar.Key.COMPANY_ID}', " +
                    $"'{poBatchProcessPar.Key.USER_ID}', " +
                    $"'{poBatchProcessPar.Key.KEY_GUID}', " +
                    $"{loObject.Count}, 'Process Finish', {liFinishFlag}";

                loDb.SqlExecNonQuery(lcQuery, loConn, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}