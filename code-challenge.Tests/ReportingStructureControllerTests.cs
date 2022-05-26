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

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
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
     
        [DataTestMethod]
        [DataRow("b7839309-3348-463b-a7e3-5de1c168beb3", "Paul", 0)]
        [DataRow("62c1084e-6e34-4630-93fd-9153afb65309", "Pete", 0)]
        [DataRow("c0c2293d-16bd-4603-8e08-638a9d18b22c", "George", 0)]
        [DataRow("03aa1462-ffa9-4978-901b-7c001562cf6f", "Ringo", 2)]
        [DataRow("16a596ae-edd3-4847-99fe-c4518e82c86f", "John", 4)]
        public void Test_GetReportingStructure(string employeeId, string firstName, int expectedDirectReports)
        {                        
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingStructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.IsNotNull(reportingStructure);
            Assert.IsNotNull(reportingStructure.Employee);
            Assert.AreEqual(reportingStructure.Employee.EmployeeId.ToString(), employeeId);
            Assert.AreEqual(reportingStructure.Employee.FirstName, firstName);
            Assert.AreEqual(reportingStructure.NumberOfReports, expectedDirectReports);
        }       
    }
}
