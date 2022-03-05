using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public interface IRequestContext
    {
        public string UserId { get; set; }
        public string[] PermissionClaims { get; set; }

        public bool HasPermission(string permission);
    }
}
