using challenge.Models;
using challenge.Repositories;
using System;
using System.Threading.Tasks;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public CompensationService(ICompensationRepository compensationRepository, IEmployeeRepository employeeRepository)
        {
            _compensationRepository = compensationRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Compensation> CreateOrUpdate(Compensation compensation)
        {
            // If Employee doesn't exist, add it. 
            if (string.IsNullOrEmpty(compensation.Employee.EmployeeId)
                || _employeeRepository.GetById(compensation.Employee.EmployeeId.ToString()) == null)
            {
                _employeeRepository.Add(compensation.Employee);
                await _employeeRepository.SaveAsync();
            }

            var existingCompensation = GetCompensation(compensation.Employee.EmployeeId);
            if (existingCompensation != null)
            {
                // If a Compensation object already exists with this employee, just update it.
                existingCompensation.Salary = compensation.Salary;
                existingCompensation.EffectiveDate = DateTime.Now;
                _compensationRepository.Update(existingCompensation);
            }
            else
            {
                // Otherwise, create a new one.
                compensation.CompensationId = Guid.NewGuid().ToString();
                compensation.EffectiveDate = DateTime.Now;
                _compensationRepository.Add(compensation);                
            }

            await _compensationRepository.SaveAsync();
            return compensation;
            
        }

        public Compensation GetCompensation(string employeeId)
        {
            return string.IsNullOrEmpty(employeeId) 
                ? null
                : _compensationRepository.GetByEmployeeId(employeeId);
        }
    }
}
