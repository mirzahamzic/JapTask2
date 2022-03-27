using JapTask1.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JapTask1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/<ReportsController>
        [HttpGet("firstStoredProcedure")]
        public async Task<ActionResult> FirstSp()
        {
            return Ok(await _reportService.FirstSp());
        }

        // GET: api/<ReportsController>
        [HttpGet("secondStoredProcedure")]
        public async Task<ActionResult> SecondSp()
        {
            return Ok(await _reportService.SecondSp());
        }

        // POST: api/<ReportsController>
        [HttpGet("thirdStoredProcedure")]
        public async Task<ActionResult> ThirdSp([FromQuery] double min, double max, int unit)
        {
            return Ok(await _reportService.ThirdSp(min, max, unit));
        }

    }
}
