using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectState State { get; set; }
        public string ProjectManagerId { get; set; }
        public string ProjectManager { get; set; }
    }
}
