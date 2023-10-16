using BlazorTrainingApi.Log;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTrainingApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ApiLog _logger;

        public LogController(ILogger<LogController> logger)
        {
            ApiLog.R_InitializeLogger(logger);
            _logger = ApiLog.R_GetInstanceLogger();
        }

        [HttpPost()]
        public void GetLog()
        {
            _logger.LogInfo("test Log");
        }
    }
}
