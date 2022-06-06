using SAP.Domain.Entities;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Tests.Helpers
{
    internal static class TestData
    {
        internal static class Users
        {
            public static User[] GetUsers()
            {
                var users = new User[]
                {
                    EntityHelper.GetUser("u-1", "Wijitha", "Wijenayake", "wijitha@yopmail.com"),
                    EntityHelper.GetUser("u-2", "Saman", "Perera", "saman@yopmail.com"),
                    EntityHelper.GetUser("u-3", "Arun", "Kumara", "arun@yopmail.com"),
                    EntityHelper.GetUser("unit-test", "Unit", "Test", "unittest@yopmail.com"),
                };

                return users;
            }
        }

        internal static class Projects
        {
            public static Project[] GetProjects()
            {
                var projects = new Project[]
                {
                    EntityHelper.GetProject("p-1", "Wire Fence", "Infrastructure - Wire Fence", new DateTime(2020, 4, 1), new DateTime(2020, 6, 1), ProjectState.Completed, "u-1"),
                    EntityHelper.GetProject("p-2", "Bridge", "Infrastructure - Bridge", new DateTime(2020, 9, 1), new DateTime(2020, 9, 10), ProjectState.Completed, "u-1"),
                    EntityHelper.GetProject("p-3", "New Coconut Crop", null, new DateTime(2021, 3, 1), null, ProjectState.Inprogress),
                    EntityHelper.GetProject("p-4", "Papow", null, new DateTime(2021, 12, 1), null, ProjectState.Inprogress, "u-2"),
                };

                return projects;
            }

            public static ProjectTag[] GetProjectTags()
            {
                var projectTags = new ProjectTag[]
                {
                    new ProjectTag { ProjectId = "p-1", TagId="t-1" },
                    new ProjectTag { ProjectId = "p-2", TagId="t-1" }
                };
                return projectTags;
            }
        }

        internal static class Lookups
        {
            public static LookupHeader[] GetLookupHeaders()
            {
                var headers = new LookupHeader[]
                {
                    EntityHelper.GetLookupHeader("h-1", "EXPENSE_TYPES", "Expense Types"),
                    EntityHelper.GetLookupHeader("h-2", "INCOME_TYPES", "Income Types"),
                    EntityHelper.GetLookupHeader("h-3", "SALUTATIONS", "Salutations"),
                };

                return headers;
            }

            public static Lookup[] GetLookups()
            {
                var lookups = new Lookup[]
                {
                    EntityHelper.GetLookup("lk-11", "h-1", "LABOUR", "Labour"),
                    EntityHelper.GetLookup("lk-12", "h-1", "MATERIAL", "Material"),
                    EntityHelper.GetLookup("lk-13", "h-1", "BILLS", "Bills"),
                    EntityHelper.GetLookup("lk-14", "h-1", "BASS_FEES", "Bass Fees", true),

                    EntityHelper.GetLookup("lk-21", "h-2", "COCONUT", "Coconut", false, true),
                    EntityHelper.GetLookup("lk-22", "h-2", "PAPOW", "Papow"),
                    EntityHelper.GetLookup("lk-23", "h-2", "VEGITABLE", "Vegitable"),
                };

                return lookups;
            }
        }

        internal static class Tags
        {
            public static Tag[] GetTags()
            {
                var tags = new Tag[]
                {
                    EntityHelper.GetTag("t-1", "infrastructure"),
                    EntityHelper.GetTag("t-2", "irrigation"),
                    EntityHelper.GetTag("t-3", "construction"),
                    EntityHelper.GetTag("t-4", "cultivation"),
                };
                return tags;
            }
        }

        internal static class WorkLogs
        {
            public static WorkLog[] GetWorkLogs()
            {
                var logs = new WorkLog[]
                {
                    EntityHelper.GetWorkLog("wl-11", "p-1", new DateTime(2020, 1, 1), "James", "Digging for posts", 2000),
                    EntityHelper.GetWorkLog("wl-12", "p-1", new DateTime(2020, 1, 2), "James", "Digging for posts", 2000),
                    EntityHelper.GetWorkLog("wl-13", "p-1", new DateTime(2020, 1, 1), "Supun", "Digging for posts", 2000),
                    EntityHelper.GetWorkLog("wl-14", "p-1", new DateTime(2020, 1, 1), "Kumara", "Clearing bushes"),

                    EntityHelper.GetWorkLog("wl-21", "p-2", new DateTime(2020, 9, 1), "Gamini", "Bridge construction", 2500),
                    EntityHelper.GetWorkLog("wl-22", "p-2", new DateTime(2020, 9, 2), "Kumara", "Bridge construction", 3000),
                    EntityHelper.GetWorkLog("wl-23", "p-2", new DateTime(2020, 9, 3), "Nihal", "Bridge construction", 1900),
                    EntityHelper.GetWorkLog("wl-24", "p-2", new DateTime(2020, 9, 4), "James", "Bridge construction", 1800),

                    EntityHelper.GetWorkLog("wl-31", "p-4", new DateTime(2021, 12, 1), "Kumara", "Landscaping", 2000),
                    EntityHelper.GetWorkLog("wl-32", "p-4", new DateTime(2021, 12, 2), "Kumara", "Weed Control", 2000),
                    EntityHelper.GetWorkLog("wl-33", "p-4", new DateTime(2021, 12, 3), "Nihal", "Digging", 1900),
                    EntityHelper.GetWorkLog("wl-34", "p-4", new DateTime(2021, 12, 4), "Nihal", "Planiting seeds", 1900),
                };
                return logs;
            }
        }

        internal static class Transactions
        {
            public static Transaction[] GetTransactions()
            {
                var txns = new Transaction[]
                {
                    EntityHelper.GetTransaction("t-1", "p-1", TransactionCategory.Expense, "lk-11", -100.00, "Daily Wage", new DateTime(2022, 1, 1), true, "u-1"),
                    EntityHelper.GetTransaction("t-2", "p-1", TransactionCategory.Expense, "lk-12", -100.00, "Cement", new DateTime(2022, 1, 5), false, null),
                    EntityHelper.GetTransaction("t-3", "p-1", TransactionCategory.Income, "lk-21", 100.00, "Selling Coconut", new DateTime(2022, 1, 12), true, "u-1"),
                    EntityHelper.GetTransaction("t-4", "p-2", TransactionCategory.Expense, "lk-13", -100.00, "Selling Coconut", new DateTime(2022, 1, 13)),
                    EntityHelper.GetTransaction("t-5", "p-2", TransactionCategory.Income, "lk-21", 100.00, "Selling Coconut", new DateTime(2022, 1, 14)),
                    EntityHelper.GetTransaction("t-6", "p-2", TransactionCategory.Income, "lk-21", 100.00, "Selling Coconut", new DateTime(2022, 1, 15)),
                    EntityHelper.GetTransaction("t-7", "p-2", TransactionCategory.Income, "lk-21", 100.00, "Selling Coconut", new DateTime(2022, 1, 16)),
                    EntityHelper.GetTransaction("t-8", "p-3", TransactionCategory.Income, "lk-21", 100.00, "Selling Coconut", new DateTime(2022, 2, 12)),
                    EntityHelper.GetTransaction("t-9", "p-3", TransactionCategory.Income, "lk-21", 100.00, "Selling Coconut", new DateTime(2022, 3, 1)),
                    EntityHelper.GetTransaction("t-10", "p-3", TransactionCategory.Income, "lk-21", 100.00, "Selling Coconut", new DateTime(2022, 3, 10)),

                };

                return txns;
            }
        }
    }
}
