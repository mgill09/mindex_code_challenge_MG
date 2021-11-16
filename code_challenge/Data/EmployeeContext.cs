using challenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
            var types = this.Employees.Include(m => m.DirectReports).Include(m=>m.Compensation).ToList();
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Employee>().OwnsOne(e => e.Compensation, comp =>
            // {
            //     comp.Property(x => x.Salary);
            //     comp.Property(x => x.EffectiveDate);
            // });

            //modelBuilder.Entity<Employee>()
            //    .Property(e => e.Compensation.Id)
            //.ValueGeneratedOnAdd();
        }
    }
}
