using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Entities
{
    public class ProjectTag
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string TagId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
