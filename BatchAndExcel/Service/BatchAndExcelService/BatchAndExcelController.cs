using BatchAndExcelCommon.DTOs;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using System.Reflection;

namespace BatchAndExcelService
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BatchAndExcelController : ControllerBase
    {
        [HttpPost]
        public DownloadFileDTO DownloadFile()
        {
            var loEx = new R_Exception();
            var loRtn = new DownloadFileDTO();

            try
            {
                var loAsm = Assembly.GetExecutingAssembly();
                var lcResourceFile = "BatchAndExcelService.File.EditorContent.docx";
                using (Stream resFilestream = loAsm.GetManifestResourceStream(lcResourceFile))
                {
                    var ms = new MemoryStream();
                    resFilestream.CopyTo(ms);
                    var bytes = ms.ToArray();

                    loRtn.FileBytes = bytes;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}