using System;
using System.Collections.Generic;
using System.Text;

namespace SAP.Domain.Dtos
{
    public class ListItemDto
    {
        public ListItemDto()
        {
        }

        public ListItemDto(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public ListItemDto(string key, string value, bool inactive)
        {
            Key = key;
            Value = value;
            Inactive = inactive;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public bool? Inactive { get; set; }
    }
}
