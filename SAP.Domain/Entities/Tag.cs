using SAP.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAP.Domain.Entities
{
    public class Tag : AuditedEntity
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false), MinLength(3)]
        public string Name { get; set; }
    }
}
