using challenge.Models;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Task<Compensation> CreateOrUpdate(Compensation compensation);
        Compensation GetCompensation(string employeeId);
    }
}
