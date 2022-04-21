using SAP.Domain.Entities;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Tests.Helpers
{
    internal static class EntityHelper
    {
        internal static Project GetProject(string id, 
            string name, 
            string desc, 
            DateTime? sDate, 
            DateTime? eDate, 
            ProjectState state,
            string projectManagerId = null)
        {
            var project = new Project
            {
                Id = id,
                Name = name,
                Description = desc,
                StartDate = sDate,
                EndDate = eDate,
                State = state,
                ProjectManagerId = projectManagerId
            };
            return project;
        }

        internal static LookupHeader GetLookupHeader(string id, string code, string name)
        {
            var lookupHeader = new LookupHeader
            {
                Id = id,
                Code = code,
                Name = name
            };
            return lookupHeader;
        }

        internal static Lookup GetLookup(string id, 
            string headerId, 
            string code, 
            string name, 
            bool inactive = false,
            bool protectedLookup = false)
        {
            var lookup = new Lookup
            {
                Id = id,
                HeaderId = headerId,
                Code = code,
                Name = name,
                Inactive = inactive,
                Protected = protectedLookup,
            };
            return lookup;
        }

        internal static User GetUser(string id, string fName, string lName, string email)
        {
            var user = new User
            {
                Id = id,
                FirstName = fName,
                LastName = lName,
                Email = email,
                UserName = email,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper(),
            };

            return user;
        }

        internal static Tag GetTag(string id, string name)
        {
            var tag = new Tag
            {
                Id = id,
                Name = name,
            };
            return tag;
        }

        internal static Transaction GetTransaction(string id,
            string projectId,
            TransactionCategory category,
            string typeId,
            double amount,
            string description,
            DateTime date,
            bool reconciled = false,
            string reconciledById = null)
        {
            var txn = new Transaction
            {
                Id = id,
                Category = category,
                TypeId = typeId,
                Date = date,
                Description = description,
                Amount = amount,
                ProjectId = projectId,
                Reconciled = reconciled,
                ReconciledById = reconciledById,
            };
            return txn;
        }
    }
}
