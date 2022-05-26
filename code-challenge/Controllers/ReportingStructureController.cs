using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace challenge.Controllers
{
    [Route("api/reportingStructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportingStructureService = reportingStructureService ?? throw new ArgumentNullException(nameof(reportingStructureService));
        }

        [HttpGet("{employeeId}", Name = "GetReportingStructure")]
        public IActionResult GetReportingStructure(string employeeId)
        {
            _logger.LogDebug($"Received Reporting Structure GET request for '{employeeId}'");
            
            var reportingStructure = _reportingStructureService.GetReportingStructure(employeeId);
            
            if (reportingStructure == null) return NotFound();

            return Ok(reportingStructure);
        }
    }
}
