using challenge.Data;
using challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;

        public CompensationRepository(CompensationContext compensationContext)
        {
            _compensationContext = compensationContext ?? throw new ArgumentNullException(nameof(compensationContext));
        }

        public Compensation Add(Compensation compensation)
        {            
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation Update(Compensation compensation)
        {
            _compensationContext.Compensations.Update(compensation);
            return compensation;
        }
       
        public Compensation GetByEmployeeId(string employeeId)
        {           
            return _compensationContext.Compensations
                .Include(x => x.Employee)
                .Where(x => x.Employee.EmployeeId.ToString() == employeeId)
                .FirstOrDefault();
        }

        public async Task SaveAsync()
        {
            await _compensationContext.SaveChangesAsync();
        }
    }
}
