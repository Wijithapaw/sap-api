using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class TransactionSummaryDto
    {
        public double Expenses { get; set; }
        public double Income { get; set; }
        public double ShareDividend { get; set; }
        public double Profit { get; set; }
    }
}
