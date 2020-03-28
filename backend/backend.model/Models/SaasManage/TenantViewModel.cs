using System;

namespace Convience.Model.Models.SaasManage
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
