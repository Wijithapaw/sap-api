using SAP.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SAP.Domain.Entities
{
    public class Lookup : AuditedEntity
    {
        public string Id { get; set; }
        [Required]
        public string HeaderId { get; set; }
        [Required(AllowEmptyStrings = false), MinLength(2)]
        public string Code { get; set; }
        [Required(AllowEmptyStrings = false), MinLength(2)]
        public string Name { get; set; }
        public bool Inactive { get; set; }

        public virtual LookupHeader Header { get; set; }
    }
}
