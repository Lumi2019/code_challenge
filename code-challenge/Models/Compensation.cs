using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {

        /// <summary>
        ///         Create a new type, Compensation.A Compensation has the following fields: employee, salary, and effectiveDate.
        /// </summary>

       [Key]
        public string CompensationId { get; set; }
        public double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EmployeeId { get; set; }
    }
}
