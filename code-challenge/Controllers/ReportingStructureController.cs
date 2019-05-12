using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;
using challenge.Repositories;

namespace challenge.Controllers
{
    [Route("api/reportingstructure")]
    public class ReportingStructureController: Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{id}", Name = "getReportingStructureById")]
        public IActionResult GetReportingStructureById(String id)
        {
            _logger.LogDebug($"Received reporting structure get request for '{id}'");

            var reportingStructure = _employeeRepository.GetReportingStructureByEmplopyeeId(id);

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }

    }
}
