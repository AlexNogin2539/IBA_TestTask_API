using IBA_TestTask;
using IBA_TestTask_API.Managers;
using Microsoft.AspNetCore.Mvc;

namespace IBA_TestTask_API.Controllers
{
    [ApiController]
    [Route("speed")]
    public class SpeedController : ControllerBase
    {
        private readonly ISourceDataManager _sourceDataManager;

        public SpeedController(ISourceDataManager sourceDataManager)
        {
            _sourceDataManager = sourceDataManager;
        }

        [HttpGet ("getAll")]
        public IActionResult GetAll()
        {
            var result = _sourceDataManager.GetAllData();
            return Ok(result);
        }

        [HttpGet("getByDateAndSpeed")]
        public IActionResult GetByDateAndSpeed(DateTime date, double speed)
        {
            var result = _sourceDataManager.GetOutspeedData(date, speed);
            return Ok(result);
        }

        [HttpGet("getMinMaxSpeeds")]
        public IActionResult GetMinMaxValueByDate(DateTime date)
        {
            var minValue = _sourceDataManager.GetMinSpeedData(date);
            var maxValue = _sourceDataManager.GetMaxSpeedData(date);
            var result = new
            {
                minValue,
                maxValue
            };
            return Ok(result);
        }


        [HttpPost("data/range")]
        public IActionResult AddRange(IList<ControlSystemData> data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _sourceDataManager.AddDataRange(data);
            return Ok();
        }
    }
}