using SAP.Domain.Entities.Base;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAP.Domain.Entities
{
    public class Transaction : AuditedEntity
    {
        public string Id { get; set; }
        public TransactionCategory Category { get; set; }
        [Required]
        public string TypeId { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        public string ProjectId { get; set; }
        public double Amount { get; set; }
        public bool Reconciled { get; set; }
        public string ReconciledById { get; set; }
        public DateTime? ReconciledDateUtc { get; set; }

        public virtual Project Project { get; set; }        
        public virtual Lookup Type { get; set; }
        public virtual User ReconciledBy { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User LastUpdatedBy { get; set; }
    }
}
