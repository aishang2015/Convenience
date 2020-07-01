using System;

namespace Convience.Model.Models.SaasManage
{
    public class TenantQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }

        public string Name { get; set; }

        public string DataBaseType { get; set; }

        public string SortKey { get; set; }

        public bool isDesc { get; set; }
    }

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

    public class TenantResultModel : TenantViewModel
    {

    }
}
