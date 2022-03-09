using SAP.Domain.Dtos;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Tests.Helpers
{
    internal static class DtoHelper
    {
        internal static ProjectDto GetProjectDto(string id,
            string name,
            string desc,
            DateTime? sDate,
            DateTime? eDate,
            ProjectState state,
            string projectManagerId = null)
        {
            var project = new ProjectDto
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

        internal static LookupDto GetLookupDto(string id, string headerId, string code, string name, bool inactive = false)
        {
            var lookup = new LookupDto
            {
                Id = id,
                HeaderId = headerId,
                Code = code,
                Name = name,
                Inactive = inactive
            };

            return lookup;
        }

        internal static TransactionDto GetTransactionDto(string id,
            string projectId,
            TransactionCategory category, 
            string typeId,
            double amount,
            string description,
            DateTime date)
        {
            var dto = new TransactionDto
            {
                Id = id,
                Category = category,
                TypeId = typeId,
                Date = date,
                Description = description,
                Amount = amount,
                ProjectId = projectId,
            };
            return dto;
        }
    }
}
