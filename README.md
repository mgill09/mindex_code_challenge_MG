# mindex_code_challenge_MG
mindex_code_challenge

Some disclaimers/notes:

1) I have never used the .NET framework, let alone for RESTful services, so I hope my design choices aren't too off the mark - I was learning as I went and I think I accomplished the tasks.
2) Due to some versioning issues, I rebuilt this entire project bringing over only the code and making many tweaks in that existing code so it would run with the most recent .NET and .NET Core frameworks - I built this using VS 2022 so there may be some compatability issues with older versions of VS/.NET frameworks.
3) During my research I discovered OwnedEntity in Microsofts documentation, which allows for one Entity inside of the already existing Employee Entity.  I used this to create the Compensation Entity inside the Employee Entity, rather than creating an entirely separate one - I thought it made the most sense design-wise because there should always be a one-to-one relationship between an Employee and the Compensation type and the Compensation type would not exist without first having an Employee type - although I could see uses for separating it as well.
4) There is an existing bug in the code exercise that caused me a lot of lost time (and gained grief for sure :) ) - the following line is required in the EmployeeContext.cs to bring over the values inside DirectReports:
  var types = this.Employees.Include(m => m.DirectReports).Include(m=>m.Compensation).ToList();
5) Due to the framework upgrade I also needed to add a line to the TestStartup.cs:
            .AddApplicationPart(typeof(Startup).Assembly);

6) The new endpoints are as follows:
    -/api/employee/reporting/{id} [GET]
    -/api/employee/compensation/{id} [PUT OR GET]
    
   The first end point will pull the employee type, along with the number of reports which is calculated dynamically using a recursive function and not stored in memory per instructions - the object returned is built on the fly as a ReportStructure class object.
   
   The second end point will push a new Compensation type into the Compensation entity, which is inside the Employee entity.  An example of the body would be:
   
   {
    "salary": 80,
    "effectiveDate": "Sept 10 2012"
    }
   
   The put will return a reponse of the Compensation object, while the get will return the Employee object that contains the Compensation object.  The results are organized slightly different than what the exercise calls for due to the Owned Entity, but the data is all there in roughly the same format that was asked (ideally the effectiveDate would have checks to ensure it is a date format but I ran out of time).  There is also an arugment to be made that the endpoints could potentially be outside of the /employee endpoint, but I thought it was fitting to have them as extensions since all of this data is directly related to the employee.
   
 7) If I had more time I would've liked to improve the error handling on each new endpoint, as well as more thorough commenting of code - I did have some time to add three new test cases for the new functionality so I went back and added them.  I will try to revisit to do these things this week if I am able.
 
 8) At the very least, this was a fantastic learning experience for me!
   
