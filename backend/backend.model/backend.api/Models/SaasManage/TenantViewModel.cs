using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Model.backend.api.Models.SaasManage
{
    public class TenantViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UrlPrefix { get; set; }

        public int DataBaseType { get; set; }

        public string ConnectionString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
