using challenge.Models;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetByEmployeeId(string employeeId);
        Compensation Add(Compensation compensation);
        Compensation Update(Compensation compensation);
        Task SaveAsync();
    }
}
