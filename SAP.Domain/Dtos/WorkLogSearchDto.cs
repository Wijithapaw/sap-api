using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class WorkLogSearchDto : PagedSearch
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string SearchTerm { get; set; }
        public string Projects { get; set; }
    }
}
