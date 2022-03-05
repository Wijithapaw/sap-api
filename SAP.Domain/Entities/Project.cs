﻿using SAP.Domain.Entities.Base;
using SAP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAP.Domain.Entities
{
    public class Project : AuditedEntity
    {
        public string Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectState State { get; set; }
        public string ProjectManagerId { get; set; }

        public virtual User ProjectManager { get; set; }
    }
}
