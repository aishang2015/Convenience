using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model.backend.api.Models.SaasManage
{
    public class TenantQuery
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string Name { get; set; }

        public string DataBaseType { get; set; }

        public string SortKey { get; set; }

        public bool isDesc { get; set; }
    }
}
