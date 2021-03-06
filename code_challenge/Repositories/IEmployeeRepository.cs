using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        ReportingStructure GetReportingStructureById(String id);
        Compensation UpdateCompensation(Employee employee, Compensation compensation);
        Employee GetCompensationById(String id);
        Task SaveAsync();
    }
}