using Microsoft.AspNetCore.Identity;
using SAP.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Entities
{
    public class User : IdentityUser, IAuditedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDateUtc { get; set; }
    }
}
