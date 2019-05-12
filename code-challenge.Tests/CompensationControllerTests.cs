using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void CreateCompensation_Returns_JohnLennon()
        {
             var compensation = new Compensation()
            {
                EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",  
                Salary = 1000000,
                EffectiveDate = DateTime.Now
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();

            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            
        }

        [TestMethod]
        public void GetCompensation_Returns_Created_PaulMcCartney()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",  
                Salary = 1000000,
                EffectiveDate = DateTime.Now
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            //create compensation
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;
            var newCompensation = response.DeserializeContent<Compensation>();
           
            //Read by employeeId
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{newCompensation.EmployeeId}");
            var getResponse = getRequestTask.Result;
            var newCreatedCompensation = getResponse.DeserializeContent<Compensation>();
        
            // Assert
            Assert.IsNotNull(newCreatedCompensation.CompensationId);
            Assert.AreEqual(compensation.EmployeeId, newCreatedCompensation.EmployeeId);
            Assert.AreEqual(compensation.Salary, newCreatedCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCreatedCompensation.EffectiveDate);
        }
    }
}
