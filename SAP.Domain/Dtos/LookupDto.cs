using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class LookupDto
    {
        public string Id { get; set; }
        public string HeaderId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Inactive { get; set; }
    }
}
