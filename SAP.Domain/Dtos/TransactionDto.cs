using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class TransactionDto
    {
        public string Id { get; set; }
        public TransactionCategory Category { get; set; }
        public string TypeId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public double Amount { get; set; }
        public bool Reconciled { get; set; }
        public string ReconciledById { get; set; }
        public string ReconciledBy { get; set; }
    }
}
