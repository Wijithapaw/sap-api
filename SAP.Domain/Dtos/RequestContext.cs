using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class RequestContext : IRequestContext
    {
        public string UserId { get; set; }
    }
}
