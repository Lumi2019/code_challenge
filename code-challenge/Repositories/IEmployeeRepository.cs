using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Compensation GetCompensationByEmployeeId(string id);
        Compensation AddCompensation(Compensation compensation);
        ReportingStructure GetReportingStructureByEmplopyeeId(string id);
        Task SaveAsync();
    }
}