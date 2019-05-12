using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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

        [TestMethod]
        public void GetReportingStructureById_Test_JohnLennon()
        {
            //Test John Lennon with multi-DirectReports
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            //Assert
            var reportStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReports, reportStructure.NumberOfReports);
        }

        [TestMethod]
        public void GetReportingStructureById_Test_PaulMcCartney()
        {
            // Test Paul McCartney with no DirectReport
            var employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";
            var expectedReports = 0;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            //Assert
            var reportStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedReports, reportStructure.NumberOfReports);
        }

        [TestMethod]
        public void GetReportingStructureById_Return_ReportStructures_JohnLennon()
        {
            //Test John Lennon with multi-DirectReports

            Employee John = new Employee {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                FirstName = "John",
                LastName = "Lennon",
                Position = "Development Manager",
                Department = "Engineering",
                DirectReports = new List<Employee>()
            };
            Employee Paul = new Employee
            {
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",
                FirstName = "Paul",
                LastName = "McCartney",
                Position = "Developer I",
                Department = "Engineering",
                DirectReports = new List<Employee>()
            };
            Employee Ringo = new Employee
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                FirstName = "Ringo",
                LastName = "Starr",
                Position = "Developer V",
                Department = "Engineering",
                DirectReports = new List<Employee>()
            };
            Employee Pete = new Employee
            {
                EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer II",
                Department = "Engineering",
                DirectReports = new List<Employee>()
            };
            Employee George = new Employee
            {
                EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                FirstName = "George",
                LastName = "Harrison",
                Position = "Developer III",
                Department = "Engineering",
                DirectReports = new List<Employee>()
            };
            Ringo.DirectReports.Add(Pete);
            Ringo.DirectReports.Add(George);

            John.DirectReports.Add(Paul);
            John.DirectReports.Add(Ringo);           

            ReportingStructure expectedReportingStructure_John = new ReportingStructure()
            {
                Employee = John,
                NumberOfReports = 4
            };
            
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{John.EmployeeId}");
            var response = getRequestTask.Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //Assert
            var actualReportStructure = response.DeserializeContent<ReportingStructure>();
            Assert.IsTrue(CheckDirectReports(expectedReportingStructure_John.Employee, actualReportStructure.Employee));
            Assert.AreEqual(expectedReportingStructure_John.NumberOfReports, actualReportStructure.NumberOfReports);
        }

        private bool CheckDirectReports(Employee expected, Employee actual)
        {
            //check Current Employee node
            bool result = AssertEmployeeProperties(expected, actual);

            //check Direct Report Employee nodes
            if (actual.DirectReports.Count == 0)
            {
               return true;
            }
            else {
                for (int i = 0; i < expected.DirectReports.Count && result; i++)
                {
                   result = CheckDirectReports(expected.DirectReports[i], actual.DirectReports[i]);
                    if (!result) return false; 
                }
            }
            return result;

        }
        private bool AssertEmployeeProperties(Employee expected, Employee actual)
        {
            return (expected.EmployeeId == actual.EmployeeId) &&
                      (expected.FirstName == actual.FirstName) &&
                      (expected.LastName == actual.LastName) &&
                      (expected.Department == actual.Department) &&
                      (expected.Position == actual.Position);
        }
    }
}
