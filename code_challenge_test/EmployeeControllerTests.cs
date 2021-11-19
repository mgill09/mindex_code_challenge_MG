using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;
using System.Threading.Tasks;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newEmployee = response.DeserializeContent<Employee>();
            Assert.IsNotNull(newEmployee.EmployeeId);
            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
            Assert.AreEqual(employee.Department, newEmployee.Department);
            Assert.AreEqual(employee.Position, newEmployee.Position);
        }

        [TestMethod]
        public void GetEmployeeById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var employee = response.DeserializeContent<Employee>();
            Assert.AreEqual(expectedFirstName, employee.FirstName);
            Assert.AreEqual(expectedLastName, employee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Department = "Engineering",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer VI",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
            var newEmployee = putResponse.DeserializeContent<Employee>();

            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_NotFound()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "Invalid_Id",
                Department = "Music",
                FirstName = "Sunny",
                LastName = "Bono",
                Position = "Singer/Song Writer",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        /*
         New test case to ensure that the response contains the proper employee info
         as well as the proper ReportsTo value
         */
        [TestMethod]
        public void GetReportingStructureByID_Returns_OK()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedNumberOfReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/reporting/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedFirstName, reportingStructure.Employee.FirstName);
            Assert.AreEqual(expectedLastName, reportingStructure.Employee.LastName);
            Assert.AreEqual(expectedNumberOfReports, reportingStructure.NumberOfReports);
        }

        /*
         New test case to ensure that the response contains the proper employee info
         as well as the proper compensation values
         */
        [TestMethod]
        public void UpdateCompensation_Returns_OK()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            var newCompensation = new Compensation()
            {
                Salary = 80000,
                EffectiveDate = "2021/11/18"
            };

            var requestContent = new JsonSerialization().ToJson(newCompensation);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/employee/compensation/{employeeId}", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = putRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
        }


        /*
         This method pushes compensation data for the purposes of the test,
         and then we use the get endpoint to ensure that the data is retrieved properly

            Note: this task MUST be async with awaits on the requests because of timing issues
         */
        [TestMethod]
        public async Task GetCompensationById_Returns_OK()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            var newCompensation = new Compensation()
            {
                Salary = 80000,
                EffectiveDate = "2021/11/18"
            };

            var requestContent = new JsonSerialization().ToJson(newCompensation);

            // Execute updating of Compensation for testing get since values do not persist between runs
            var putRequestTask = await _httpClient.PutAsync($"api/employee/compensation/{employeeId}", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putRequestTask.StatusCode);

            // Execute actual getting of Compensation
            var getRequestTask = await _httpClient.GetAsync($"api/employee/compensation/{employeeId}");
            Employee employee = getRequestTask.DeserializeContent<Employee>();
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, getRequestTask.StatusCode);
            Assert.AreEqual(employee.Compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(employee.Compensation.EffectiveDate, newCompensation.EffectiveDate);
        }
    }
}
