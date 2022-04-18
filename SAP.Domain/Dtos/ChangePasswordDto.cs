using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class ChangePasswordDto
    {
        public string CurrentPwd { get; set; }
        public string NewPwd { get; set; }
    }

    public class ChangePasswordResult
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }
}
