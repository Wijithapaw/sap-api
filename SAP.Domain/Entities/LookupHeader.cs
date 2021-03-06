using SAP.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAP.Domain.Entities
{
    public class LookupHeader: AuditedEntity
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}
