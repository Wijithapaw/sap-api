using SAP.Domain.Dtos;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Tests.Helpers
{
    internal static class DtoHelper
    {
        internal static ProjectDto CreateProjectDto(string id,
            string title,
            string desc,
            DateTime? sDate,
            DateTime? eDate,
            ProjectState state)
        {
            var project = new ProjectDto
            {
                Id = id,
                Title = title,
                Description = desc,
                StartDate = sDate,
                EndDate = eDate,
                State = state
            };
            return project;
        }
    }
}
