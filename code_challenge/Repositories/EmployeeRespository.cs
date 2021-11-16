using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public ReportingStructure GetReportingStructureById(string id)
        {
            var given_employee = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
            var total_reports_count = given_employee.GetTotalReportsTo();

            return new ReportingStructure(given_employee, total_reports_count);
        }

        public String UpdateCompensation(Employee employee, Compensation compensation)
        {
            if (compensation.Salary != 0 && compensation.EffectiveDate != null)
            {
                employee.Compensation = compensation;
                _employeeContext.Update(employee);
                return "Compensation updated successfully.";

            }
            else
            {
                return "Incorrect compensation provided.";
            }
        }

        public Object GetCompensationById(string id)
        {
            var given_employee = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
            return new { employee = given_employee, compensation = given_employee.Compensation };
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
