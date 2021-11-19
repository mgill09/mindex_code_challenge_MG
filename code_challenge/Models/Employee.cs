using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Employee
    {
        [Key]
        public String EmployeeId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Position { get; set; }
        public String Department { get; set; }
        public virtual List<Employee> DirectReports { get; set; }
        public Compensation Compensation { get; set; }


        /*
            This recursive method gets the total number of reports under a given employee by traversing the
            employee n-ary tree and recursively gathering the number of reports
         */
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
