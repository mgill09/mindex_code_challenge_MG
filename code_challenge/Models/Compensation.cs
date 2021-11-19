using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    [Owned] //Indicates this is an Owned Entity, owned by Employee
    public class Compensation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Generates a new ID for each Compensation
        [Key]
        public String Id { get; set; }
        public int Salary { get; set; }
        public string EffectiveDate { get; set; }
    }
}
