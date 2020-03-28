namespace Convience.Model.Models.SaasManage
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
