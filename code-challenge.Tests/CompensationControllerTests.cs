using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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
        public void Test_AddCompensation_ExistingEmployee()
        {
            // Arrange
            var compensation = new Compensation()
            {
                Employee = new Employee()
                {
                    EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                    FirstName = "Paul",
                    LastName = "McCartney",
                    Position = "Developer I",
                    Department = "Engineering",
                },
                Salary = 123456.78M
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation);
            Assert.IsNotNull(newCompensation.Employee);
            Assert.AreEqual(newCompensation.Employee.EmployeeId, compensation.Employee.EmployeeId);
            Assert.AreEqual(newCompensation.Employee.FirstName, compensation.Employee.FirstName);
            Assert.AreEqual(newCompensation.Employee.LastName, compensation.Employee.LastName);
            Assert.AreEqual(newCompensation.Employee.Position, compensation.Employee.Position);
            Assert.AreEqual(newCompensation.Employee.Department, compensation.Employee.Department);
            Assert.AreEqual(newCompensation.Salary, compensation.Salary);
            Assert.IsNotNull(newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void Test_AddCompensation_NewEmployee()
        {
            // Arrange
            var compensation = new Compensation()
            {
                Employee = new Employee()
                {                   
                    FirstName = "Bart",
                    LastName = "Simpson",
                    Position = "Developer I",
                    Department = "Engineering",
                },
                Salary = 11111.22M
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation);
            Assert.IsNotNull(newCompensation.Employee);
            Assert.AreEqual(newCompensation.Employee.FirstName, compensation.Employee.FirstName);
            Assert.AreEqual(newCompensation.Employee.LastName, compensation.Employee.LastName);
            Assert.AreEqual(newCompensation.Employee.Position, compensation.Employee.Position);
            Assert.AreEqual(newCompensation.Employee.Department, compensation.Employee.Department);
            Assert.AreEqual(newCompensation.Salary, compensation.Salary);
            Assert.IsNotNull(newCompensation.EffectiveDate);
        }

        [DataTestMethod]
        [DataRow("16a596ae-edd3-4847-99fe-c4518e82c86f")]
        [DataRow("03aa1462-ffa9-4978-901b-7c001562cf6f")]
        [DataRow("c0c2293d-16bd-4603-8e08-638a9d18b22c")]
        public void Test_GetCompensationByEmployeeId_RecordNotFound(string employeeId)
        {            
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void Test_GetCompensationByEmployeeId()
        {
            // Arrange
            var expectedCompensation = new Compensation()
            {
                Employee = new Employee()
                {
                    EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                    FirstName = "Paul",
                    LastName = "McCartney",
                    Position = "Developer I",
                    Department = "Engineering",
                },
                Salary = 123456.78M
            };

            // Execute
            var requestContent = new JsonSerialization().ToJson(expectedCompensation);            
            _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json")).Wait();
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{expectedCompensation.Employee.EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var compensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(compensation);
            Assert.IsNotNull(compensation.Employee);
            Assert.AreEqual(compensation.Salary, Convert.ToDecimal(expectedCompensation.Salary));
        }

    }
}
