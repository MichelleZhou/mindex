using challenge.Models;

namespace challenge.Services
{
    public interface IReportingStructureService
    {
        ReportingStructure GetReportingStructure(string employeeId);
    }
}
