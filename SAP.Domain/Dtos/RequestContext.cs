using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class RequestContext : IRequestContext
    {
        public string UserId { get; set; }
        public string[] PermissionClaims { get; set; }

        public bool HasPermission(string permission)
        {
            return PermissionClaims?.Any(p => p.ToLower() == permission.ToLower()) ?? false;
        }
    }
}
