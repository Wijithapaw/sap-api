using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class TransactionSearchDto
    {
        public string Projects { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SearchTerm { get; set; }
        public TransactionCategory? Category { get; set; }
        public string[] CategotyTypes { get; set; }
        public bool? Reconsiled { get; set; }
    }
}
