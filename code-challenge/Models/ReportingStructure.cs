using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class ReportingStructure
    {
        /// <summary>
        /// Create a new type, ReportingStructure, that has two properties: employee and numberOfReports.
        /// </summary>
        [Key]
        public int ReportingStructureId { get; set; }
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    }
}
