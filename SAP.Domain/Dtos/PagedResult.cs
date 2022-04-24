using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class PagedResult<T>
    {
        public int Total { get; set; }
        public List<T> Items { get; set; }
    }
}
