using SAP.Domain.Entities;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Tests.Helpers
{
    internal static class EntityHelper
    {
        internal static Project CreateProject(string id, 
            string title, 
            string desc, 
            DateTime? sDate, 
            DateTime? eDate, 
            ProjectState state)
        {
            var project = new Project
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
