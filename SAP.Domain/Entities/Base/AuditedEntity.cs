using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Entities.Base
{
    public class AuditedEntity : IAuditedEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }
    }
}
