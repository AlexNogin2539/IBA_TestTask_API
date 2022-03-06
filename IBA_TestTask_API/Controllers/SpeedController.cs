using IBA_TestTask;
using IBA_TestTask_API.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IBA_TestTask_API.Controllers
{
    [ApiController]
    [Route("speed")]
    public class SpeedController : ControllerBase
    {
        private readonly ISourceDataManager _sourceDataManager;
        private readonly AppSettings _appSettings;

        public SpeedController(ISourceDataManager sourceDataManager, IOptionsSnapshot<AppSettings> appSettings)
        {
            _sourceDataManager = sourceDataManager;
            _appSettings = appSettings.Value;
        }

        [HttpGet ("getAll")]
        public IActionResult GetAll()
        {
            if (!CheckAvailability())
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var result = _sourceDataManager.GetAllData();
            return Ok(result);
        }

        [HttpGet("getByDateAndSpeed")]
        public IActionResult GetByDateAndSpeed(DateTime date, double speed)
        {
            if (!CheckAvailability())
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var result = _sourceDataManager.GetOutspeedData(date, speed);
            return Ok(result);
        }

        [HttpGet("getMinMaxSpeeds")]
        public IActionResult GetMinMaxValueByDate(DateTime date)
        {
            if (!CheckAvailability())
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }

            var minValue = _sourceDataManager.GetMinSpeedData(date);
            var maxValue = _sourceDataManager.GetMaxSpeedData(date);
            var result = new
            {
                minValue,
                maxValue
            };
            return Ok(result);
        }


        [HttpPost("add/data/range")]
        public IActionResult AddRange(IList<ControlSystemData> data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _sourceDataManager.AddDataRange(data);
            return Ok();
        }

        private bool CheckAvailability()
        {
            var startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, _appSettings.StartTime, 00, 00);
            var finishTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, _appSettings.EndTime, 00, 00);
            return DateTime.Compare(startTime, DateTime.Now) + DateTime.Compare(finishTime, DateTime.Now) == 0;
        }
    }
}