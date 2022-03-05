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

                    EntityHelper.GetLookup("lk-21", "h-2", "COCONUT", "Coconut"),
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
    }
}
