using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class LoginResultDto
    {
        public bool Succeeded { get; set; }
        public string ErrorCode { get; set; }
        public string AuthToken { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
