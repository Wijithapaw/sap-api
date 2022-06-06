using SAP.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAP.Domain.Entities
{
    public class WorkLog : AuditedEntity
    {
        public string Id { get; set; }
        [Required]
        public string ProjectId { get; set; }
        [Required, MaxLength(50)]
        public string LabourName { get; set; }
        [Required, MaxLength(100)]
        public string JobDescription { get; set; }
        public DateTime Date { get; set; }
        public double? Wage { get; set; }

        public virtual Project Project { get; set; }
    }
}
