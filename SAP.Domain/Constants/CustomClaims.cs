using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Constants
{
    public static class CustomClaims
    {
        public const string LookupsManage = "LOOKUPS_MANAGE";
        public const string ProjectsAllAccess = "PROJECTS_ALL_ACCESS";
        public const string TransactionEntry = "TRANSACTION_ENTRY";
        public const string FinancialReports = "FINANCIAL_REPORTS";
    }

    public static class CustomsClaimTypes
    {
        public const string SapPermission = "sap/permission";
    }
}
