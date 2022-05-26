using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _compensationService = compensationService ?? throw new ArgumentNullException(nameof(compensationService));
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received Compensation CREATE request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            _compensationService.CreateOrUpdate(compensation);

            return CreatedAtRoute("GetCompensationByEmployeeId", new { employeeId = compensation.Employee.EmployeeId }, compensation);
        }

        [HttpGet("{employeeId}", Name = "GetCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Received Compensation GET request for '{employeeId}'");

            var compensation = _compensationService.GetCompensation(employeeId);

            if (compensation == null) return NotFound();

            return Ok(compensation);
        }       
    }
}