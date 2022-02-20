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
    }
}
