using SAP.Domain.Entities;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Tests.Helpers
{
    internal static class TestData
    {
        internal static class Projects
        {
            public static Project[] GetProjects()
            {
                var projects = new Project[]
                {
                    EntityHelper.CreateProject("p-1", "Wire Fence", "Infrastructure - Wire Fence", new DateTime(2020, 4, 1), new DateTime(2020, 6, 1), ProjectState.Completed),
                    EntityHelper.CreateProject("p-2", "Bridge", "Infrastructure - Bridge", new DateTime(2020, 9, 1), new DateTime(2020, 9, 10), ProjectState.Completed),
                    EntityHelper.CreateProject("p-3", "New Coconut Crop", null, new DateTime(2021, 3, 1), null, ProjectState.Inprogress),
                };

                return projects;
            }
        }

        internal static class Lookups
        {
            public static LookupHeader[] GetLookupHeaders()
            {
                var headers = new LookupHeader[]
                {
                    EntityHelper.CreateLookupHeader("h-1", "EXPENSE_TYPES", "Expense Types"),
                    EntityHelper.CreateLookupHeader("h-2", "INCOME_TYPES", "Income Types"),
                    EntityHelper.CreateLookupHeader("h-3", "SALUTATIONS", "Salutations"),
                };

                return headers;
            }

            public static Lookup[] GetLookups()
            {
                var lookups = new Lookup[]
                {
                    EntityHelper.CreateLookup("lk-11", "h-1", "LABOUR", "Labour"),
                    EntityHelper.CreateLookup("lk-12", "h-1", "MATERIAL", "Material"),
                    EntityHelper.CreateLookup("lk-13", "h-1", "BILLS", "Bills"),
                    EntityHelper.CreateLookup("lk-14", "h-1", "BASS_FEES", "Bass Fees", true),

                    EntityHelper.CreateLookup("lk-21", "h-2", "COCONUT", "Coconut"),
                    EntityHelper.CreateLookup("lk-22", "h-2", "PAPOW", "Papow"),
                    EntityHelper.CreateLookup("lk-23", "h-2", "VEGITABLE", "Vegitable"),
                };

                return lookups;
            }
        }
    }
}
