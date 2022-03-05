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
            string title,
            string desc,
            DateTime? sDate,
            DateTime? eDate,
            ProjectState state,
            string projectManagerId = null)
        {
            var project = new ProjectDto
            {
                Id = id,
                Title = title,
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
    }
}
