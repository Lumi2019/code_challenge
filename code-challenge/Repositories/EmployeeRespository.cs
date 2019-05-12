using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public Compensation GetCompensationByEmployeeId(string id)
        {
           return _employeeContext.Compensations.Where(x=> x.EmployeeId == id).FirstOrDefault();            
        }
        public ReportingStructure GetReportingStructureByEmplopyeeId(string id)
        {
            ReportingStructure reportingStructure = new ReportingStructure();
            reportingStructure.Employee = GetEmployeeById(id);
            reportingStructure.NumberOfReports = FindTotalReports(id, 0);
            return reportingStructure;
        }
        private int FindTotalReports(string employeeId, int c)
        {
           int counter = c;
            Employee employee = GetEmployeeById(employeeId);

            if (employee.DirectReports.Count() == 0)
                return counter;
            else 
            {
                foreach (var item in employee.DirectReports)
                {
                    counter = FindTotalReports(item.EmployeeId, counter) + 1;
                }
            }
            return counter;
        }

        private Employee GetEmployeeById(string id)
        {
            Employee employee = _employeeContext.Employees.Find(id);
            if (employee.DirectReports == null)
            {
                var dr = _employeeContext.Employees.Where(x => x.EmployeeId == id).Select(x => x.DirectReports).FirstOrDefault();
                employee.DirectReports = dr;
            }
            return employee;
        }

        public Compensation AddCompensation(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

    }
}
