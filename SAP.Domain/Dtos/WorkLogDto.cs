﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class WorkLogDto
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string LabourName { get; set; }
        public string JobDescription { get; set; }
        public DateTime Date { get; set; }
        public double? Wage { get; set; }
    }
}
