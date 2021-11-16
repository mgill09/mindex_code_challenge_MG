using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure(Employee employee, int numberofreports)
        {
            Employee = employee;
            NumberOfReports = numberofreports;
        }
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    }
}
