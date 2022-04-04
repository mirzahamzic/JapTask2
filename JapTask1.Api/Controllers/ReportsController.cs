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
        public async Task<IActionResult> FirstSp()
        {
            return Ok(await _reportService.FirstSp());
        }

        // GET: api/<ReportsController>
        [HttpGet("secondStoredProcedure")]
        public async Task<IActionResult> SecondSp()
        {
            return Ok(await _reportService.SecondSp());
        }

        // POST: api/<ReportsController>
        [HttpGet("thirdStoredProcedure")]
        public async Task<IActionResult> ThirdSp([FromQuery] double minIngredientQuantity, double maxIngredientQuantity, int unitOfMeasure)
        {
            return Ok(await _reportService.ThirdSp(minIngredientQuantity, maxIngredientQuantity, unitOfMeasure));
        }

    }
}
