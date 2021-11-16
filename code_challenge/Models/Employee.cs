using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Employee
    {
        public Employee()
        {
            //Compensation = new Compensation(EmployeeId);
        }
        [Key]
        public String EmployeeId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Position { get; set; }
        public String Department { get; set; }
        public virtual List<Employee> DirectReports { get; set; }
        public Compensation Compensation { get; set; }

        public int GetTotalReportsTo(){
            if (DirectReports.Count == 0)
            {
                return 0;
            }
            else
            {
                int total = DirectReports.Count;
                for (int i = 0; i < DirectReports.Count; i++)
                {
                    total += DirectReports[i].GetTotalReportsTo();
                }
                return total;
            }

        }
    }
}
