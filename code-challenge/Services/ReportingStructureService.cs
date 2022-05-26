using challenge.Models;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public ReportingStructureService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public ReportingStructure GetReportingStructure(string employeeId)
        {
            var employee = _employeeRepository.GetById(employeeId);
            return new ReportingStructure()
            {
                Employee = employee,
                NumberOfReports = GetNumberOfReports(employeeId)
            };
        }

        /// <summary>
        /// Calculates the number of reports under a given employee. 
        /// <br/>The number of reports is determined to be the number of DirectReports for an employee and all of their DirectReports.
        /// </summary>        
        private int GetNumberOfReports(string employeeId)
        {
            var employee = _employeeRepository.GetById(employeeId);
            var numberOfReports = 0;
            if (employee.DirectReports.Count > 0)
            {
                foreach(var report in employee.DirectReports)
                {
                    numberOfReports++;
                    numberOfReports += GetNumberOfReports(report.EmployeeId);
                }
            }
            return numberOfReports;
        }
    }
}
