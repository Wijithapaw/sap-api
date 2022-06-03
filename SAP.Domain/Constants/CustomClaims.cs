﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Constants
{
    public static class CustomClaims
    {
        public const string AdminControl = "ADMIN_CONTROL";
        public const string LookupsFullAccess = "LOOKUPS_FULL_MANAGE";
        public const string ProjectsFullAccess = "PROJECTS_FULL_ACCESS";
        public const string TransactionEntry = "TRANSACTION_ENTRY";
        public const string TransactionReconcile = "TRANSACTION_RECONCILE";
        public const string TransactionDelete = "TRANSACTION_DELETE";
        public const string FinancialReports = "FINANCIAL_REPORTS";
        public const string UsersFullAccess = "USERS_FULL_ACCESS";
    }

    public static class CustomClaimTypes
    {
        public const string SapPermission = "sap/permission";
    }

    public static class IdentityRoles
    {
        public const string Admin = "Admin";
        public const string ProjectManager = "ProjectManager";
        public const string DataAnalyzer = "DataAnalyzer";
    }
}
